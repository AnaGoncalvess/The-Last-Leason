using UnityEngine;
using Cinemachine;

public class LimitesFase : MonoBehaviour
{
    [Header("Identificadores de Limite na Cena")]
    [SerializeField] private Collider2D limiteInicio;
    [SerializeField] private Collider2D limiteFim;

    private CinemachineConfiner2D confiner;

    private void Awake()
    {
        confiner = GetComponent<CinemachineConfiner2D>();
    }

    private void Start()
    {
        // Se não atribuiu manualmente no Inspector, procura pelos nomes padrão
        if (limiteInicio == null)
        {
            GameObject objInicio = GameObject.Find("Limite_Inicio");
            if (objInicio != null) limiteInicio = objInicio.GetComponent<Collider2D>();
        }

        if (limiteFim == null)
        {
            GameObject objFim = GameObject.Find("Limite_Fim");
            if (objFim != null) limiteFim = objFim.GetComponent<Collider2D>();
        }

        // Define o limite inicial da cena
        AplicarLimite(limiteInicio);
    }

    public void AplicarLimite(Collider2D novoLimite)
    {
        if (confiner != null && novoLimite != null)
        {
            confiner.m_BoundingShape2D = novoLimite;
            confiner.InvalidateCache();
        }
    }
}
