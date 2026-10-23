using UnityEngine;

public class ProjetilVeneno : MonoBehaviour
{
    [SerializeField] private float velocidade = 8f;
    [SerializeField] private float dano = 10f;
    [SerializeField] private float tempoVida = 4f;

    private void Start()
    {
        Destroy(gameObject, tempoVida);
    }

    private void Update()
    {
        transform.Translate(Vector3.left * velocidade * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Substitua 'PlayerHealth' pelo nome do seu script de vida do jogador
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TomarDano(dano);
            }

            Destroy(gameObject);
        }
    }
}