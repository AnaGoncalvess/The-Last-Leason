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

//         GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
//         if (playerObj != null)
//         {
//             direcaoFixa = ((Vector2)playerObj.transform.position - (Vector2)transform.position).normalized;
//         }
//         else
//         {
//             direcaoFixa = Vector2.left;
//         }

//         // Mantém a rotação zerada (0,0,0) para o sprite não virar de cabeça para baixo nem deslocar
//         transform.rotation = Quaternion.identity;

//         // Só espelha horizontalmente se o Player estiver à direita do Boss
//         SpriteRenderer sr = GetComponent<SpriteRenderer>();
//         if (sr != null)
//         {
//             sr.flipX = direcaoFixa.x > 0f;
//         }

//         Destroy(gameObject, tempoDeVida);
//     }

//     private void FixedUpdate()
//     {
//         if (rb == null) return;

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

// using UnityEngine;

// public class ProjetilBoss : MonoBehaviour
// {
//     [Header("Configurações do Projétil Teleguiado")]
//     [SerializeField] private float velocidade = 7f;
//     [SerializeField] private float suavidadePerseguicao = 2.5f; // Quanto maior, mais ele persegue o Player
//     [SerializeField] private float tempoDeVida = 4f;
//     [SerializeField] private float dano = 1f;

//     private Rigidbody2D rb;
//     [SerializeField]private Transform alvoPlayer;
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
//             direcaoAtual = ((Vector2)alvoPlayer.position - (Vector2)transform.position).normalized;
//         }
//         else
//         {
//             direcaoAtual = Vector2.left;
//         }

//         transform.rotation = Quaternion.identity;
//         Destroy(gameObject, tempoDeVida);
//     }

//     private void FixedUpdate()
//     {
//         if (rb == null) return;

//         if (alvoPlayer != null)
//         {
//             Vector2 direcaoDesejada = ((Vector2)alvoPlayer.position - rb.position).normalized;
//             // Ajusta a direção suavemente em direção ao Player sem girar o sprite em 180°
//             direcaoAtual = Vector2.Lerp(direcaoAtual, direcaoDesejada, suavidadePerseguicao * Time.fixedDeltaTime).normalized;
//         }

//         SpriteRenderer sr = GetComponent<SpriteRenderer>();
//         if (sr != null)
//         {
//             sr.flipX = direcaoAtual.x > 0f;
//         }

//         rb.linearVelocity = direcaoAtual * velocidade;
//     }

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (collision.GetComponentInParent<BossController>() != null) return;

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

public class ProjetilBoss : MonoBehaviour
{
    [Header("Configurações do Projétil Teleguiado")]
    [SerializeField] private float velocidade = 7f;
    [SerializeField] private float suavidadePerseguicao = 2.5f;
    [SerializeField] private float tempoDeVida = 4f;
    [SerializeField] private float dano = 1f;
    [SerializeField] private float raioAcerto = 0.6f; // Área de detecção garantida

    private Rigidbody2D rb;
    private Transform alvoPlayer;
    private Vector2 direcaoAtual;
    private float tempoNascimento;
    private bool jaAcertou = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tempoNascimento = Time.time;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            alvoPlayer = playerObj.transform;
            direcaoAtual = ((Vector2)alvoPlayer.position - (Vector2)transform.position).normalized;
        }
        else
        {
            direcaoAtual = Vector2.left;
        }

        transform.rotation = Quaternion.identity;
        Destroy(gameObject, tempoDeVida);
    }

    private void FixedUpdate()
    {
        if (jaAcertou) return;

        if (alvoPlayer != null)
        {
            Vector2 direcaoDesejada = ((Vector2)alvoPlayer.position - rb.position).normalized;
            direcaoAtual = Vector2.Lerp(direcaoAtual, direcaoDesejada, suavidadePerseguicao * Time.fixedDeltaTime).normalized;
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.flipX = direcaoAtual.x > 0f;
        }

        if (rb != null)
        {
            rb.linearVelocity = direcaoAtual * velocidade;
        }

        // Verificação direta de proximidade/colisão (funciona mesmo se Physics2D ignorar a Layer)
        Collider2D[] proximos = Physics2D.OverlapCircleAll(transform.position, raioAcerto);
        foreach (Collider2D col in proximos)
        {
            if (col.CompareTag("Player") || col.GetComponentInParent<Player>() != null)
            {
                AplicarDanoEDestruir(col);
                return;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (jaAcertou || collision.GetComponentInParent<BossController>() != null) return;

        if (collision.CompareTag("Player") || collision.GetComponentInParent<Player>() != null)
        {
            AplicarDanoEDestruir(collision);
        }
        else if (Time.time - tempoNascimento > 0.15f && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }

    private void AplicarDanoEDestruir(Collider2D collision)
    {
        jaAcertou = true;
        Player scriptPlayer = collision.GetComponent<Player>();
        if (scriptPlayer == null) scriptPlayer = collision.GetComponentInParent<Player>();

        if (scriptPlayer != null)
        {
            scriptPlayer.TomarDano(dano);
        }

        Destroy(gameObject);
    }
}