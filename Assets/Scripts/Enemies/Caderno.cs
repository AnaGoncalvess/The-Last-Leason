using System.Collections;
using UnityEngine;

public class Caderno : MonoBehaviour
{
    [Header("Configurações de Movimento e Visão")]
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private float maxVision = 6f;
    [SerializeField] private float attackDistance = 1.8f;

    [Header("Configurações de Combate")]
    [SerializeField] private int maxLife = 3;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackAnimDuration = 0.6f;
    [SerializeField] private float shootDelay = 0.3f;

    [Header("Ataque à Distância")]
    [SerializeField] private GameObject bolinhaPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Referências")]
    [SerializeField] private Transform player;

    // Componentes internos
    private Rigidbody2D rb;
    private Animator anim;

    // Estado interno
    private int currentLife;
    private float nextAttackTime;
    private bool isDead;
    private bool isTakingHit;
    private bool isAttacking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        currentLife = maxLife;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    private void Update()
    {
        if (isDead || isTakingHit || player == null)
            return;

        IAController();
    }

    private void IAController()
    {
        // 1. Sempre vira a sprite para encarar o Player quando não estiver no meio da animação de ataque
        if (!isAttacking)
        {
            LookAtPlayer();
        }

        float distance = Vector2.Distance(transform.position, player.position);

        // 2. Se estiver no meio do ataque, mantém parado
        if (isAttacking)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        // 3. Fora de visão: Idle
        if (distance > maxVision)
        {
            Idle();
            return;
        }

        // 4. Dentro da visão, mas fora do alcance de ataque: Persegue
        if (distance > attackDistance)
        {
            ChasePlayer();
        }
        // 5. No alcance de ataque: Ataca mantendo a distância configurada
        else
        {
            Attack();
        }
    }

    private void LookAtPlayer()
    {
        // Vira para a direita se o Player estiver à direita; para a esquerda se estiver à esquerda
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void Idle()
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        SetTransition(0);
    }

    private void ChasePlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * speed, rb.linearVelocity.y);

        SetTransition(1);
    }

    private void Attack()
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        if (Time.time >= nextAttackTime && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
        else
        {
            SetTransition(0);
        }
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;

        anim.SetTrigger("attack");

        yield return new WaitForSeconds(shootDelay);
        
        Shoot();

        yield return new WaitForSeconds(Mathf.Max(0, attackAnimDuration - shootDelay));

        isAttacking = false;
        SetTransition(0);
    }

    private void Shoot()
    {
        if (bolinhaPrefab == null) return;

        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
        GameObject bolinhaObj = Instantiate(bolinhaPrefab, spawnPosition, Quaternion.identity);

        BolinhaDePapel bolinha = bolinhaObj.GetComponent<BolinhaDePapel>();

        if (bolinha != null)
        {
            float direction = transform.localScale.x >= 0 ? 1f : -1f;
            bolinha.SetDirection(direction);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentLife -= damageAmount;
        isTakingHit = true;
        isAttacking = false;

        anim.SetTrigger("hit");

        if (currentLife <= 0)
        {
            Die();
        }
        else
        {
            Invoke(nameof(ResetHit), 0.3f);
        }
    }

    private void ResetHit()
    {
        isTakingHit = false;
    }

    private void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        anim.SetTrigger("death");

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 2f);
    }

    private void SetTransition(int value)
    {
        anim.SetInteger("transition", value);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxVision);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
