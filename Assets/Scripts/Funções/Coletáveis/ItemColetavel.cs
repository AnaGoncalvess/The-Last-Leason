using UnityEngine;

public class ItemColetavel : MonoBehaviour
{
    public enum TipoItem { Cristal, Moeda }

    [Header("Tipo do Coletável")]
    [SerializeField] private TipoItem tipo = TipoItem.Moeda;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Teste de Colisão
        Debug.Log($"[DIAGNÓSTICO] Objeto '{gameObject.name}' foi tocado por: '{collision.gameObject.name}' (Tag: '{collision.tag}')");

        bool ePlayer = collision.CompareTag("Player") || 
                      collision.gameObject.name.ToLower().Contains("player") ||
                      (collision.attachedRigidbody != null && collision.attachedRigidbody.CompareTag("Player"));

        if (ePlayer)
        {
            Debug.Log($"[DIAGNÓSTICO] Player reconhecido com sucesso no item '{gameObject.name}'!");
            Coletar();
        }
        else
        {
            Debug.LogWarning($"[DIAGNÓSTICO] O objeto '{collision.gameObject.name}' tocou no item, mas NÃO foi reconhecido como Player.");
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