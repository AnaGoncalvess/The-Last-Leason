using UnityEngine;
using TMPro;

public class ResultadosUI : MonoBehaviour
{
    [Header("Textos dos Resultados")]
    [SerializeField] private TextMeshProUGUI txtCristaisResultado;
    [SerializeField] private TextMeshProUGUI txtLampadasResultado;

    private void OnEnable()
    {
        ExibirResultados();
    }

    public void ExibirResultados()
    {
        if (txtCristaisResultado != null)
        {
            txtCristaisResultado.text = ColetaveisManager.TotalCristaisGlobal.ToString();
        }

        if (txtLampadasResultado != null)
        {
            txtLampadasResultado.text = ColetaveisManager.TotalMoedasGlobal.ToString();
        }
    }
}