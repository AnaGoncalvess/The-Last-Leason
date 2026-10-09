using System;
using UnityEngine;
using UnityEngine.SceneManagement; // <- ADICIONE ESTA LINHA NO TOPO

/// <summary>
/// Singleton de pausa que se auto-instancia. Aperte "Cancel" (Esc / joystick 1) para
/// pausar/despausar. Um painel de UI é opcional: sem ele, o jogo já pausa corretamente
/// (Time.timeScale) e outros scripts podem assinar OnPauseChanged para reagir.
/// </summary>
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

    [SerializeField] private GameObject pausePanel; // opcional: painel de UI a ativar/desativar

    [SerializeField] private string mainMenuSceneName = "Tela-inicial"; // Ajuste o nome da cena conforme seu projeto

    private void Awake()
    {
        // Propositalmente SEM DontDestroyOnLoad: este singleton é por cena. "pausePanel" é uma
        // referência de UI daquela cena específica; se persistisse entre cenas (ex: reiniciar o
        // nível), o singleton velho continuaria de pé segurando um "instance" já preenchido, a
        // instância nova (com o painel novo) seria destruída no seu próprio Awake, e o pause
        // ficaria travado pra sempre apontando pra um painel já destruído.
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

    public void Resume()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false); // Esconde o painel
        }

        Time.timeScale = 1f; // Volta o tempo do jogo ao normal
                             // isPaused = false;
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

    public void RestartGame()
{
    // 1. Restaura o tempo do jogo (para despausar)
    Time.timeScale = 1f;

    // 2. Reseta o progresso global dos coletáveis (Cristais e Lâmpadas)
    ColetaveisManager.ResetarProgressoGlobal();

    // 3. Reseta a vida perdida do jogador
    Player.ResetHealth();

    // 4. Recarrega a fase atual ou volta para o Nível 1
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Restaura a velocidade do jogo
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void ResumeGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        Time.timeScale = 1f; // Restaura a velocidade do jogo
    }
}
