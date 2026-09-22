using System;
using UnityEngine;

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
        SetPaused(false);
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
}
