// using UnityEngine;

// public class GameOverManager : MonoBehaviour
// {
//     [Header("Objeto com a Animação de Game Over")]
//     [SerializeField] private GameObject gameOverAnimObject;

//     private void Awake()
//     {
//         // Esconde a animação no início do jogo
//         if (gameOverAnimObject != null)
//         {
//             gameOverAnimObject.SetActive(false);
//         }
//     }

//     public void ShowGameOver()
//     {
//         if (gameOverAnimObject != null)
//         {
//             gameOverAnimObject.SetActive(true);

//             // Se o objeto tiver um Animator, garante que a animação comece do zero
//             Animator anim = gameOverAnimObject.GetComponent<Animator>();
//             if (anim != null)
//             {
//                 anim.Play(0); // Toca a animação do estado inicial
//             }
//         }
//         else
//         {
//             Debug.LogError("Arraste o objeto da animação no Inspector do GameOverManager!");
//         }
//     }
// }

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Tela/Painel Completo de Game Over")]
    [SerializeField] private GameObject gameOverPanel; // Arraste o objeto GameOver aqui

    [Header("Nome da Cena do Menu")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Awake()
    {
        // Garante que todo o conjunto (fundo, animação e botões) comece escondido
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}