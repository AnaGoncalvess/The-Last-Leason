using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Configurações do Player")]
    private float direction;
    [SerializeField] private float speed = 9f;
    [SerializeField] private float health = 1.2f;
    [SerializeField] private float jumpForce = 19f;
    private bool isJumping;
    private bool isAttacking;

    // Entradas Virtuais para Mobile (Android/iOS)
    private float mobileHorizontalInput = 0f;

    [Header("Sistema de Dano e Imunidade")]
    [SerializeField] private float invincibilityCooldown = 1.0f;
    [SerializeField] private int maxHits = 4;
    private float lastHitTime;
    private bool isDead;

    [Header("Bloqueio de Câmera")]
    [SerializeField] private CameraTargetController cameraController;

    [Header("Referências Externas")]
    [SerializeField] private GameOverManager gameOverManager;
    [SerializeField] private HealthUI healthUI;
    private static int hitsReceived = 0;

    public Transform point;
    public float radius;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer = 1 << 6;
    [SerializeField] private LayerMask hazardLayer = 1 << 7;

    [Header("Pulo")]
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float jumpBufferTime = 0.15f;
    private int jumpsUsed;
    private bool jumpBuffered;
    private float jumpBufferDeadline;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 24f;
    [SerializeField] private float dashDuration = 0.16f;
    [SerializeField] private float dashCooldown = 0.35f;
    private bool isDashing;
    private float dashEndTime;
    private float nextDashTime;
    private float dashDirection = 1f;
    private TrailRenderer dashTrail;

    [Header("Sons (opcionais)")]
    [SerializeField] private AudioClip jumpSfx;
    [SerializeField] private AudioClip dashSfx;
    [SerializeField] private AudioClip attackSfx;
    [SerializeField] private AudioClip hitSfx;
    [SerializeField] private AudioClip deathSfx;

    // Controle de Animações
    private bool idleAnim;
    private bool runAnim;
    private bool jumpAnim;
    private bool attackAnim;
    private bool hitAnim;
    private bool deathAnim;

    public bool IdleAnim { get => idleAnim; set => idleAnim = value; }
    public bool RunAnim { get => runAnim; set => runAnim = value; }
    public bool JumpAnim { get => jumpAnim; set => jumpAnim = value; }
    public bool AttackAnim { get => attackAnim; set => attackAnim = value; }
    public bool HitAnim { get => hitAnim; set => hitAnim = value; }
    public bool DeathAnim { get => deathAnim; set => deathAnim = value; }
    public bool IsDashing => isDashing;


    private void Start()
    {
        // Se o jogador já tinha tomado dano nas fases anteriores, 
        // atualiza os corações da tela logo no início do nível!
        if (hitsReceived > 0 && healthUI != null)
        {
            healthUI.UpdateHealthUI(hitsReceived);
        }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        dashTrail = gameObject.AddComponent<TrailRenderer>();
        dashTrail.time = 0.18f;
        dashTrail.startWidth = 0.2f;
        dashTrail.endWidth = 0f;
        dashTrail.minVertexDistance = 0.05f;
        dashTrail.material = new Material(Shader.Find("Sprites/Default"));
        dashTrail.startColor = new Color(1f, 1f, 1f, 0.5f);
        dashTrail.endColor = new Color(1f, 1f, 1f, 0f);
        dashTrail.emitting = false;
    }

    private void Update()
    {
        if (isDead || (PauseManager.Instance != null && PauseManager.Instance.IsPaused)) return;

        // Leitura combinada: Teclado ou Controles Touch Mobile
        if (Input.GetButtonDown("Jump")) TriggerJump();
        if (Input.GetButtonDown("Fire1")) TriggerAttack();
        if (Input.GetButtonDown("Fire3")) TriggerDash();

        UpdateDash();
    }

    private void FixedUpdate()
    {
        if (isDead || (PauseManager.Instance != null && PauseManager.Instance.IsPaused)) return;

        OnMove();
    }

    private void OnMove()
    {
        if (isDashing) return;

        if (isAttacking)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        // Soma entrada do Teclado com a do Touch Mobile
        direction = Input.GetAxis("Horizontal");
        if (direction == 0) direction = mobileHorizontalInput;

        if (direction < 0 && cameraController != null && cameraController.IsBlockingBackwardMovement)
        {
            direction = 0;
        }

        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

        if (direction < 0)
        {
            dashDirection = -1f;
            if (!isJumping) runAnim = true;
            transform.eulerAngles = new Vector2(0, 180);
        }
        else if (direction > 0)
        {
            dashDirection = 1f;
            if (!isJumping) runAnim = true;
            transform.eulerAngles = new Vector2(0, 0);
        }
        else if (direction == 0 && !isJumping && !isAttacking)
        {
            idleAnim = true;
        }
    }

    // --- MÉTODOS PÚBLICOS PARA BOTÕES TOUCH (MOBILE) ---

    public void MoveMobile(float input)
    {
        mobileHorizontalInput = input;
    }

    public void TriggerJump()
    {
        if (CanJump())
        {
            PerformJump();
        }
        else
        {
            jumpBuffered = true;
            jumpBufferDeadline = Time.time + jumpBufferTime;
        }
    }

    public void TriggerAttack()
    {
        if (!isDashing)
        {
            if (!isAttacking)
            {
                StartAttack();
            }
            else
            {
                attackQueued = true;
            }
        }
    }

    public void TriggerDash()
    {
        if (!isDashing && !isAttacking && Time.time >= nextDashTime)
        {
            isDashing = true;
            dashEndTime = Time.time + dashDuration;
            nextDashTime = Time.time + dashCooldown;

            dashTrail.emitting = true;
            PlaySfx(dashSfx);
        }
    }

    // --- LÓGICA INTERNA DE JOGO ---

    private bool CanJump()
    {
        return !isAttacking && !isDashing && jumpsUsed < maxJumps;
    }

    private void PerformJump()
    {
        jumpAnim = true;
        isJumping = true;
        jumpsUsed++;
        jumpBuffered = false;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        PlaySfx(jumpSfx);
    }

    private void UpdateDash()
    {
        if (!isDashing) return;

        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);

        if (Time.time >= dashEndTime)
        {
            isDashing = false;
            dashTrail.emitting = false;
        }
    }

    [Header("Configurações de Ataque")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackLockDuration = 0.3f;
    private bool attackQueued;

    private void StartAttack()
    {
        attackAnim = true;
        isAttacking = true;
        attackQueued = false;

        PlaySfx(attackSfx);

        if (attackPoint != null)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

            foreach (Collider2D hit in hits)
            {
                Caderno caderno = hit.GetComponentInParent<Caderno>();
                if (caderno == null)
                {
                    caderno = hit.GetComponent<Caderno>();
                }

                if (caderno != null)
                {
                    caderno.TakeDamage(attackDamage);
                }
            }
        }

        StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackLockDuration);
        attackAnim = false;
        isAttacking = false;

        if (attackQueued)
        {
            StartAttack();
        }
    }

    public void OnHit()
    {
        if (isDead || isDashing) return;

        if (Time.time >= lastHitTime + invincibilityCooldown)
        {
            lastHitTime = Time.time;
            hitsReceived++;

            if (healthUI != null)
            {
                healthUI.UpdateHealthUI(hitsReceived);
            }

            if (hitsReceived < maxHits)
            {
                hitAnim = true;
                health -= 0.3f;
                PlaySfx(hitSfx);
                CameraTargetController.Shake(0.12f, 0.15f);
            }
            else
            {
                health = 0f;
                Die();
            }
        }
    }

    public void KillInstantly()
    {
        if (isDead) return;
        health = 0f;
        Die();
    }


    private void Die()
    {
        isDead = true;
        deathAnim = true;
        rb.linearVelocity = Vector2.zero;

        hitsReceived = 0;
        ColetaveisManager.ResetarProgressoGlobal();

        PlaySfx(deathSfx);
        CameraTargetController.Shake(0.2f, 0.3f);

        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(1.2f);

        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver();
        }
    }

    private void PlaySfx(AudioClip clip)
    {
        if (clip == null) return;
        AudioManager.Instance.PlaySfx(clip);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsInLayerMask(collision.gameObject.layer, groundLayer))
        {
            isJumping = false;
            jumpsUsed = 0;

            if (jumpBuffered && Time.time <= jumpBufferDeadline && CanJump())
            {
                PerformJump();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsInLayerMask(collision.gameObject.layer, hazardLayer))
        {
            OnHit();
        }
    }

    private static bool IsInLayerMask(int layer, LayerMask mask)
    {
        return (mask.value & (1 << layer)) != 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }

        if (point != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(point.position, radius);
        }
    }
}