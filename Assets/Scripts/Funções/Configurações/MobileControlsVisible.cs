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

// [ExecuteAlways]
// public class MobileControlsVisible : MonoBehaviour
// {
//     [Header("Painel dos Botoes Tateis")]
//     public GameObject mobileControlsPanel;

//     [Header("Modo de Teste no Editor")]
//     [Tooltip("Marca esta caixa se quiseres forçar os botões a aparecer no Unity Editor para testes.")]
//     public bool forceShowInEditor = false;

//     void Start()
//     {
//         UpdateControlsVisibility();
//     }

//     // Executado automaticamente no Editor sempre que mudas algo no Inspector ou voltas a focar o Unity
//     void OnValidate()
//     {
//         UpdateControlsVisibility();
//     }

//     void UpdateControlsVisibility()
//     {
//         if (mobileControlsPanel == null) return;

//         // Se estivermos no Editor do Unity
//         #if UNITY_EDITOR
//         if (!Application.isPlaying)
//         {
//             // No modo de edição (fora do Play), respeita a caixa de teste para conseguires ver o painel
//             mobileControlsPanel.SetActive(forceShowInEditor);
//             return;
//         }

//         // Se estouver a correr no Editor em modo Play
//         if (forceShowInEditor)
//         {
//             mobileControlsPanel.SetActive(true);
//             return;
//         }
//         #endif

//         // Comportamento normal em Build / Jogo real
//         if (Application.isMobilePlatform)
//         {
//             mobileControlsPanel.SetActive(true);
//         }
//         else
//         {
//             mobileControlsPanel.SetActive(false);
//         }
//     }
// }