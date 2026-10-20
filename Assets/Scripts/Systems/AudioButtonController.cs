using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AudioButtonController : MonoBehaviour
{
    private void Start()
    {
        Button btn = GetComponent<Button>();
        Image img = GetComponent<Image>(); // Pega a imagem do próprio botão

        if (AudioManager.Instance != null)
        {
            // Envia o botão e a imagem do botão atual da cena para o AudioManager
            AudioManager.Instance.ConfigurarBotaoUI(btn, img);
        }
    }
}