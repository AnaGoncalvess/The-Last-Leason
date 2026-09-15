using UnityEngine;

public class BolinhaDePapel : MonoBehaviour
{
    [Header("Configurações do Projétil")]
    [SerializeField] private float speed = 7f;
    [SerializeField] private float damage = 0.4f;
    [SerializeField] private float lifetime = 3f; // Destrói após alguns segundos se não acertar nada

    private Vector2 moveDirection;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Garante que a bolinha seja destruída após o tempo limite para não poluir a cena
        Destroy(gameObject, lifetime);
    }

    // Método chamado pelo Caderno no momento do disparo para definir a direção (1 para direita, -1 para esquerda)
    public void SetDirection(float directionX)
    {
        // Define a direção horizontal do movimento
        moveDirection = new Vector2(directionX, 0).normalized;

        // Caso a sprite da bolinha tenha lado, podemos virá-la também
        if (directionX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void FixedUpdate()
    {
        // Move o projétil em linha reta
        if (rb != null)
        {
            rb.linearVelocity = moveDirection * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se colidiu com o Player
        if (collision.CompareTag("Player"))
        {
            // Tenta chamar o método OnHit ou dar dano no Player
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.OnHit();
            }

            // Destrói a bolinha ao acertar o jogador
            Destroy(gameObject);
        }
        // Destrói a bolinha se bater no chão/obstáculos (Layer do cenário, ex: Layer 6 do seu projeto)
        else if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
    }
}