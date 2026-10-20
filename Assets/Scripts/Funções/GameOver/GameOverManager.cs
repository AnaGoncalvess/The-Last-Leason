// // using UnityEngine;

// // public class GameOverManager : MonoBehaviour
// // {
// //     [Header("Objeto com a Animação de Game Over")]
// //     [SerializeField] private GameObject gameOverAnimObject;

// //     private void Awake()
// //     {
// //         // Esconde a animação no início do jogo
// //         if (gameOverAnimObject != null)
// //         {
// //             gameOverAnimObject.SetActive(false);
// //         }
// //     }

// //     public void ShowGameOver()
// //     {
// //         if (gameOverAnimObject != null)
// //         {
// //             gameOverAnimObject.SetActive(true);

// //             // Se o objeto tiver um Animator, garante que a animação comece do zero
// //             Animator anim = gameOverAnimObject.GetComponent<Animator>();
// //             if (anim != null)
// //             {
// //                 anim.Play(0); // Toca a animação do estado inicial
// //             }
// //         }
// //         else
// //         {
// //             Debug.LogError("Arraste o objeto da animação no Inspector do GameOverManager!");
// //         }
// //     }
// // }

// // using UnityEngine;
// // using UnityEngine.SceneManagement;

// // public class GameOverManager : MonoBehaviour
// // {
// //     [Header("Tela/Painel Completo de Game Over")]
// //     [SerializeField] private GameObject gameOverPanel; // Arraste o objeto GameOver aqui

// //     [Header("Nome da Cena do Menu")]
// //     [SerializeField] private string mainMenuSceneName = "MainMenu";

// //     private void Awake()
// //     {
// //         // Garante que todo o conjunto (fundo, animação e botões) comece escondido
// //         if (gameOverPanel != null)
// //         {
// //             gameOverPanel.SetActive(false);
// //         }
// //     }

// //     public void ShowGameOver()
// //     {
// //         if (gameOverPanel != null)
// //         {
// //             gameOverPanel.SetActive(true);
// //         }
// //     }

// //     public void RestartGame()
// //     {
// //         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
// //     }

// //     public void GoToMainMenu()
// //     {
// //         SceneManager.LoadScene(mainMenuSceneName);
// //     }
// // }

// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class GameOverManager : MonoBehaviour
// {
//     [Header("Painel da Tela de Game Over")]
//     [SerializeField] private GameObject gameOverPanel;

//     [Header("UI do Jogo (HUD)")]
//     [SerializeField] private GameObject hudPanel; // Arraste o objeto/painel das vidas e coletáveis aqui

//     [Header("Nome da Cena do Menu")]
//     [SerializeField] private string mainMenuSceneName = "MainMenu";

//     private void Awake()
//     {
//         if (gameOverPanel != null)
//         {
//             gameOverPanel.SetActive(false);
//         }
//     }

//     public void ShowGameOver()
//     {
//         // Esconde a interface de gameplay (vidas e coletáveis)
//         if (hudPanel != null)
//         {
//             hudPanel.SetActive(false);
//         }

//         // Exibe a tela de Game Over
//         if (gameOverPanel != null)
//         {
//             gameOverPanel.SetActive(true);
//         }
//     }

//     // public void RestartGame()
//     // {
//     //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//     // }

//     // public void GoToMainMenu()
//     // {
//     //     SceneManager.LoadScene(mainMenuSceneName);
//     // }

//     // public void RestartGame()
//     // {
//     //     Time.timeScale = 1f; // Restaura a velocidade do jogo
//     //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//     // }

//     // public void GoToMainMenu()
//     // {
//     //     Time.timeScale = 1f; // Restaura a velocidade do jogo
//     //     SceneManager.LoadScene(mainMenuSceneName);
//     // }

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


// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class GameOverManager : MonoBehaviour
// {
//     [Header("Painel da Tela de Game Over")]
//     [SerializeField] private GameObject gameOverPanel;

//     [Header("UI do Jogo (HUD)")]
//     [SerializeField] private GameObject hudPanel;

//     [Header("Nome da Cena do Menu")]
//     [SerializeField] private string mainMenuSceneName = "Tela-inicial";

//     private void Awake()
//     {
//         if (gameOverPanel != null)
//         {
//             gameOverPanel.SetActive(false);
//         }
//     }

//     public void ShowGameOver()
//     {
//         // Esconde a interface de gameplay
//         if (hudPanel != null)
//         {
//             hudPanel.SetActive(false);
//         }

//         // Exibe a tela de Game Over
//         if (gameOverPanel != null)
//         {
//             gameOverPanel.SetActive(true);
//         }
//     }

//     // BOTÃO REINICIAR DA TELA DE GAME OVER
//     public void RestartGame()
//     {
//         Time.timeScale = 1f;

//         // Zera vida e coletáveis ao reiniciar
//         Player.ResetHealth();
//         ColetaveisManager.ResetarProgressoGlobal();

//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//     }

//     // BOTÃO MENU DA TELA DE GAME OVER
//     public void GoToMainMenu()
//     {
//         Time.timeScale = 1f;

//         // Zera vida e coletáveis ao ir pro menu
//         Player.ResetHealth();
//         ColetaveisManager.ResetarProgressoGlobal();

//         SceneManager.LoadScene(mainMenuSceneName);
//     }
// }

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
}