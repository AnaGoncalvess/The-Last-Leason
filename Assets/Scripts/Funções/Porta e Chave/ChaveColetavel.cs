using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ChaveColetavel : MonoBehaviour
{
    private PortaController porta;

    // Recebe a referência da porta vinda do ColetaveisManager
    public void ConfigurarChave(PortaController portaAlvo)
    {
        porta = portaAlvo;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Usa a mesma lógica do seu ItemColetavel: detecta o Player imediatamente
        if (collision.CompareTag("Player") || collision.GetComponentInParent<Player>() != null)
        {
            ColetarChave();
        }
    }

    private void ColetarChave()
    {
        // 1. Abre a porta se a referência existir
        if (porta != null)
        {
            porta.AbrirPorta();
        }

        // 2. Some com a chave da cena (igual aos cristais e lâmpadas)
        Destroy(gameObject);
    }
}
