using UnityEngine;

public class AutoHideMobileUI : MonoBehaviour
{
    [Header("Arraste para aqui o painel com os botões do celular")]
    public GameObject painelBotoesMobile;

    [Header("Arraste para aqui TODAS as telas finais (Vitória, Game Over, Erro, etc.)")]
    public GameObject[] telasFinais;

    void Update()
    {
        if (painelBotoesMobile == null || telasFinais == null) return;

        bool algumaTelaFinalAberta = false;

        // Verifica se qualquer uma das telas finais está ativa/visível no momento
        for (int i = 0; i < telasFinais.Length; i++)
        {
            if (telasFinais[i] != null && telasFinais[i].activeInHierarchy)
            {
                algumaTelaFinalAberta = true;
                break;
            }
        }

        // Se alguma tela final abriu, esconde os botões mobile. Se fechou, volta a mostrar.
        if (algumaTelaFinalAberta && painelBotoesMobile.activeSelf)
        {
            painelBotoesMobile.SetActive(false);
        }
        else if (!algumaTelaFinalAberta && !painelBotoesMobile.activeSelf)
        {
            painelBotoesMobile.SetActive(true);
        }
    }
}