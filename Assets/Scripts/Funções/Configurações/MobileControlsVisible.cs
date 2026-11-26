using UnityEngine;

public class MobileControlsVisible : MonoBehaviour
{
    [Header("Painel dos Botoes Tateis")]
    public GameObject mobileControlsPanel;

    [Header("Modo de Teste no Editor")]
    [Tooltip("Marca esta caixa se quiseres forçar os botões a aparecer no Unity Editor / Simulator para testes.")]
    public bool forceShowInEditor = false;

    void Start()
    {
        UpdateControlsVisibility();
    }

    void UpdateControlsVisibility()
    {
        if (mobileControlsPanel == null) return;

        #if UNITY_EDITOR
        if (forceShowInEditor)
        {
            mobileControlsPanel.SetActive(true);
            return;
        }
        #endif

        if (Application.isMobilePlatform)
        {
            mobileControlsPanel.SetActive(true);
        }
        else
        {
            mobileControlsPanel.SetActive(false);
        }
    }
}

// using UnityEngine;

// public class MobileControlsVisible : MonoBehaviour
// {
//     [Header("Painel dos Botoes Tateis")]
//     public GameObject mobileControlsPanel;

//     [Header("Modo de Teste no Editor")]
//     public bool forceShowInEditor = false;

//     void Start()
//     {
//         UpdateControlsVisibility();
//     }

//     void UpdateControlsVisibility()
//     {
//         if (mobileControlsPanel == null) return;

//         #if UNITY_EDITOR
//         if (forceShowInEditor)
//         {
//             mobileControlsPanel.SetActive(true);
//             return;
//         }
//         #endif

//         // Se o hardware for um PC/Desktop, esconde sempre os botões mobile
//         if (SystemInfo.deviceType == DeviceType.Desktop)
//         {
//             mobileControlsPanel.SetActive(false);
//         }
//         else
//         {
//             // Apenas em telemóveis e tablets reais (Handheld)
//             mobileControlsPanel.SetActive(true);
//         }
//     }
// }