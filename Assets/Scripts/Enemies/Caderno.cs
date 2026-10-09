// // using System.Collections;
// // using UnityEngine;

// // public class Caderno : MonoBehaviour
// // {
// //     [Header("Configurações de Movimento e Visão")]
// //     [SerializeField] private float speed = 2.5f;
// //     [SerializeField] private float maxVision = 6f;
// //     [SerializeField] private float attackDistance = 1.8f;

// //     [Header("Configurações de Combate")]
// //     [SerializeField] private int maxLife = 3;
// //     [SerializeField] private float attackCooldown = 1.5f;
// //     [SerializeField] private float attackAnimDuration = 0.6f;
// //     [SerializeField] private float shootDelay = 0.3f;

// //     [Header("Ataque à Distância")]
// //     [SerializeField] private GameObject bolinhaPrefab;
// //     [SerializeField] private Transform firePoint;

// //     [Header("Referências")]
// //     [SerializeField] private Transform player;

// //     // Componentes internos
// //     private Rigidbody2D rb;
// //     private Animator anim;

// //     // Estado interno
// //     [SerializeField] private int currentLife;
// //     private float nextAttackTime;
// //     private bool isDead;
// //     private bool isTakingHit;
// //     private bool isAttacking;

// //     private void Awake()
// //     {
// //         rb = GetComponent<Rigidbody2D>();
// //         anim = GetComponent<Animator>();
// //     }

// //     private void Start()
// //     {
// //         currentLife = maxLife;

// //         if (player == null)
// //         {
// //             GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
// //             if (playerObj != null)
// //             {
// //                 player = playerObj.transform;
// //             }
// //         }
// //     }

// //     private void Update()
// //     {
// //         if (isDead || isTakingHit || player == null)
// //             return;

// //         IAController();
// //     }

// //     private void IAController()
// //     {
// //         // 1. Sempre vira a sprite para encarar o Player quando não estiver no meio da animação de ataque
// //         if (!isAttacking)
// //         {
// //             LookAtPlayer();
// //         }

// //         float distance = Vector2.Distance(transform.position, player.position);

// //         // 2. Se estiver no meio do ataque, mantém parado
// //         if (isAttacking)
// //         {
// //             rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
// //             return;
// //         }

// //         // 3. Fora de visão: Idle
// //         if (distance > maxVision)
// //         {
// //             Idle();
// //             return;
// //         }

// //         // 4. Dentro da visão, mas fora do alcance de ataque: Persegue
// //         if (distance > attackDistance)
// //         {
// //             ChasePlayer();
// //         }
// //         // 5. No alcance de ataque: Ataca mantendo a distância configurada
// //         else
// //         {
// //             Attack();
// //         }
// //     }

// //     private void LookAtPlayer()
// //     {
// //         // Vira para a direita se o Player estiver à direita; para a esquerda se estiver à esquerda
// //         if (player.position.x > transform.position.x)
// //         {
// //             transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
// //         }
// //         else if (player.position.x < transform.position.x)
// //         {
// //             transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
// //         }
// //     }

// //     private void Idle()
// //     {
// //         rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
// //         SetTransition(0);
// //     }

// //     private void ChasePlayer()
// //     {
// //         Vector2 dir = (player.position - transform.position).normalized;
// //         rb.linearVelocity = new Vector2(dir.x * speed, rb.linearVelocity.y);

// //         SetTransition(1);
// //     }

// //     private void Attack()
// //     {
// //         rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

// //         if (Time.time >= nextAttackTime && !isAttacking)
// //         {
// //             StartCoroutine(AttackRoutine());
// //         }
// //         else
// //         {
// //             SetTransition(0);
// //         }
// //     }

// //     private IEnumerator AttackRoutine()
// //     {
// //         isAttacking = true;
// //         nextAttackTime = Time.time + attackCooldown;

// //         anim.SetTrigger("attack");

// //         yield return new WaitForSeconds(shootDelay);

// //         Shoot();

// //         yield return new WaitForSeconds(Mathf.Max(0, attackAnimDuration - shootDelay));

// //         isAttacking = false;
// //         SetTransition(0);
// //     }

// //     private void Shoot()
// //     {
// //         if (bolinhaPrefab == null) return;

// //         Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
// //         GameObject bolinhaObj = Instantiate(bolinhaPrefab, spawnPosition, Quaternion.identity);

// //         BolinhaDePapel bolinha = bolinhaObj.GetComponent<BolinhaDePapel>();

// //         if (bolinha != null)
// //         {
// //             float direction = transform.localScale.x >= 0 ? 1f : -1f;
// //             bolinha.SetDirection(direction);
// //         }
// //     }

