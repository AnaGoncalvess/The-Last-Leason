using UnityEngine;

public class GatilhoMudancaLimite : MonoBehaviour
{
    public enum TipoLimite { IrParaFim, IrParaInicio }

    [Header("Configuração Automática")]
    [Tooltip("Escolha se este gatilho ativa o Limite_Fim ou o Limite_Inicio")]
    [SerializeField] private TipoLimite acaoGatilho = TipoLimite.IrParaFim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se é o jogador que pisou no gatilho
        if (collision.CompareTag("Player") || collision.GetComponentInParent<Player>() != null)
        {
            LimitesFase gerenciador = FindFirstObjectByType<LimitesFase>();
            if (gerenciador == null) return;

            if (acaoGatilho == TipoLimite.IrParaFim)
            {
                GameObject objFim = GameObject.Find("Limite_Fim");
                if (objFim != null)
                {
                    gerenciador.AplicarLimite(objFim.GetComponent<Collider2D>());
                }
            }
            else if (acaoGatilho == TipoLimite.IrParaInicio)
            {
                GameObject objInicio = GameObject.Find("Limite_Inicio");
                if (objInicio != null)
                {
                    gerenciador.AplicarLimite(objInicio.GetComponent<Collider2D>());
                }
            }
        }
    }
}
