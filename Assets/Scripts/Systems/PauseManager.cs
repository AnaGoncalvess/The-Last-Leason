// using System;
// using UnityEngine;
// using UnityEngine.SceneManagement; // <- ADICIONE ESTA LINHA NO TOPO

// /// <summary>
// /// Singleton de pausa que se auto-instancia. Aperte "Cancel" (Esc / joystick 1) para
// /// pausar/despausar. Um painel de UI é opcional: sem ele, o jogo já pausa corretamente
// /// (Time.timeScale) e outros scripts podem assinar OnPauseChanged para reagir.
// /// </summary>
// public class PauseManager : MonoBehaviour
// {
//     private static PauseManager instance;

//     public static PauseManager Instance
//     {
//         get
//         {
//             if (instance == null)
//             {
//                 GameObject go = new GameObject("PauseManager (Auto)");
//                 instance = go.AddComponent<PauseManager>();
//             }

//             return instance;
//         }
//     }

//     public static event Action<bool> OnPauseChanged;

//     public bool IsPaused { get; private set; }

//     [SerializeField] private GameObject pausePanel; // opcional: painel de UI a ativar/desativar

//     [SerializeField] private string mainMenuSceneName = "Tela-inicial"; // Ajuste o nome da cena conforme seu projeto

//     private void Awake()
//     {
//         // Propositalmente SEM DontDestroyOnLoad: este singleton é por cena. "pausePanel" é uma
//         // referência de UI daquela cena específica; se persistisse entre cenas (ex: reiniciar o
//         // nível), o singleton velho continuaria de pé segurando um "instance" já preenchido, a
//         // instância nova (com o painel novo) seria destruída no seu próprio Awake, e o pause
//         // ficaria travado pra sempre apontando pra um painel já destruído.
//         if (instance != null && instance != this)
//         {
//             Destroy(gameObject);
//             return;
//         }

//         instance = this;
//     }

//     private void OnDestroy()
//     {
//         if (instance == this)
//         {
//             instance = null;
//         }
//     }

//     private void Update()
//     {
//         if (Input.GetButtonDown("Cancel"))
//         {
//             TogglePause();
//         }
//     }

//     public void TogglePause()
//     {
//         SetPaused(!IsPaused);
//     }

//     public void Resume()
//     {
//         if (pausePanel != null)
//         {
//             pausePanel.SetActive(false); // Esconde o painel
//         }

//         Time.timeScale = 1f; // Volta o tempo do jogo ao normal
//                              // isPaused = false;
//     }

//     public void SetPaused(bool paused)
//     {
//         IsPaused = paused;
//         Time.timeScale = paused ? 0f : 1f;

//         if (pausePanel != null)
//         {
//             pausePanel.SetActive(paused);
//         }

//         OnPauseChanged?.Invoke(paused);
//     }



//     // 1. BOTÃO CONTINUAR
//     public void ResumeGame()
//     {
//         if (pausePanel != null)
//         {
//             pausePanel.SetActive(false);
//         }
//         Time.timeScale = 1f; // Voltar o tempo do jogo ao normal
//     }

//     // 2. BOTÃO REINICIAR (Recarrega a fase limpando vida e coletáveis)
//     public void RestartGame()
//     {
//         Time.timeScale = 1f; // IMPORTANTE: Restaura a velocidade do jogo

//         // Zera todos os dados estáticos guardados entre fases
//         Player.ResetHealth();
//         ColetaveisManager.ResetarProgressoGlobal();

//         // Recarrega a cena atual (ou a primeira fase)
//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//     }

//     // 3. BOTÃO MENU (Volta ao menu principal limpando vida e coletáveis)
//     public void GoToMainMenu()
//     {
//         Time.timeScale = 1f; // IMPORTANTE: Restaura a velocidade do jogo

//         // Zera os dados estáticos antes de ir para o Menu
//         Player.ResetHealth();
//         ColetaveisManager.ResetarProgressoGlobal();

//         // Carrega a cena do Menu Principal
//         SceneManager.LoadScene(mainMenuSceneName);
//     }
// }

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private static PauseManager instance;

    public static PauseManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("PauseManager (Auto)");
                instance = go.AddComponent<PauseManager>();
            }

            return instance;
        }
    }

    public static event Action<bool> OnPauseChanged;

    public bool IsPaused { get; private set; }

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private string mainMenuSceneName = "Tela-inicial";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        SetPaused(!IsPaused);
    }

    public void SetPaused(bool paused)
    {
        IsPaused = paused;
        Time.timeScale = paused ? 0f : 1f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(paused);
        }

        OnPauseChanged?.Invoke(paused);
    }

    // 1. BOTÃO CONTINUAR
    public void ResumeGame()
    {
        SetPaused(false);
    }

    // 2. BOTÃO REINICIAR (Recarrega a fase limpando vida e coletáveis)
    public void RestartGame()
    {
        Time.timeScale = 1f;

        Player.ResetHealth();
        ColetaveisManager.ResetarProgressoGlobal();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 3. BOTÃO MENU (Volta ao menu principal limpando vida e coletáveis)
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        Player.ResetHealth();
        ColetaveisManager.ResetarProgressoGlobal();

        SceneManager.LoadScene(mainMenuSceneName);
    }
}