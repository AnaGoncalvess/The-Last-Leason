// // using UnityEngine;

// // public class ProjetilLaserPaulo : MonoBehaviour
// // {
// //     [Header("Configurações do Laser Teleguiado")]
// //     [SerializeField] private float velocidade = 7.5f;
// //     [SerializeField] private float suavidadeCurva = 3.0f;
// //     [SerializeField] private float tempoDeVida = 4f;
// //     [SerializeField] private float dano = 1f;

// //     private Rigidbody2D rb;
// //     private Transform alvoPlayer;
// //     private Vector2 direcaoAtual;
// //     private float tempoNascimento;

// //     private void Start()
// //     {
// //         rb = GetComponent<Rigidbody2D>();
// //         tempoNascimento = Time.time;

// //         GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
// //         if (playerObj != null)
// //         {
// //             alvoPlayer = playerObj.transform;
// //             direcaoAtual = ((Vector2)alvoPlayer.position - (Vector2)transform.position).normalized;
// //         }
// //         else
// //         {
// //             direcaoAtual = Vector2.left;
// //         }

// //         AtualizarRotacao();
// //         Destroy(gameObject, tempoDeVida);
// //     }

// //     private void FixedUpdate()
// //     {
// //         if (rb == null) return;

// //         if (alvoPlayer != null)
// //         {
// //             Vector2 direcaoDesejada = ((Vector2)alvoPlayer.position - rb.position).normalized;
// //             direcaoAtual = Vector2.Lerp(direcaoAtual, direcaoDesejada, suavidadeCurva * Time.fixedDeltaTime).normalized;
// //         }

// //         AtualizarRotacao();
// //         rb.linearVelocity = direcaoAtual * velocidade;
// //     }

// //     private void AtualizarRotacao()
// //     {
// //         float angulo = Mathf.Atan2(direcaoAtual.y, direcaoAtual.x) * Mathf.Rad2Deg;
// //         transform.rotation = Quaternion.Euler(0f, 0f, angulo);
// //     }

// //     private void OnTriggerEnter2D(Collider2D collision)
// //     {
// //         // Ignora o próprio Paulo
// //         if (collision.GetComponentInParent<PauloBossController>() != null) return;

// //         if (collision.CompareTag("Player") || collision.GetComponentInParent<Player>() != null)
// //         {
// //             Player scriptPlayer = collision.GetComponent<Player>();
// //             if (scriptPlayer == null) scriptPlayer = collision.GetComponentInParent<Player>();

// //             if (scriptPlayer != null)
// //             {
// //                 scriptPlayer.TomarDano(dano);
// //             }

// //             Destroy(gameObject);
// //         }
// //         else if (Time.time - tempoNascimento > 0.15f && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
// //         {
// //             Destroy(gameObject);
// //         }
// //     }
// // }

// using UnityEngine;

// public class ProjetilLaserPaulo : MonoBehaviour
// {
//     [Header("Configurações do Laser Teleguiado")]
//     [SerializeField] private float velocidade = 7.5f;
//     [SerializeField] private float suavidadeCurva = 3.0f;
//     [SerializeField] private float tempoDeVida = 4f;
//     [SerializeField] private float dano = 1f;

//     [Header("Ajuste Visual e Mira")]
//     [SerializeField] private float compensacaoAnguloSprite = -40f; // Ajuste no Inspector para alinhar o traço inclinado
//     [SerializeField] private Vector2 offsetMiraPlayer = new Vector2(0f, 0.8f); // Mira no tronco do Player em vez dos pés

//     private Rigidbody2D rb;
//     private Transform alvoPlayer;
//     private Vector2 direcaoAtual;
//     private float tempoNascimento;

//     private void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         tempoNascimento = Time.time;

//         GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
//         if (playerObj != null)
//         {
//             alvoPlayer = playerObj.transform;
//             Vector2 pontoAlvo = (Vector2)alvoPlayer.position + offsetMiraPlayer;
//             direcaoAtual = (pontoAlvo - (Vector2)transform.position).normalized;
//         }
//         else
//         {
//             direcaoAtual = Vector2.left;
//         }

//         AtualizarRotacao();
//         Destroy(gameObject, tempoDeVida);
//     }

//     private void FixedUpdate()
//     {
//         if (rb == null) return;

//         if (alvoPlayer != null)
//         {
//             Vector2 pontoAlvo = (Vector2)alvoPlayer.position + offsetMiraPlayer;
//             Vector2 direcaoDesejada = (pontoAlvo - rb.position).normalized;
//             direcaoAtual = Vector2.Lerp(direcaoAtual, direcaoDesejada, suavidadeCurva * Time.fixedDeltaTime).normalized;
//         }

//         AtualizarRotacao();
//         rb.linearVelocity = direcaoAtual * velocidade;
//     }

//     private void AtualizarRotacao()
//     {
//         float angulo = Mathf.Atan2(direcaoAtual.y, direcaoAtual.x) * Mathf.Rad2Deg;
//         transform.rotation = Quaternion.Euler(0f, 0f, angulo + compensacaoAnguloSprite);
//     }

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         // Ignora o próprio Paulo
//         if (collision.GetComponentInParent<PauloBossController>() != null) return;

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
//         else if (Time.time - tempoNascimento > 0.15f && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
//         {
//             Destroy(gameObject);
//         }
//     }
// }

using UnityEngine;

public class ProjetilLaserPaulo : MonoBehaviour
{
    [Header("Configurações do Laser em Linha Reta")]
    [SerializeField] private float velocidade = 9f;
    [SerializeField] private float tempoDeVida = 4f;
    [SerializeField] private float dano = 1f;

    [Header("Ajuste Visual e Mira")]
    [SerializeField] private float compensacaoAnguloSprite = -40f; // Ajuste no Inspector para alinhar o traço inclinado
    [SerializeField] private Vector2 offsetMiraPlayer = new Vector2(0f, 0.8f); // Mira na altura do tronco do Player

    private Rigidbody2D rb;
    private Vector2 direcaoFixa;
    private float tempoNascimento;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tempoNascimento = Time.time;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            // Calcula a direção APENAS UMA VEZ no momento do disparo (não persegue depois)
            Vector2 pontoAlvo = (Vector2)playerObj.transform.position + offsetMiraPlayer;
            direcaoFixa = (pontoAlvo - (Vector2)transform.position).normalized;
        }
        else
        {
            direcaoFixa = Vector2.left;
        }

        AtualizarRotacao();
        Destroy(gameObject, tempoDeVida);
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        // Mantém a velocidade constante em linha reta na direção inicial
        rb.linearVelocity = direcaoFixa * velocidade;
    }

    private void AtualizarRotacao()
    {
        float angulo = Mathf.Atan2(direcaoFixa.y, direcaoFixa.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angulo + compensacaoAnguloSprite);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignora o próprio Paulo
        if (collision.GetComponentInParent<PauloBossController>() != null) return;

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
        else if (Time.time - tempoNascimento > 0.15f && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}