using UnityEngine;
using UnityEngine.UI;

public class CameraAspectFitter : MonoBehaviour
{
    private const float TargetAspect = 1280f / 720f; // Proporção alvo exata (16:9)
    private int lastScreenWidth;
    private int lastScreenHeight;
    private Camera mainCam;

    private void Start()
    {
        AplicarAjusteCompleto();
    }

    private void Update()
    {
        // Reajusta se a janela do jogo for redimensionada
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            AplicarAjusteCompleto();
        }
    }

    private void AplicarAjusteCompleto()
    {
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;

        AjustarCameraPrincipal();
        // Nota: O ajuste forçado do Canvas para 1920x1080 foi removido daqui para proteger o teu projeto!
    }

    private void AjustarCameraPrincipal()
    {
        mainCam = GetComponent<Camera>();
        if (mainCam == null) mainCam = Camera.main;
        if (mainCam == null) return;

        // Garante fundo preto nas barras laterais/superiores se a tela mudar de proporção
        mainCam.clearFlags = CameraClearFlags.SolidColor;
        mainCam.backgroundColor = Color.black;

        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / TargetAspect;

        Rect rect = mainCam.rect;

        if (scaleHeight < 1.0f)
        {
            // Tela mais alta que o normal -> barras pretas em cima e embaixo
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0f;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else
        {
            // Tela mais larga -> barras pretas nas laterais
            float scaleWidth = 1.0f / scaleHeight;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0f;
        }

        mainCam.rect = rect;
    }
}


// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

// public class CameraAspectFitter : MonoBehaviour
// {
//     private const float TargetAspect = 16f / 9f; // Proporção padrão do PC da escola
//     private int lastScreenWidth;
//     private int lastScreenHeight;
//     private Camera mainCam;

//     // Roda automaticamente em TODAS as cenas sem precisar colocar em nenhum objeto!
//     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
//     private static void Init()
//     {
//         GameObject obj = new GameObject("AutoResponsiveManager");
//         DontDestroyOnLoad(obj);
//         obj.AddComponent<CameraAspectFitter>();
//     }

//     private void OnEnable()
//     {
//         SceneManager.sceneLoaded += OnSceneLoaded;
//     }

//     private void OnDisable()
//     {
//         SceneManager.sceneLoaded -= OnSceneLoaded;
//     }

//     private void Start()
//     {
//         AplicarAjusteCompleto();
//     }

//     private void Update()
//     {
//         // Reajusta na hora se o jogador redimensionar a janela no navegador ou PC
//         if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
//         {
//             AplicarAjusteCompleto();
//         }
//     }

//     private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//     {
//         AplicarAjusteCompleto();
//     }

//     private void AplicarAjusteCompleto()
//     {
//         lastScreenWidth = Screen.width;
//         lastScreenHeight = Screen.height;

//         AjustarCameraPrincipal();
//         AjustarTodosOsCanvas();
//     }

//     private void AjustarCameraPrincipal()
//     {
//         mainCam = Camera.main;
//         if (mainCam == null) return;

//         // Garante fundo preto nas barras laterais/superiores
//         mainCam.clearFlags = CameraClearFlags.SolidColor;
//         mainCam.backgroundColor = Color.black;

//         float windowAspect = (float)Screen.width / Screen.height;
//         float scaleHeight = windowAspect / TargetAspect;

//         Rect rect = mainCam.rect;

//         if (scaleHeight < 1.0f)
//         {
//             // Tela mais alta/quadrada que 16:9 -> barras pretas em cima e embaixo
//             rect.width = 1.0f;
//             rect.height = scaleHeight;
//             rect.x = 0f;
//             rect.y = (1.0f - scaleHeight) / 2.0f;
//         }
//         else
//         {
//             // Tela mais larga que 16:9 (como notebook 16:10 ou ultrawide) -> barras pretas nas laterais
//             float scaleWidth = 1.0f / scaleHeight;
//             rect.width = scaleWidth;
//             rect.height = 1.0f;
//             rect.x = (1.0f - scaleWidth) / 2.0f;
//             rect.y = 0f;
//         }

//         mainCam.rect = rect;
//     }

//     private void AjustarTodosOsCanvas()
//     {
//         CanvasScaler[] scalers = FindObjectsByType<CanvasScaler>(FindObjectsSortMode.None);
//         foreach (CanvasScaler scaler in scalers)
//         {
//             scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
//             scaler.referenceResolution = new Vector2(1920f, 1080f);
//             scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
//             scaler.matchWidthOrHeight = 0.5f;
//         }
//     }
// }