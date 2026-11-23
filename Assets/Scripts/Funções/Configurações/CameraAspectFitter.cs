using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Deixa o jogo responsivo em qualquer dispositivo mantendo a proporção 16:9 (1280x720).
///
/// - Roda sozinho em TODAS as cenas (não precisa estar em nenhum objeto).
///   Instâncias antigas deste script que estão nas cenas se desativam sozinhas.
/// - Câmera: viewport 16:9 centralizado (barras pretas em cima/embaixo ou nas laterais).
///   Uma câmera de fundo preto limpa as barras, evitando "lixo" de imagem em celulares.
/// - Canvas: Scale With Screen Size, 1280x720, modo Expand. A UI usa a mesma escala
///   da área do jogo e nunca é cortada, qualquer que seja a proporção da tela.
/// </summary>
public class CameraAspectFitter : MonoBehaviour
{
    private const float RefWidth = 1280f;
    private const float RefHeight = 720f;
    private const float TargetAspect = RefWidth / RefHeight; // 16:9

    // Garante o mesmo Canvas Scaler em todas as cenas (inclusive futuras).
    private const bool ForcarCanvasScaler = true;

    private static CameraAspectFitter instance;

    private int lastScreenWidth;
    private int lastScreenHeight;
    private Camera lastMainCam;
    private Camera backgroundCam;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        if (instance != null) return;

        GameObject go = new GameObject("ResponsiveScreen");
        DontDestroyOnLoad(go);
        go.AddComponent<CameraAspectFitter>();
    }

    private void Awake()
    {
        // Cópia colocada manualmente numa cena: o gerenciador global já cuida de tudo.
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        CriarCameraDeFundo();
    }

    private void OnEnable()
    {
        if (instance != this) return;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        if (instance != this) return;
        AplicarAjusteCompleto();
    }

    private void LateUpdate()
    {
        if (instance != this) return;

        // Reajusta se a janela/tela mudou (redimensionar, girar o celular) ou se a câmera trocou.
        if (Screen.width != lastScreenWidth ||
            Screen.height != lastScreenHeight ||
            Camera.main != lastMainCam)
        {
            AplicarAjusteCompleto();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AplicarAjusteCompleto();
    }

    private void AplicarAjusteCompleto()
    {
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
        lastMainCam = Camera.main;

        AjustarCameras();
        if (ForcarCanvasScaler) AjustarCanvas();
    }

    private void CriarCameraDeFundo()
    {
        GameObject bg = new GameObject("LetterboxBackground");
        bg.transform.SetParent(transform, false);

        backgroundCam = bg.AddComponent<Camera>();
        backgroundCam.clearFlags = CameraClearFlags.SolidColor;
        backgroundCam.backgroundColor = Color.black;
        backgroundCam.cullingMask = 0;          // não desenha nada, só limpa a tela de preto
        backgroundCam.depth = -100;             // roda antes de todas as outras câmeras
        backgroundCam.orthographic = true;
        backgroundCam.allowHDR = false;
        backgroundCam.allowMSAA = false;
        backgroundCam.useOcclusionCulling = false;
    }

    private Rect CalcularViewport()
    {
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / TargetAspect;

        if (scaleHeight < 1.0f)
        {
            // Tela mais "alta" que 16:9 (tablet 4:3, celular em pé) -> barras em cima e embaixo
            return new Rect(0f, (1.0f - scaleHeight) / 2.0f, 1.0f, scaleHeight);
        }

        // Tela mais larga que 16:9 (celular 20:9, ultrawide) -> barras nas laterais
        float scaleWidth = 1.0f / scaleHeight;
        return new Rect((1.0f - scaleWidth) / 2.0f, 0f, scaleWidth, 1.0f);
    }

    private void AjustarCameras()
    {
        Rect rect = CalcularViewport();

        foreach (Camera cam in Camera.allCameras)
        {
            if (cam == backgroundCam) continue;
            if (cam.targetTexture != null) continue; // câmeras que renderizam em textura não entram

            cam.rect = rect;
        }

        Camera main = Camera.main;
        if (main != null)
        {
            main.clearFlags = CameraClearFlags.SolidColor;
            main.backgroundColor = Color.black;
        }
    }

    private void AjustarCanvas()
    {
        CanvasScaler[] scalers = FindObjectsByType<CanvasScaler>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (CanvasScaler scaler in scalers)
        {
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(RefWidth, RefHeight);
            // Expand: escala = menor entre (largura/1280) e (altura/720).
            // Mesma escala da área 16:9 do jogo e a UI nunca fica cortada.
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            scaler.matchWidthOrHeight = 0.5f;
        }
    }
}