// //     public void TakeDamage(int damageAmount)
// //     {
// //         if (isDead) return;

// //         currentLife -= damageAmount;
// //         isTakingHit = true;
// //         isAttacking = false;

// //         anim.SetTrigger("hit");

// //         if (currentLife <= 0)
// //         {
// //             Die();
// //         }
// //         else
// //         {
// //             Invoke(nameof(ResetHit), 0.3f);
// //         }
// //     }

// //     private void ResetHit()
// //     {
// //         isTakingHit = false;
// //     }

// //     // private void Die()
// //     // {
// //     //     isDead = true;
// //     //     rb.linearVelocity = Vector2.zero;
// //     //     anim.SetTrigger("death");

// //     //     Collider2D col = GetComponent<Collider2D>();
// //     //     if (col != null) col.enabled = false;

// //     //     Destroy(gameObject, 2f);
// //     // }

// //     [Header("Configurações de Drop")]
// //     [SerializeField] private bool dropsCristal = true; // Desmarque no Inspector para o Boss Final
// //     [SerializeField] private GameObject cristalPrefab;

// //     private void Die()
// //     {
// //         isDead = true;
// //         rb.linearVelocity = Vector2.zero;
// //         anim.SetTrigger("death");

// //         // Instancia o cristal na posição atual do inimigo ao morrer
// //         if (dropsCristal && cristalPrefab != null)
// //         {
// //             Instantiate(cristalPrefab, transform.position, Quaternion.identity);
// //         }

// //         Collider2D col = GetComponent<Collider2D>();
// //         if (col != null) col.enabled = false;

// //         Destroy(gameObject, 2f);
// //     }

// //     private void SetTransition(int value)
// //     {
// //         anim.SetInteger("transition", value);
// //     }

// //     private void OnDrawGizmosSelected()
// //     {
// //         Gizmos.color = Color.yellow;
// //         Gizmos.DrawWireSphere(transform.position, maxVision);

// //         Gizmos.color = Color.red;
// //         Gizmos.DrawWireSphere(transform.position, attackDistance);
// //     }
// // }


// using System;
// using UnityEngine;

// public class BolinhaDePapel : MonoBehaviour
// {
//     [Header("Configurações do Projétil")]
//     [SerializeField] private float speed = 7f;
//     [SerializeField] private float damage = 0.4f;
//     [SerializeField] private float lifetime = 3f; // Destrói (ou devolve ao pool) após alguns segundos se não acertar nada

//     private Vector2 moveDirection;
//     private Rigidbody2D rb;
//     private float timeAlive;

//     // Definido pelo spawner (ex: Caderno) quando o projétil vem de um pool.
//     // Se ninguém chamar Initialize, o projétil se comporta normalmente e usa Destroy().
//     private Action<BolinhaDePapel> releaseToPool;

//     private void Awake()
//     {
//         rb = GetComponent<Rigidbody2D>();
//     }

//     private void OnEnable()
//     {
//         timeAlive = 0f;
//     }

//     public void Initialize(Action<BolinhaDePapel> releaseCallback)
//     {
//         releaseToPool = releaseCallback;
//     }

//     // Método chamado pelo Caderno no momento do disparo para definir a direção (1 para direita, -1 para esquerda)
//     public void SetDirection(float directionX)
//     {
//         // Define a direção horizontal do movimento
//         moveDirection = new Vector2(directionX, 0).normalized;

//         // Caso a sprite da bolinha tenha lado, podemos virá-la também
//         transform.localScale = directionX < 0 ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
//     }

//     private void Update()
//     {
//         timeAlive += Time.deltaTime;
//         if (timeAlive >= lifetime)
//         {
//             Return();
//         }
//     }

//     private void FixedUpdate()
//     {
//         // Move o projétil em linha reta
//         if (rb != null)
//         {
//             rb.linearVelocity = moveDirection * speed;
//         }
//     }

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         // Verifica se colidiu com o Player
//         if (collision.CompareTag("Player"))
//         {
//             // Tenta chamar o método OnHit ou dar dano no Player
//             Player player = collision.GetComponent<Player>();
//             if (player != null)
//             {
//                 player.OnHit();
//             }

//             Return();
//         }
//         // Destrói/devolve a bolinha se bater no chão/obstáculos (Layer do cenário, ex: Layer 6 do seu projeto)
//         else if (collision.gameObject.layer == 6)
//         {
//             Return();
//         }
//     }

