using UnityEngine;

public class ItemColetavel : MonoBehaviour
{
    public enum TipoItem { Cristal, Moeda }

    [Header("Tipo do Item")]
    [SerializeField] private TipoItem tipo = TipoItem.Cristal;

    [Header("Animação de Flutuação")]
    [SerializeField] private float amplitude = 0.15f; // Altura do movimento
    [SerializeField] private float velocidade = 2.5f; // Velocidade da flutuação

    private Vector3 posInicial;

    private void Start()
    {
        // Salva a posição inicial onde o item spawnou
        posInicial = transform.position;
    }

    private void Update()
    {
        // Calcula a nova posição no eixo Y usando onda Senoidal
        float novoY = posInicial.y + Mathf.Sin(Time.time * velocidade) * amplitude;
        transform.position = new Vector3(posInicial.x, novoY, posInicial.z);
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     // Detecta se foi o Player que encostou
    //     if (collision.CompareTag("Player") || collision.GetComponentInParent<Player>() != null)
    //     {
    //         if (CristalManager.instance != null)
    //         {
    //             if (tipo == TipoItem.Cristal)
    //             {
    //                 CristalManager.instance.AddCristal();
    //             }
    //             else if (tipo == TipoItem.Moeda)
    //             {
    //                 // CristalManager.instance.AddMoeda(); (Ativar quando criar a moeda)
    //             }
    //         }

    //         Destroy(gameObject);
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player") || collision.GetComponentInParent<Player>() != null)
    {
        if (ColetaveisManager.instance != null)
        {
            if (tipo == TipoItem.Cristal)
            {
                ColetaveisManager.instance.AddCristal();
            }
            else if (tipo == TipoItem.Moeda)
            {
                ColetaveisManager.instance.AddMoeda();
            }
        }

        Destroy(gameObject);
    }
}
}