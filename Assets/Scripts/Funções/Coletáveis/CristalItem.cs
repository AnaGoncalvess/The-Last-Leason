
using UnityEngine;

public class CristalItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // // Exibe mensagem no Console assim que QUALQUER objeto encosta no cristal
        // Debug.Log("Objeto entrou no Trigger do Cristal: " + collision.gameObject.name);

        if (collision.CompareTag("Player"))
        {
            if (ColetaveisManager.instance != null)
            {
                ColetaveisManager.instance.AddCristal();
            }
            Destroy(gameObject);
        }
    }
}
