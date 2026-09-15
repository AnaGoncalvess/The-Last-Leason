using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Configurações do Player")]
    private float direction;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float health = 1.2f;
    [SerializeField] private float jumpForce = 10f;
    private bool isJumping;
    private bool isAttacking;

    [Header("Sistema de Dano e Imunidade")]
    [SerializeField] private float invincibilityCooldown = 1.0f;
    private float lastHitTime;
    private bool isDead;

    [Header("Bloqueio de Câmera")]
    [SerializeField] private CameraTargetController cameraController;

    [Header("Referências Externas")]
    [SerializeField] private GameOverManager gameOverManager;
    [SerializeField] private HealthUI healthUI;
    private int hitsReceived = 0;


    public Transform point;
    public float radius;


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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isDead) return;

        OnJump();
        OnAttack();
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        OnMove();
    }

    private void OnMove()
    {
        // Se estiver atacando, trava a movimentação horizontal
        if (attackAnim)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        direction = Input.GetAxis("Horizontal");

        if (direction < 0 && cameraController != null && cameraController.IsBlockingBackwardMovement)
        {
            direction = 0;
        }

        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

        if (direction < 0)
        {
            if (!isJumping) runAnim = true;
            transform.eulerAngles = new Vector2(0, 180);
        }
        else if (direction > 0)
        {
            if (!isJumping) runAnim = true;
            transform.eulerAngles = new Vector2(0, 0);
        }
        else if (direction == 0 && !isJumping && !isAttacking)
        {
            idleAnim = true;
        }
    }

    private void OnJump()
    {
        if (Input.GetButtonDown("Jump") && !isJumping && !attackAnim)
        {
            jumpAnim = true;
            isJumping = true;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    [Header("Configurações de Ataque")]
    [SerializeField] private Transform attackPoint; // Ponto de origem do golpe (na mão/espada do Player)
    [SerializeField] private float attackRange = 0.5f; // Raio da área de impacto
    [SerializeField] private LayerMask enemyLayer; // Layer onde o Caderno se encontra
    [SerializeField] private int attackDamage = 1;

//     void OnAttack()
// {
//     // Só permite atacar se NÃO estiver atacando no momento
//     if (Input.GetButtonDown("Fire1") && !attackAnim )
//     {
//         attackAnim = true;

//         isAttacking = true;

//         // Executa o dano no momento do clique
//         if (point != null)
//         {
//             Collider2D hit = Physics2D.OverlapCircle(point.position, radius, enemyLayer);

//             if (hit != null)
//             {
//                 Caderno caderno = hit.GetComponent<Caderno>();
//                 if (caderno != null)
//                 {
//                     caderno.TakeDamage(1);
//                 }
//             }
//         }

//         // Reseta o estado do ataque acompanhando a duração da animação
//         StartCoroutine(ResetAttack());
//     }
// }

void OnAttack()
    {
        // Só permite atacar se NÃO estiver atacando no momento
        if (Input.GetButtonDown("Fire1") && !attackAnim)
        {
            attackAnim = true;
            isAttacking = true;

            // Executa a detecção de dano
            if (point != null)
            {
                // Busca TODOS os colliders na área de impacto com a Layer selecionada
                Collider2D[] hits = Physics2D.OverlapCircleAll(point.position, radius, enemyLayer);

                foreach (Collider2D hit in hits)
                {
                    // Tenta pegar o Caderno no objeto atingido ou nos seus pais
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

            // Reseta o estado do ataque acompanhando a duração da animação
            StartCoroutine(ResetAttack());
        }
    }
IEnumerator ResetAttack()
{
    // Tempo aproximado da animação do ataque tocar por inteiro
    yield return new WaitForSeconds(0.4f);
    attackAnim = false;
    isAttacking = false;
}

    public void OnHit()
    {
        if (isDead) return;

        if (Time.time >= lastHitTime + invincibilityCooldown)
        {
            lastHitTime = Time.time;
            hitsReceived++;

            if (healthUI != null)
            {
                healthUI.UpdateHealthUI(hitsReceived);
            }

            if (hitsReceived < 4)
            {
                hitAnim = true;
                health -= 0.3f;
            }
            else
            {
                health = 0f;
                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true;
        deathAnim = true;
        rb.linearVelocity = Vector2.zero;

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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isJumping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            OnHit();
        }
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(point.position, radius);
    }
}
