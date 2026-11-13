// using UnityEngine;

// public class ProjetilBoss : MonoBehaviour
// {
//     [Header("Configurações do Projétil Teleguiado")]
//     [SerializeField] private float velocidade = 6f;
//     [SerializeField] private float forcaCurva = 220f; // Quanto maior, mais rápido faz a curva atrás do Player
//     [SerializeField] private float tempoDeVida = 4f;  // Destrói o projétil se o Player desviar por muito tempo
//     [SerializeField] private float dano = 1f;

//     private Transform alvoPlayer;
//     private Rigidbody2D rb;

//     private void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();

//         // Localiza o Player automaticamente na cena
//         GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
//         if (playerObj != null)
//         {
//             alvoPlayer = playerObj.transform;
//         }

//         // Destrói o projétil após o tempo limite para não ficar infinito na cena
//         Destroy(gameObject, tempoDeVida);
//     }

//     private void FixedUpdate()
//     {
//         if (rb == null) return;

//         if (alvoPlayer != null)
//         {
//             // Calcula a direção da ponta do projétil até o centro do Player
//             Vector2 direcaoParaAlvo = ((Vector2)alvoPlayer.position - rb.position).normalized;

//             // Calcula o ângulo de rotação suave (teleguiado)
//             float valorRotacao = Vector3.Cross(direcaoParaAlvo, transform.right).z;
//             rb.angularVelocity = -valorRotacao * forcaCurva;
//         }

//         // Move sempre para frente na direção em que está apontando
//         rb.linearVelocity = transform.right * velocidade;
//     }

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         // Se atingir o Player, aplica dano e destrói o projétil
//         if (collision.CompareTag("Player") || collision.GetComponentInParent<Player>() != null)
//         {
//             Player scriptPlayer = collision.GetComponent<Player>();
//             if (scriptPlayer == null) scriptPlayer = collision.GetComponentInParent<Player>();

//             if (scriptPlayer != null)
//             {
//                 scriptPlayer.TomarDano(dano);
//             }

//             Destroy(gameObject);
//         }
//         // Se bater no chão ou parede (Layer Ground), também some
//         else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
//         {
//             Destroy(gameObject);
//         }
//     }
// }

// using UnityEngine;

// public class ProjetilBoss : MonoBehaviour
// {
//     [Header("Configurações do Projétil Direcionado")]
//     [SerializeField] private float velocidade = 9f;
//     [SerializeField] private float tempoDeVida = 4f;
//     [SerializeField] private float dano = 1f;

//     private Rigidbody2D rb;
//     private Vector2 direcaoFixa;

//     private void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();

//         // Localiza o Player no momento exato em que o Boss cospe
//         GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
//         if (playerObj != null)
//         {
//             // Calcula a linha reta da boca do Boss até a posição atual do Player
//             direcaoFixa = ((Vector2)playerObj.transform.position - (Vector2)transform.position).normalized;
//         }
//         else
//         {
//             direcaoFixa = Vector2.left;
//         }

//         // Gira o sprite do projétil para apontar exatamente nessa direção
//         float angulo = Mathf.Atan2(direcaoFixa.y, direcaoFixa.x) * Mathf.Rad2Deg;
//         transform.rotation = Quaternion.Euler(0f, 0f, angulo);

//         Destroy(gameObject, tempoDeVida);
//     }

//     private void FixedUpdate()
//     {
//         if (rb == null) return;

//         // Mantém velocidade constante em linha reta (não faz curva se o Player pular/desviar)
//         rb.linearVelocity = direcaoFixa * velocidade;
//     }

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (collision.CompareTag("Player") || collision.GetComponentInParent<Player>() != null)
//         {
//             Player scriptPlayer = collision.GetComponent<Player>();
//             if (scriptPlayer == null) scriptPlayer = collision.GetComponentInParent<Player>();

//             if (scriptPlayer != null)
//             {
//                 scriptPlayer.TomarDano(dano);
//             }

//             Destroy(gameObject);
//         }
//         else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
//         {
//             Destroy(gameObject);
//         }
//     }
// }

using UnityEngine;

public class ProjetilBoss : MonoBehaviour
{
    [Header("Configurações do Projétil Direcionado")]
    [SerializeField] private float velocidade = 9f;
    [SerializeField] private float tempoDeVida = 4f;
    [SerializeField] private float dano = 1f;

    private Rigidbody2D rb;
    private Vector2 direcaoFixa;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            direcaoFixa = ((Vector2)playerObj.transform.position - (Vector2)transform.position).normalized;
        }
        else
        {
            direcaoFixa = Vector2.left;
        }

        // Mantém a rotação zerada (0,0,0) para o sprite não virar de cabeça para baixo nem deslocar
        transform.rotation = Quaternion.identity;

        // Só espelha horizontalmente se o Player estiver à direita do Boss
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.flipX = direcaoFixa.x > 0f;
        }

        Destroy(gameObject, tempoDeVida);
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        rb.linearVelocity = direcaoFixa * velocidade;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.GetComponentInParent<Player>() != null)
        {
            Player scriptPlayer = collision.GetComponent<Player>();
            if (scriptPlayer == null) scriptPlayer = collision.GetComponentInParent<Player>();

            if (scriptPlayer != null)
            {
                scriptPlayer.TomarDano(dano);
            }

            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}