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

//         if (collision.GetComponentInParent<Caderno>() != null) return;
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


// using System;
// using UnityEngine;

// public class BolinhaDePapel : MonoBehaviour
// {
//     [Header("Configurações do Projétil")]
//     [SerializeField] private float speed = 7f;
//     [SerializeField] private float damage = 0.4f;
//     [SerializeField] private float lifetime = 3f;

//     private Vector2 moveDirection;
//     private Rigidbody2D rb;
//     private float timeAlive;
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

//     public void SetDirection(float directionX)
//     {
//         moveDirection = new Vector2(directionX, 0).normalized;
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
//         if (rb != null)
//         {
//             rb.linearVelocity = moveDirection * speed;
//         }
//     }

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         // Ignora colisão com o próprio Caderno ou outros inimigos
//         if (collision.GetComponentInParent<Caderno>() != null) return;

//         // Verifica se colidiu com o Player (mesmo se acertar um colisor filho)
//         Player player = collision.GetComponent<Player>();
//         if (player == null)
//         {
//             player = collision.GetComponentInParent<Player>();
//         }

//         if (player != null)
//         {
//             player.TomarDano(damage);
//             Return();
//             return;
//         }

//         // Destrói/devolve a bolinha se bater no chão/obstáculos (Layer 6)
//         if (collision.gameObject.layer == 6)
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

using System;
using UnityEngine;

public class BolinhaDePapel : MonoBehaviour
{
    [Header("Configurações do Projétil")]
    [SerializeField] private float speed = 7f;
    [SerializeField] private float damage = 0.4f;
    [SerializeField] private float lifetime = 3f;

    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private float timeAlive;
    private Action<BolinhaDePapel> releaseToPool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        timeAlive = 0f;
    }

    public void Initialize(Action<BolinhaDePapel> releaseCallback)
    {
        releaseToPool = releaseCallback;
    }

    public void SetDirection(float directionX)
    {
        moveDirection = new Vector2(directionX, 0f).normalized;
        transform.localScale = directionX < 0 ? new Vector3(-1f, 1f, 1f) : new Vector3(1f, 1f, 1f);
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= lifetime)
        {
            Return();
        }
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = moveDirection * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignora o próprio Caderno e outras bolinhas
        if (collision.GetComponentInParent<Caderno>() != null || collision.GetComponent<BolinhaDePapel>() != null)
            return;

        // Detecta o Player (no objeto raiz ou em colisores filhos)
        Player player = collision.GetComponent<Player>();
        if (player == null) player = collision.GetComponentInParent<Player>();

        if (player != null)
        {
            player.TomarDano(damage);
            Return();
            return;
        }

        // Destrói ao bater no chão/paredes (Layer 6)
        if (collision.gameObject.layer == 6)
        {
            Return();
        }
    }

    private void Return()
    {
        if (releaseToPool != null)
        {
            releaseToPool(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}