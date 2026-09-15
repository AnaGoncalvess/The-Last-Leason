using UnityEngine;
using UnityEngine.UI; // Necessário para componentes de UI

public class HealthUI : MonoBehaviour
{
    [Header("Componente de UI")]
    [SerializeField] private Image healthImage;

    [Header("Sprites das Etapas da Vida")]
    [SerializeField] private Sprite fullHealthSprite; // Sprite com 0 X (prancheta limpa)
    [SerializeField] private Sprite[] hitSprites;     // Array com 4 sprites (1X, 2X, 3X, 4X)

    private void Awake()
    {
        // Tenta pegar o componente Image no próprio objeto se não foi arrastado no Inspector
        if (healthImage == null)
        {
            healthImage = GetComponent<Image>();
        }
    }

    public void UpdateHealthUI(int currentHits)
    {
        if (healthImage == null)
        {
            Debug.LogError("HealthUI: O componente 'Image' não foi encontrado!");
            return;
        }

        // Se tiver 0 hits ou menos, mostra a prancheta totalmente limpa
        if (currentHits <= 0)
        {
            if (fullHealthSprite != null)
            {
                healthImage.sprite = fullHealthSprite;
            }
        }
        // Atualiza para o sprite de acordo com o número do hit (1 a 4)
        else if (currentHits - 1 < hitSprites.Length)
        {
            if (hitSprites[currentHits - 1] != null)
            {
                healthImage.sprite = hitSprites[currentHits - 1];
            }
        }
    }
}    


