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