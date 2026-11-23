using UnityEngine;

public class MobileControlsVisible : MonoBehaviour
{
    void Awake()
    {
        // Verifica se está a correr num telemóvel/tablet OU se estamos a forçar o teste no editor do Unity
        bool isMobile = Application.isMobilePlatform || SystemInfo.deviceType == DeviceType.Handheld;

        #if UNITY_EDITOR
        // Se estiveres no Editor do Unity e quiseres testar os botões na tela, muda para 'true'
        // Quando fores publicar para PC/Itch.io, podes mudar para 'false' para esconder no PC
        isMobile = true; 
        #endif

        // Ativa ou desativa o objeto (o painel ou os botões) com base na plataforma
        gameObject.SetActive(isMobile);
    }
}