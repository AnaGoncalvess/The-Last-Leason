using UnityEngine;

/// <summary>
/// Ponte para o botão de mudo do menu de pausa. Existe porque AudioManager é um singleton que
/// pode persistir entre cenas (DontDestroyOnLoad): ligar o botão direto a uma instância
/// específica dele quebraria depois de um reload de cena. Passando sempre por
/// AudioManager.Instance, o botão continua funcionando não importa qual instância está viva.
/// </summary>
public class AudioMuteToggle : MonoBehaviour
{
    public void Toggle()
    {
        AudioManager.Instance.ToggleMute();
    }
}
