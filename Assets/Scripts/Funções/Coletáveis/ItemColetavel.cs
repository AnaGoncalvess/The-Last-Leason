using UnityEngine;

public class ItemColetavel : MonoBehaviour
{
    public enum TipoItem { Cristal, Moeda }

    [SerializeField] private AudioClip somColetavel;
    [Header("Tipo do Coletável")]
    [SerializeField] private TipoItem tipo = TipoItem.Moeda;

    private void OnTriggerEnter2D(Collider2D collision)
    {


    Debug.Log($"Colidiu com: {collision.gameObject.name} | Tag: {collision.tag}");

    if (collision.CompareTag("Player"))
    {
        if (somColetavel == null)
        {
            Debug.LogWarning("O somColetavel está VAZIO no Inspector!");
        }

        if (AudioManager.Instance != null && somColetavel != null)
        {
            AudioManager.Instance.PlaySfxWithDucking(somColetavel, 0.2f, 0.5f);
        }

        Destroy(gameObject);
    }


        bool ePlayer = collision.CompareTag("Player") ||
                      collision.gameObject.name.ToLower().Contains("player") ||
                      (collision.attachedRigidbody != null && collision.attachedRigidbody.CompareTag("Player"));

        if (ePlayer)
        {
            Coletar();
        }
        
        if (collision.CompareTag("Player"))
        {
            // Toca o som baixando temporariamente a música de fundo
            if (AudioManager.Instance != null && somColetavel != null)
            {
                AudioManager.Instance.PlaySfxWithDucking(somColetavel, 0.2f, 0.5f);
            }

            Destroy(gameObject);
        }


    }

    private void Coletar()
    {
        ColetaveisManager manager = ColetaveisManager.Instance != null ? ColetaveisManager.Instance : FindFirstObjectByType<ColetaveisManager>();

        if (manager != null)
        {

            if (tipo == TipoItem.Cristal)
            {
                manager.AddCristal();
            }
            else
            {
                manager.AddMoeda();
            }
            Destroy(gameObject);
        }
    }
}