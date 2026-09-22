using UnityEngine;

/// <summary>
/// Item coletável genérico: some ao tocar o Player e soma pontos no ScoreManager.
/// Para usar: crie um GameObject com SpriteRenderer + Collider2D (Is Trigger) e este script.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour
{
    [SerializeField] private int scoreValue = 1;
    [SerializeField] private AudioClip pickupSfx;

    private void Reset()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        ScoreManager.Instance.Add(scoreValue);

        if (pickupSfx != null)
        {
            AudioManager.Instance.PlaySfx(pickupSfx);
        }

        Destroy(gameObject);
    }
}
