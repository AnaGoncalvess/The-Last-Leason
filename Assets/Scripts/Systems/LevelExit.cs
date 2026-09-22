using UnityEngine;

/// <summary>
/// Zona de trigger que troca de fase: quando o Player entra, carrega a próxima cena (com fade).
/// Coloque num Collider2D (Is Trigger) na borda final da fase.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class LevelExit : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Level2";
    [SerializeField] private float fadeDuration = 0.5f;

    private bool triggered;

    private void Reset()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered || !collision.CompareTag("Player")) return;

        triggered = true;
        ScreenFader.Instance.FadeOutAndLoad(nextSceneName, fadeDuration);
    }
}
