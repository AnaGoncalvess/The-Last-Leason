using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    public void AtualizarBarra(float vidaAtual, float vidaMaxima)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = Mathf.Clamp01(vidaAtual / vidaMaxima);
        }
    }
}