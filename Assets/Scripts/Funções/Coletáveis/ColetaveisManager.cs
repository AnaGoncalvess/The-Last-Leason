// using UnityEngine;
// using TMPro;

// public class CristalManager : MonoBehaviour
// {
//     public static CristalManager instance;

//     [Header("UI do Contador")]
//     [SerializeField] private TextMeshProUGUI cristalText;
//     private int cristalCount = 0;

//     private void Awake()
//     {
//         if (instance == null)
//         {
//             instance = this;
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     private void Start()
//     {
//         UpdateUI();
//     }

//     public void AddCristal()
//     {
//         cristalCount++;
//         UpdateUI();
//     }

//     private void UpdateUI()
//     {
//         if (cristalText != null)
//         {
//             cristalText.text = cristalCount.ToString();
//         }
//     }
// }

// using UnityEngine;
// using TMPro;

// public class ColetaveisManager : MonoBehaviour
// {
//     public static ColetaveisManager instance;

//     [Header("UI dos Contadores")]
//     [SerializeField] private TextMeshProUGUI cristalText;
//     [SerializeField] private TextMeshProUGUI moedaText;

//     private int cristalCount = 0;
//     private int moedaCount = 0;



//     [SerializeField] private int totalCristaisNaFase = 1;
//     [SerializeField] private GameObject chavePrefab;
//     [SerializeField] private PortaController porta;
//     [SerializeField] private Transform pontoSpawnChave; // Onde a chave aparece (ex: no último cristal ou no Player)

//     public void AddCristal()
//     {
//         cristalCount++;
//         UpdateUI();

//         if (cristalCount >= totalCristaisNaFase)
//         {
//             ChamarChaveVoadora();
//         }
//     }

//     private void ChamarChaveVoadora()
//     {
//         GameObject chaveObj = Instantiate(chavePrefab, pontoSpawnChave.position, Quaternion.identity);
//         ChaveVoadora chave = chaveObj.GetComponent<ChaveVoadora>();
//         chave.IniciarVoo(porta.PontoFechadura, porta);
//     }

//     private void Awake()
//     {
//         if (instance == null)
//         {
//             instance = this;
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     // private void Start()
//     // {
//     //     UpdateUI();
//     // }

//     private void Start()
//     {
//         totalColetaveisNaCena = FindObjectsByType<ItemColetavel>(FindObjectsSortMode.None).Length;
//         Debug.Log($"Total de itens detectados no início da fase: {totalColetaveisNaCena}");
//         UpdateUI();
//     }

//     private void VerificarConclusao()
//     {
//         Debug.Log($"Itens Recolhidos: {totalColetados} de {totalColetaveisNaCena}");

//         if (totalColetados >= totalColetaveisNaCena && totalColetaveisNaCena > 0)
//         {
//             ChamarChaveVoadora();
//         }
//     }

//     public void AddMoeda()
//     {
//         moedaCount++;
//         UpdateUI();
//     }

//     private void UpdateUI()
//     {
//         if (cristalText != null)
//         {
//             cristalText.text = cristalCount.ToString();
//         }

//         if (moedaText != null)
//         {
//             moedaText.text = moedaCount.ToString();
//         }
//     }
// }

using UnityEngine;
using TMPro;

public class ColetaveisManager : MonoBehaviour
{
    public static ColetaveisManager instance;

    [Header("UI dos Contadores")]
    [SerializeField] private TextMeshProUGUI cristalText;
    [SerializeField] private TextMeshProUGUI moedaText;

    [Header("Configuração da Chave e Porta")]
    [SerializeField] private GameObject chavePrefab;
    [SerializeField] private PortaController porta;
    [SerializeField] private Transform pontoSpawnChave;

    private int cristalCount = 0;
    private int moedaCount = 0;
    private int totalColetaveisNaCena = 0;
    private int totalColetados = 0;
    private bool chaveCriada = false; // Trava para evitar spawn duplo

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        totalColetaveisNaCena = FindObjectsByType<ItemColetavel>(FindObjectsSortMode.None).Length;
        UpdateUI();
    }

    public void AddCristal()
    {
        cristalCount++;
        totalColetados++;
        UpdateUI();
        VerificarConclusao();
    }

    public void AddMoeda()
    {
        moedaCount++;
        totalColetados++;
        UpdateUI();
        VerificarConclusao();
    }

    private void VerificarConclusao()
    {
        if (!chaveCriada && totalColetados >= totalColetaveisNaCena && totalColetaveisNaCena > 0)
        {
            chaveCriada = true; // Garante que só executa uma vez
            ChamarChaveVoadora();
        }
    }

    // private void ChamarChaveVoadora()
    // {
    //     if (chavePrefab != null && porta != null)
    //     {
    //         Vector3 posicaoSpawn = pontoSpawnChave != null ? pontoSpawnChave.position : transform.position;
    //         GameObject chaveObj = Instantiate(chavePrefab, posicaoSpawn, Quaternion.identity);
            
    //         ChaveVoadora chave = chaveObj.GetComponent<ChaveVoadora>();
    //         if (chave != null)
    //         {
    //             chave.IniciarVoo(porta.PontoFechadura, porta);
    //         }
    //     }
    // }

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

    private void UpdateUI()
    {
        if (cristalText != null) cristalText.text = cristalCount.ToString();
        if (moedaText != null) moedaText.text = moedaCount.ToString();
    }
}