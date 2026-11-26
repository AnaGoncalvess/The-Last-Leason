using UnityEngine;

public class HideMobileControlsOnEnd : MonoBehaviour
{
    [Header("Painel de Controlos Mobile")]
    public GameObject mobileControlsPanel;

    // Podes chamar esta função publicamente quando houver vitória, game over ou ecrã final
    public void HideControls()
    {
        if (mobileControlsPanel != null)
        {
            mobileControlsPanel.SetActive(false);
        }
    }
}