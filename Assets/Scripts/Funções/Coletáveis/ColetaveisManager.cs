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

    public void AddCristal()
    {
        cristalCount++;
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

