// using UnityEngine;

// public class ItemColetavel : MonoBehaviour
// {
//     public enum TipoItem { Cristal, Moeda }

//     [SerializeField] private AudioClip somColetavel;
//     [Header("Tipo do Coletável")]
//     [SerializeField] private TipoItem tipo = TipoItem.Moeda;

//     private void OnTriggerEnter2D(Collider2D collision)
//     {


//     Debug.Log($"Colidiu com: {collision.gameObject.name} | Tag: {collision.tag}");

//     if (collision.CompareTag("Player"))
//     {
//         if (somColetavel == null)
//         {
//             Debug.LogWarning("O somColetavel está VAZIO no Inspector!");
//         }

//         if (AudioManager.Instance != null && somColetavel != null)
//         {
//             AudioManager.Instance.PlaySfxWithDucking(somColetavel, 0.2f, 0.5f);
//         }

//         Destroy(gameObject);
//     }


//         bool ePlayer = collision.CompareTag("Player") ||
//                       collision.gameObject.name.ToLower().Contains("player") ||
//                       (collision.attachedRigidbody != null && collision.attachedRigidbody.CompareTag("Player"));

//         if (ePlayer)
//         {
//             Coletar();
//         }
        
//         if (collision.CompareTag("Player"))
//         {
//             // Toca o som baixando temporariamente a música de fundo
//             if (AudioManager.Instance != null && somColetavel != null)
//             {
//                 AudioManager.Instance.PlaySfxWithDucking(somColetavel, 0.2f, 0.5f);
//             }

//             Destroy(gameObject);
//         }


//     }

//     private void Coletar()
//     {
//         ColetaveisManager manager = ColetaveisManager.Instance != null ? ColetaveisManager.Instance : FindFirstObjectByType<ColetaveisManager>();

//         if (manager != null)
//         {

//             if (tipo == TipoItem.Cristal)
//             {
//                 manager.AddCristal();
//             }
//             else
//             {
//                 manager.AddMoeda();
//             }
//             Destroy(gameObject);
//         }
//     }
// }

// using UnityEngine;

// public class ItemColetavel : MonoBehaviour
// {
//     public enum TipoItem { Cristal, Moeda }

//     [SerializeField] private AudioClip somColetavel;
//     [Header("Tipo do Coletável")]
//     [SerializeField] private TipoItem tipo = TipoItem.Moeda;

//     private bool jaColetado = false;

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (jaColetado) return;

//         bool ePlayer = collision.CompareTag("Player") || 
//                       collision.gameObject.name.ToLower().Contains("player") || 
//                       (collision.attachedRigidbody != null && collision.attachedRigidbody.CompareTag("Player"));

//         if (ePlayer)
//         {
//             jaColetado = true;

//             // 1. Toca o som com Ducking
//             if (AudioManager.Instance != null && somColetavel != null)
//             {
//                 AudioManager.Instance.PlaySfxWithDucking(somColetavel, 0.2f, 0.5f);
//             }

//             // 2. Incrementa o contador no Manager
//             Coletar();
//         }
//     }

//     private void Coletar()
//     {
//         ColetaveisManager manager = ColetaveisManager.Instance != null ? ColetaveisManager.Instance : FindFirstObjectByType<ColetaveisManager>();

//         if (manager != null)
//         {
//             if (tipo == TipoItem.Cristal)
//             {
//                 manager.AddCristal();
//             }
//             else
//             {
//                 manager.AddMoeda();
//             }
//         }

//         Destroy(gameObject);
//     }
// }

using UnityEngine;

public class ItemColetavel : MonoBehaviour
{
    public enum TipoItem { Cristal, Moeda }

    [SerializeField] private AudioClip somColetavel;
    [Header("Tipo do Coletável")]
    [SerializeField] private TipoItem tipo = TipoItem.Moeda;

    private bool jaColetado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (jaColetado) return;

        bool ePlayer = collision.CompareTag("Player") || 
                      collision.gameObject.name.ToLower().Contains("player") || 
                      (collision.attachedRigidbody != null && collision.attachedRigidbody.CompareTag("Player"));

        if (ePlayer)
        {
            jaColetado = true;

            // 1. Toca o som com efeito Ducking na música
            if (AudioManager.Instance != null && somColetavel != null)
            {
                AudioManager.Instance.PlaySfxWithDucking(somColetavel, 0.2f, 0.5f);
            }

            // 2. Incrementa o contador no Manager e se destrói
            Coletar();
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
        }

        Destroy(gameObject);
    }
}