// using UnityEngine;

// public class ItemColetavel : MonoBehaviour
// {
//     public enum TipoItem { Cristal, Moeda }

//     [SerializeField] private AudioClip somColetavel;
//     [Header("Tipo do Coletável")]
//     [SerializeField] private TipoItem tipo = TipoItem.Moeda;

//     [Header("Efeito de Flutuação (Mexer)")]
//     [SerializeField] private bool flutuar = true;
//     [SerializeField] private float velocidadeFlutuacao = 3f; // Velocidade do balanço
//     [SerializeField] private float alturaFlutuacao = 0.15f;  // Amplitude do movimento

//     private Vector3 posicaoInicial;
//     private bool jaColetado = false;

//     private void Start()
//     {
//         // Salva a posição original onde o item foi colocado na cena
//         posicaoInicial = transform.position;
//     }

//     private void Update()
//     {
//         // Faz o item subir e descer suavemente em loop
//         if (flutuar && !jaColetado)
//         {
//             float novoY = posicaoInicial.y + Mathf.Sin(Time.time * velocidadeFlutuacao) * alturaFlutuacao;
//             transform.position = new Vector3(posicaoInicial.x, novoY, posicaoInicial.z);
//         }
//     }

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (jaColetado) return;

//         bool ePlayer = collision.CompareTag("Player") || 
//                       collision.gameObject.name.ToLower().Contains("player") || 
//                       (collision.attachedRigidbody != null && collision.attachedRigidbody.CompareTag("Player"));

//         if (ePlayer)
//         {
//             jaColetado = true;

//             // 1. Toca o som com Ducking na música
//             if (AudioManager.Instance != null && somColetavel != null)
//             {
//                 AudioManager.Instance.PlaySfxWithDucking(somColetavel, 0.2f, 0.5f);
//             }

//             // 2. Incrementa a contagem no ColetaveisManager
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
    public enum TipoItem { Cristal, Moeda, Chave }

    [SerializeField] private AudioClip somColetavel;
    [Header("Tipo do Coletável")]
    [SerializeField] private TipoItem tipo = TipoItem.Moeda;

    [Header("Efeito de Flutuação (Mexer)")]
    [SerializeField] private bool flutuar = true;
    [SerializeField] private float velocidadeFlutuacao = 3f; // Velocidade do balanço
    [SerializeField] private float alturaFlutuacao = 0.15f;  // Amplitude do movimento

    private Vector3 posicaoInicial;
    private bool jaColetado = false;

    private void Start()
    {
        // Salva a posição original onde o item foi colocado na cena
        posicaoInicial = transform.position;
    }

    private void Update()
    {
        // Faz o item subir e descer suavemente em loop
        if (flutuar && !jaColetado)
        {
            float novoY = posicaoInicial.y + Mathf.Sin(Time.time * velocidadeFlutuacao) * alturaFlutuacao;
            transform.position = new Vector3(posicaoInicial.x, novoY, posicaoInicial.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (jaColetado) return;

        bool ePlayer = collision.CompareTag("Player") || 
                      collision.gameObject.name.ToLower().Contains("player") || 
                      (collision.attachedRigidbody != null && collision.attachedRigidbody.CompareTag("Player"));

        if (ePlayer)
        {
            jaColetado = true;

            // 1. Toca o som com Ducking na música
            if (AudioManager.Instance != null && somColetavel != null)
            {
                AudioManager.Instance.PlaySfxWithDucking(somColetavel, 0.2f, 0.5f);
            }

            // 2. Incrementa a contagem ou ativa a chave
            Coletar();
        }
    }

    private void Coletar()
    {
        if (tipo == TipoItem.Chave)
        {
            // Busca o componente LevelExitCheck anexado ao PontoFechadura
            LevelExitCheck fechadura = FindFirstObjectByType<LevelExitCheck>();
            if (fechadura != null)
            {
                fechadura.ColetarChave();
            }
            else
            {
                Debug.LogWarning("Chave coletada, mas nenhum LevelExitCheck (PontoFechadura) foi encontrado na cena!");
            }
        }
        else
        {
            ColetaveisManager manager = ColetaveisManager.Instance != null ? ColetaveisManager.Instance : FindFirstObjectByType<ColetaveisManager>();

            if (manager != null)
            {
                if (tipo == TipoItem.Cristal)
                {
                    manager.AddCristal();
                }
                else if (tipo == TipoItem.Moeda)
                {
                    manager.AddMoeda();
                }
            }
        }

        Destroy(gameObject);
    }
}