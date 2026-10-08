using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ColetaveisManager : MonoBehaviour
{
    public static ColetaveisManager Instance { get; private set; }
    public static ColetaveisManager instance => Instance;

    // Adicione no ColetaveisManager.cs
    public static int TotalCristaisGlobal => totalCristaisGlobal;
    public static int TotalMoedasGlobal => totalMoedasGlobal;

    [Header("UI dos Contadores na Prancheta")]
    [SerializeField] private TextMeshProUGUI cristalText;
    [SerializeField] private TextMeshProUGUI moedaText;

    [Header("Configuração da Chave e Porta")]
    [SerializeField] private GameObject chavePrefab;
    [SerializeField] private PortaController porta;
    [SerializeField] private Transform pontoSpawnChave;

    // Totais globais mantidos entre as fases
    private static int totalCristaisGlobal = 0;
    private static int totalMoedasGlobal = 0;

    private int coletaveisNaCenaAtual = 0;
    private int coletadosNaCenaAtual = 0;
    private bool chaveCriada = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        coletadosNaCenaAtual = 0;
        chaveCriada = false;

        // Força encontrar os novos componentes de UI da cena carregada
        BuscarReferenciasNaCena();

        // Quantidade de coletáveis na nova fase
        coletaveisNaCenaAtual = FindObjectsByType<ItemColetavel>(FindObjectsSortMode.None).Length;

        UpdateUI();
    }

    public void BuscarReferenciasNaCena()
    {
        // Reencontra a porta do novo nível
        porta = FindFirstObjectByType<PortaController>();

        // Reencontra os textos de pontuação do Canvas atual na cena
        TextMeshProUGUI[] todosTextos = FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);

        foreach (var texto in todosTextos)
        {
            string nomeObjeto = texto.gameObject.name.ToLower();

            if (nomeObjeto.Contains("cristal") || nomeObjeto.Contains("txt_cristal"))
            {
                cristalText = texto;
            }
            else if (nomeObjeto.Contains("moeda") || nomeObjeto.Contains("lampada") || nomeObjeto.Contains("txt_lampada") || nomeObjeto.Contains("txt_moeda"))
            {
                moedaText = texto;
            }
        }
    }

    // public void AddCristal()
    // {
    //     totalCristaisGlobal++;
    //     coletadosNaCenaAtual++;
    //     UpdateUI();
    //     VerificarConclusao();
    // }

    // public void AddMoeda()
    // {
    //     totalMoedasGlobal++;
    //     coletadosNaCenaAtual++;
    //     UpdateUI();
    //     VerificarConclusao();
    // }
    public void AddCristal()
    {
        totalCristaisGlobal++;
        coletadosNaCenaAtual++;
        UpdateUI();
        VerificarConclusao();
    }

    public void AddMoeda()
    {
        totalMoedasGlobal++;
        coletadosNaCenaAtual++;
        UpdateUI();
        VerificarConclusao();
    }

    private void UpdateUI()
    {
        // Se as referências da UI estiverem perdidas/nulas, reconecta imediatamente
        if (cristalText == null || moedaText == null)
        {
            ReconectarReferenciasEAtualizarUI();
            return;
        }

        if (cristalText != null) cristalText.text = totalCristaisGlobal.ToString();
        if (moedaText != null) moedaText.text = totalMoedasGlobal.ToString();
    }

    public void ReconectarReferenciasEAtualizarUI()
    {
        // Busca todos os TextMeshProUGUI presentes na cena do Nível 2
        TextMeshProUGUI[] todosTextos = FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);

        foreach (var texto in todosTextos)
        {
            string nome = texto.gameObject.name.ToLower();

            if (nome.Contains("cristal"))
            {
                cristalText = texto;
            }
            else if (nome.Contains("lampada") || nome.Contains("moeda"))
            {
                moedaText = texto;
            }
        }

        // Atualiza os valores visuais na tela
        if (cristalText != null) cristalText.text = totalCristaisGlobal.ToString();
        if (moedaText != null) moedaText.text = totalMoedasGlobal.ToString();
    }
    private void VerificarConclusao()
    {
        if (!chaveCriada && coletadosNaCenaAtual >= coletaveisNaCenaAtual && coletaveisNaCenaAtual > 0)
        {
            chaveCriada = true;
            ChamarChaveVoadora();
        }
    }

    private void ChamarChaveVoadora()
    {
        if (chavePrefab != null && porta != null)
        {
            Vector3 posicaoSpawn = pontoSpawnChave != null ? pontoSpawnChave.position : transform.position;
            GameObject chaveObj = Instantiate(chavePrefab, posicaoSpawn, Quaternion.identity);

            ChaveColetavel chave = chaveObj.GetComponent<ChaveColetavel>();
            if (chave != null)
            {
                chave.ConfigurarChave(porta);
            }
        }
    }

    // private void UpdateUI()
    // {
    //     // Se as referências estiverem nulas, tenta reatribuir antes de escrever
    //     if (cristalText == null || moedaText == null)
    //     {
    //         BuscarReferenciasNaCena();
    //     }

    //     if (cristalText != null) cristalText.text = totalCristaisGlobal.ToString();
    //     if (moedaText != null) moedaText.text = totalMoedasGlobal.ToString();
    // }

    public static void ResetarProgressoGlobal()
    {
        totalCristaisGlobal = 0;
        totalMoedasGlobal = 0;
    }
}