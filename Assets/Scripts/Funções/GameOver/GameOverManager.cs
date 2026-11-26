using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Painel da Tela de Game Over")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("UI do Jogo (HUD)")]
    [SerializeField] private GameObject hudPanel;

    [Header("Nome da Cena do Menu")]
    [SerializeField] private string mainMenuSceneName = "Tela-inicial";

    private void Awake()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void ShowGameOver()
    {
        // Esconde a interface de gameplay (vidas e coletáveis)
        if (hudPanel != null)
        {
            hudPanel.SetActive(false);
        }

        // Exibe a tela de Game Over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    // BOTÃO REINICIAR DA TELA DE GAME OVER
    public void RestartGame()
    {
        Time.timeScale = 1f; // Restaura o tempo do jogo

        // Zera a vida do jogador e os coletáveis acumulados
        Player.ResetHealth();
        ColetaveisManager.ResetarProgressoGlobal();

        // Recarrega a fase atual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // BOTÃO MENU DA TELA DE GAME OVER
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Restaura o tempo do jogo

        // Zera a vida do jogador e os coletáveis acumulados
        Player.ResetHealth();
        ColetaveisManager.ResetarProgressoGlobal();

        // Volta para a cena do Menu Principal
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void GameOver()
{
    
    // Procura o script e esconde os controlos mobile
    HideMobileControlsOnEnd mobileHider = FindObjectOfType<HideMobileControlsOnEnd>();
    if (mobileHider != null)
    {
        mobileHider.HideControls();
    }
}
}