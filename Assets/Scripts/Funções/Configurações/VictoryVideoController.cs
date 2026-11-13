// using UnityEngine;
// using UnityEngine.Video;
// using UnityEngine.SceneManagement;

// public class VictoryVideoController : MonoBehaviour
// {
//     [Header("Componentes de Vídeo")]
//     [SerializeField] private VideoPlayer videoPlayer;
//     [SerializeField] private GameObject botaoVoltarMenu; // Aparece quando os créditos terminam

//     [Header("Configuração de Saída")]
//     [SerializeField] private string nomeCenaMenu = "Menu";
//     [SerializeField] private bool voltarAutomaticamenteAoTerminar = false;

//     private void Awake()
//     {
//         if (videoPlayer == null)
//         {
//             videoPlayer = GetComponent<VideoPlayer>();
//         }
//     }

//     private void OnEnable()
//     {
//         // Oculta o botão de menu no início do vídeo para não atrapalhar a leitura
//         if (botaoVoltarMenu != null)
//         {
//             botaoVoltarMenu.SetActive(false);
//         }

//         // Para a música de fundo da fase (se houver AudioManager) para não misturar com o áudio do vídeo
//         if (AudioManager.Instance != null)
//         {
//             AudioSource[] fontesAudio = AudioManager.Instance.GetComponentsInChildren<AudioSource>();
//             foreach (AudioSource fonte in fontesAudio)
//             {
//                 fonte.Stop();
//             }
//         }

//         if (videoPlayer != null)
//         {
//             // Inscreve o evento que avisa quando o vídeo chegou ao fim
//             videoPlayer.loopPointReached += AoTerminarVideo;
//             videoPlayer.Play();
//         }
//     }

//     private void OnDisable()
//     {
//         if (videoPlayer != null)
//         {
//             videoPlayer.loopPointReached -= AoTerminarVideo;
//         }
//     }

//     private void AoTerminarVideo(VideoPlayer vp)
//     {
//         Debug.Log("Vídeo de vitória e créditos finalizado!");

//         if (voltarAutomaticamenteAoTerminar)
//         {
//             VoltarParaMenu();
//         }
//         else if (botaoVoltarMenu != null)
//         {
//             botaoVoltarMenu.SetActive(true);
//         }
//     }

//     // Conecte esta função no OnClick() do botão "Voltar ao Menu"
//     public void VoltarParaMenu()
//     {
//         Time.timeScale = 1f;
//         Player.ResetHealth();
//         ColetaveisManager.ResetarProgressoGlobal();
//         SceneManager.LoadScene(nomeCenaMenu);
//     }
// }

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VictoryVideoController : MonoBehaviour
{
    [Header("Componentes de Vídeo")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage rawImageVideo; // Arraste o objeto 'TelaVideo' (RawImage) aqui
    [SerializeField] private GameObject botaoVoltarMenu;

    [Header("Configuração de Saída")]
    [SerializeField] private string nomeCenaMenu = "Menu";
    [SerializeField] private bool voltarAutomaticamenteAoTerminar = false;

    private void Awake()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }
    }

    private void OnEnable()
    {
        if (botaoVoltarMenu != null)
        {
            botaoVoltarMenu.SetActive(false);
        }

        // 1. Esconde a RawImage imediatamente para não mostrar o frame antigo preso na Render Texture
        if (rawImageVideo != null)
        {
            rawImageVideo.enabled = false;
        }

        // 2. Limpa a Render Texture fisicamente da memória
        if (videoPlayer != null && videoPlayer.targetTexture != null)
        {
            RenderTexture rtAtiva = RenderTexture.active;
            RenderTexture.active = videoPlayer.targetTexture;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = rtAtiva;
        }

        // Para a música de fundo da fase
        if (AudioManager.Instance != null)
        {
            AudioSource[] fontesAudio = AudioManager.Instance.GetComponentsInChildren<AudioSource>();
            foreach (AudioSource fonte in fontesAudio)
            {
                fonte.Stop();
            }
        }

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += AoTerminarVideo;
            videoPlayer.prepareCompleted += AoPrepararVideo;

            // Prepara o vídeo do zero (frame 0) antes de exibir
            videoPlayer.Prepare();
        }
    }

    private void OnDisable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= AoTerminarVideo;
            videoPlayer.prepareCompleted -= AoPrepararVideo;
        }
    }

    // Só exibe a imagem e dá Play quando o frame 0 estiver 100% carregado
    private void AoPrepararVideo(VideoPlayer vp)
    {
        if (rawImageVideo != null)
        {
            rawImageVideo.enabled = true;
        }
        vp.Play();
    }

    private void AoTerminarVideo(VideoPlayer vp)
    {
        if (voltarAutomaticamenteAoTerminar)
        {
            VoltarParaMenu();
        }
        else if (botaoVoltarMenu != null)
        {
            botaoVoltarMenu.SetActive(true);
        }
    }

    public void VoltarParaMenu()
    {
        Time.timeScale = 1f;
        Player.ResetHealth();
        ColetaveisManager.ResetarProgressoGlobal();
        SceneManager.LoadScene(nomeCenaMenu);
    }
}