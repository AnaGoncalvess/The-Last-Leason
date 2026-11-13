// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class LevelExitCheck : MonoBehaviour
// {
//     [Header("Requisitos do Nível")]
//     [SerializeField] private bool precisaDeChave = true;
//     [SerializeField] private bool temChaveColetada = false;

//     [Header("Inimigos Obrigatórios (Opcional)")]
//     [SerializeField] private GameObject inimigoObrigatorio; // Ex: O Caderno do Nível 1

//     [Header("Interface de Falha")]
//     [SerializeField] private GameObject painelNaoFoiDessaVez;

//     private bool nivelPerdido = false;

//     private void Start()
//     {
//         if (painelNaoFoiDessaVez != null)
//         {
//             painelNaoFoiDessaVez.SetActive(false);
//         }
//     }

//     // Função pública chamada quando a chave é coletada no mapa
//     public void ColetarChave()
//     {
//         temChaveColetada = true;
//         Debug.Log("Chave coletada com sucesso!");
//     }

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (nivelPerdido) return;

//         // Verifica se o Player encostou no ponto final da fase
//         if (collision.CompareTag("Player") || collision.GetComponentInParent<Player>() != null)
//         {
//             bool chaveOk = !precisaDeChave || temChaveColetada;
//             bool inimigoOk = (inimigoObrigatorio == null) || (!inimigoObrigatorio.activeInHierarchy);

//             if (chaveOk && inimigoOk)
//             {
//                 Debug.Log("Fase concluída com sucesso! Avançando...");
//                 // Coloque aqui a chamada para avançar de fase (ex: SceneManager.LoadScene("ProximaFase"))
//             }
//             else
//             {
//                 // Jogador esqueceu a chave ou deixou o inimigo vivo
//                 ExibirTelaFalha();
//             }
//         }
//     }

//     private void ExibirTelaFalha()
//     {
//         nivelPerdido = true;
//         Time.timeScale = 0f; // Pausa o jogo

//         if (painelNaoFoiDessaVez != null)
//         {
//             painelNaoFoiDessaVez.SetActive(true);
//         }
//     }

//     // Método atribuído ao botão "TENTAR NOVAMENTE" no Canvas
//     public void ReiniciarFase()
//     {
//         Time.timeScale = 1f; // Despausa o tempo
//         Player.ResetHealth(); // Reseta os acertos acumulados
//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//     }
// }

// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class LevelExitCheck : MonoBehaviour
// {
//     [Header("Requisitos do Nível")]
//     [SerializeField] private bool precisaDeChave = true;
//     public bool temChaveColetada = false; // Marcado como public para fácil depuração

//     [Header("Inimigos Obrigatórios (Opcional)")]
//     [SerializeField] private GameObject inimigoObrigatorio; // Ex: O Caderno

//     [Header("Interface de Falha")]
//     [SerializeField] private GameObject painelNaoFoiDessaVez;

//     private bool nivelPerdido = false;

//     private void Start()
//     {
//         if (painelNaoFoiDessaVez != null)
//         {
//             painelNaoFoiDessaVez.SetActive(false);
//         }
//     }

//     // Função pública chamada quando a chave é coletada no mapa
//     public void ColetarChave()
//     {
//         temChaveColetada = true;
//         Debug.Log("Chave coletada com sucesso no LevelExitCheck!");
//     }

//     [Header("Próxima Fase")]
//     [SerializeField] private string nomeDaProximaFase = "NomeDaSuaProximaCena"; // Digita o nome da próxima cena aqui no Inspector

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (nivelPerdido) return;

//         if (collision.CompareTag("Player") || collision.GetComponentInParent<Player>() != null)
//         {
//             bool chaveOk = !precisaDeChave || temChaveColetada;
//             bool inimigoOk = (inimigoObrigatorio == null) || (!inimigoObrigatorio.activeInHierarchy);

