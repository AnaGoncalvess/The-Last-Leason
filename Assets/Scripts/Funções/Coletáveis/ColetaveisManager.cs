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

using UnityEngine;
using TMPro;

public class ColetaveisManager : MonoBehaviour
{
    public static ColetaveisManager instance;

    [Header("UI dos Contadores")]
    [SerializeField] private TextMeshProUGUI cristalText;
    [SerializeField] private TextMeshProUGUI moedaText;

    private int cristalCount = 0;
    private int moedaCount = 0;



    [SerializeField] private int totalCristaisNaFase = 1;
    [SerializeField] private GameObject chavePrefab;
    [SerializeField] private PortaController porta;
    [SerializeField] private Transform pontoSpawnChave; // Onde a chave aparece (ex: no último cristal ou no Player)

    public void AddCristal()
    {
        cristalCount++;
        UpdateUI();

        if (cristalCount >= totalCristaisNaFase)
        {
            ChamarChaveVoadora();
        }
    }

    private void ChamarChaveVoadora()
    {
        GameObject chaveObj = Instantiate(chavePrefab, pontoSpawnChave.position, Quaternion.identity);
        ChaveVoadora chave = chaveObj.GetComponent<ChaveVoadora>();
        chave.IniciarVoo(porta.PontoFechadura, porta);
    }

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
        UpdateUI();
    }

    

    public void AddMoeda()
    {
        moedaCount++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (cristalText != null)
        {
            cristalText.text = cristalCount.ToString();
        }

        if (moedaText != null)
        {
            moedaText.text = moedaCount.ToString();
        }
    }
}

