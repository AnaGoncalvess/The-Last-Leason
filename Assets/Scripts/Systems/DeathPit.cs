using UnityEngine;

/// <summary>
/// Zona de trigger para poços sem fundo: mata o Player na hora ao cair nela, em vez de deixá-lo
/// caindo pra sempre. Coloque num Collider2D (Is Trigger) posicionado abaixo dos buracos da fase.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class DeathPit : MonoBehaviour
{
    private void Reset()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            // player.KillInstantly();
        }
    }
}
