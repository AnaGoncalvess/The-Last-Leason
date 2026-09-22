using UnityEngine;

public class ChaveVoadora : MonoBehaviour
{
    [SerializeField] private float velocidade = 5f;
    private Transform alvoFechadura;
    private PortaController porta;

    public void IniciarVoo(Transform destino, PortaController portaAlvo)
    {
        alvoFechadura = destino;
        porta = portaAlvo;
    }

    private void Update()
    {
        if (alvoFechadura == null) return;

        // Move a chave gradualmente em direção à fechadura
        transform.position = Vector3.MoveTowards(transform.position, alvoFechadura.position, velocidade * Time.deltaTime);

        // Quando chega à fechadura
        if (Vector3.Distance(transform.position, alvoFechadura.position) < 0.05f)
        {
            if (porta != null)
            {
                porta.AbrirPorta(); // Inicia a animação da porta (Frame 3 em diante)
            }
            Destroy(gameObject); // Oculta/destrói a chave voadora
        }
    }
}