//             if (chaveOk && inimigoOk)
//             {
//                 Debug.Log("FASE CONCLUÍDA! O jogador possui a chave e derrotou os inimigos.");

//                 // CARREGA A PRÓXIMA CENA AQUI:
//                 SceneManager.LoadScene(nomeDaProximaFase);
//             }
//             else
//             {
//                 if (!chaveOk) Debug.LogWarning("Passagem Bloqueada: O Player NÃO coletou a chave!");
//                 if (!inimigoOk) Debug.LogWarning("Passagem Bloqueada: O inimigo obrigatório ainda está vivo!");

//                 ExibirTelaFalha();
//             }
//         }
//     }

//     private void ExibirTelaFalha()
//     {
//         nivelPerdido = true;
//         Time.timeScale = 0f; // Pausa o jogo

//         if (painelNaoFoiDessaVez != null)
//         {
//             painelNaoFoiDessaVez.SetActive(true);
//         }
//     }

//     public void ReiniciarFase()
//     {
//         Time.timeScale = 1f; // Despausa o tempo
//         Player.ResetHealth(); // Reseta vida/hits acumulados do Player
//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//     }
// }

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExitCheck : MonoBehaviour
{
    [Header("Requisitos do Nível")]
    [SerializeField] private bool precisaDeChave = true;
    public bool temChaveColetada = false;

    [Header("Inimigos Obrigatórios (Opcional)")]
    [SerializeField] private GameObject inimigoObrigatorio;

    [Header("Próxima Fase")]
    [SerializeField] private string nomeDaProximaFase = "NomeDaSuaProximaCena";
    [SerializeField] private float tempoEsperarAnimacao = 1.0f; // Tempo de duração da animação da porta

    [Header("Referências Visuais")]
    [SerializeField] private Animator portaAnimator; // Arraste o Animator da Porta aqui
    [SerializeField] private GameObject painelNaoFoiDessaVez;

    private bool nivelPerdido = false;
    private bool nivelConcluido = false;

    private void Start()
    {
        if (painelNaoFoiDessaVez != null)
        {
            painelNaoFoiDessaVez.SetActive(false);
        }

        if (portaAnimator == null)
        {
            portaAnimator = GetComponent<Animator>();
        }
    }

    public void ColetarChave()
    {
        temChaveColetada = true;
        Debug.Log("Chave coletada com sucesso no LevelExitCheck!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (nivelPerdido || nivelConcluido) return;

        if (collision.CompareTag("Player") || collision.GetComponentInParent<Player>() != null)
        {
            bool chaveOk = !precisaDeChave || temChaveColetada;
            bool inimigoOk = (inimigoObrigatorio == null) || (!inimigoObrigatorio.activeInHierarchy);

            if (chaveOk && inimigoOk)
            {
                StartCoroutine(AbrirPortaEAvancar());
            }
            else
            {
                if (!chaveOk) Debug.LogWarning("Passagem Bloqueada: O Player NÃO coletou a chave!");
                if (!inimigoOk) Debug.LogWarning("Passagem Bloqueada: O inimigo obrigatório ainda está vivo!");

                ExibirTelaFalha();
            }
        }
    }

    private IEnumerator AbrirPortaEAvancar()
    {
        nivelConcluido = true;

        // Dispara a animação de abrir a porta
        if (portaAnimator != null)
        {
            portaAnimator.SetTrigger("abrir"); // Garanta que o Trigger no Animator se chama 'Abrir'
        }

        // Aguarda a animação terminar antes de trocar de cena
        yield return new WaitForSeconds(tempoEsperarAnimacao);

        SceneManager.LoadScene(nomeDaProximaFase);
    }

    private void ExibirTelaFalha()
    {
        nivelPerdido = true;
        Time.timeScale = 0f;

        if (painelNaoFoiDessaVez != null)
        {
            painelNaoFoiDessaVez.SetActive(true);
        }
    }

    public void ReiniciarFase()
    {
        Time.timeScale = 1f;
        Player.ResetHealth();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}