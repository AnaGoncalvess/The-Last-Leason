using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Singleton de fade de tela: escurece antes de trocar de cena e clareia de volta depois,
/// sem precisar de nenhuma UI montada na cena (constrói o próprio Canvas em runtime).
/// Use ScreenFader.Instance.FadeOutAndLoad("NomeDaCena") em vez de SceneManager.LoadScene
/// direto para transições mais suaves entre fases/menus.
/// </summary>
public class ScreenFader : MonoBehaviour
{
    private static ScreenFader instance;

    public static ScreenFader Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("ScreenFader (Auto)");
                instance = go.AddComponent<ScreenFader>();
            }

            return instance;
        }
    }

    private CanvasGroup canvasGroup;
    private bool isFading;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        BuildUI();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void BuildUI()
    {
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;

        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        GameObject imgGO = new GameObject("FadeImage", typeof(RectTransform));
        imgGO.transform.SetParent(transform, false);

        RectTransform rect = imgGO.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image img = imgGO.AddComponent<Image>();
        img.color = Color.black;
        img.raycastTarget = false;
    }

    public void FadeOutAndLoad(string sceneName, float duration = 0.5f)
    {
        if (isFading) return;
        StartCoroutine(FadeOutRoutine(sceneName, duration));
    }

    private IEnumerator FadeOutRoutine(string sceneName, float duration)
    {
        isFading = true;
        canvasGroup.blocksRaycasts = true;

        yield return Fade(canvasGroup.alpha, 1f, duration);

        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeInRoutine(0.5f));
    }

    private IEnumerator FadeInRoutine(float duration)
    {
        yield return Fade(canvasGroup.alpha, 0f, duration);
        canvasGroup.blocksRaycasts = false;
        isFading = false;
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;
        canvasGroup.alpha = from;

        while (elapsed < duration)
        {
            // Tempo não-escalado: o fade funciona mesmo se a troca de cena acontecer com o jogo pausado.
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = to;
    }
}