//     private void Return()
//     {
//         if (releaseToPool != null)
//         {
//             releaseToPool(this);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }
// }


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

    [Header("Sons (opcionais)")]
    [SerializeField] private AudioClip attackSfx;
    [SerializeField] private AudioClip hitSfx;
    [SerializeField] private AudioClip deathSfx;

    // Componentes internos
    private Rigidbody2D rb;
    private Animator anim;
    private ObjectPool<BolinhaDePapel> bolinhaPool;

    // Estado interno
    private int currentLife;
    private float nextAttackTime;
    private bool isDead;
    private bool isTakingHit;
    private bool isAttacking;

    private bool estaVisivelNaTela = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (bolinhaPrefab != null)
        {
            BolinhaDePapel bolinhaComponent = bolinhaPrefab.GetComponent<BolinhaDePapel>();
            if (bolinhaComponent != null)
            {
                bolinhaPool = new ObjectPool<BolinhaDePapel>(bolinhaComponent);
            }
        }
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

    // private void IAController()
    // {
    //     // 1. Sempre vira a sprite para encarar o Player quando não estiver no meio da animação de ataque
    //     if (!isAttacking)
    //     {
    //         LookAtPlayer();
    //     }

    //     float distance = Vector2.Distance(transform.position, player.position);

    //     // 2. Se estiver no meio do ataque, mantém parado
    //     if (isAttacking)
    //     {
    //         rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    //         return;
    //     }

    //     // 3. Fora de visão: Idle
    //     if (distance > maxVision)
    //     {
    //         Idle();
    //         return;
    //     }

    //     // 4. Dentro da visão, mas fora do alcance de ataque: Persegue
    //     if (distance > attackDistance)
    //     {
    //         ChasePlayer();
    //     }
    //     // 5. No alcance de ataque: Ataca mantendo a distância configurada
    //     else
    //     {
    //         Attack();
    //     }
    // }

private void IAController()
    {
        // 1. Sempre vira a sprite para encarar o Player quando não estiver atacando
        if (!isAttacking)
        {
            LookAtPlayer();
        }

        // Se o Caderno NÃO estiver visível na câmera do jogador, ele permanece em descanso (Idle)
        if (!estaVisivelNaTela)
        {
            Idle();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        // 2. Se estiver no meio da animação de ataque, mantém parado
        if (isAttacking)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        // 3. Fora do raio de visão: Idle
        if (distance > maxVision)
        {
            Idle();
            return;
        }

        // 4. Dentro do raio de visão, mas longe para atacar: Persegue
        if (distance > attackDistance)
        {
            ChasePlayer();
        }
        // 5. No alcance de ataque: Ataca apenas se estiver na tela
        else
        {
            Attack();
        }
    }

    private void Attack()
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        // Só inicia a rotina de ataque se já estiver visível para o jogador
        if (Time.time >= nextAttackTime && !isAttacking && estaVisivelNaTela)
        {
            StartCoroutine(AttackRoutine());
        }
        else
        {
            SetTransition(0);
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

    // private void Attack()
    // {
    //     rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

    //     if (Time.time >= nextAttackTime && !isAttacking)
    //     {
    //         StartCoroutine(AttackRoutine());
    //     }
    //     else
    //     {
    //         SetTransition(0);
    //     }
    // }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;

        anim.SetTrigger("attack");
        PlaySfx(attackSfx);

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
        float direction = transform.localScale.x >= 0 ? 1f : -1f;

        BolinhaDePapel bolinha = bolinhaPool != null
            ? bolinhaPool.Get(spawnPosition, Quaternion.identity)
            : Instantiate(bolinhaPrefab, spawnPosition, Quaternion.identity).GetComponent<BolinhaDePapel>();

        if (bolinha != null)
        {
            bolinha.SetDirection(direction);

            if (bolinhaPool != null)
            {
                bolinha.Initialize(b => bolinhaPool.Release(b));
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentLife -= damageAmount;
        isTakingHit = true;
        isAttacking = false;

        anim.SetTrigger("hit");
        PlaySfx(hitSfx);

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


    [Header("Configurações de Drop")]
    [SerializeField] private bool dropsCristal = true; // Desmarque no Inspector para o Boss Final
    [SerializeField] private GameObject cristalPrefab;

    private void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        anim.SetTrigger("death");
        PlaySfx(deathSfx);
        CameraTargetController.Shake(0.08f, 0.12f);

        if (dropsCristal && cristalPrefab != null)
        {
            Instantiate(cristalPrefab, transform.position, Quaternion.identity);
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 2f);
    }


    private void PlaySfx(AudioClip clip)
    {
        if (clip == null) return;
        AudioManager.Instance.PlaySfx(clip);
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

    private void OnBecameVisible()
    {
        estaVisivelNaTela = true;
    }

    private void OnBecameInvisible()
    {
        estaVisivelNaTela = false;
    }


}
