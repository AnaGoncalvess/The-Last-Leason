using UnityEngine;

public class BossController : MonoBehaviour
{
    public enum FaseBoss { Fase1_DuasCabecas, Fase2_UmaCabeca, Fase3_Rabada }

    [Header("Configurações de Vida")]
    [SerializeField] private float vidaMaxima = 300f;
    private float vidaAtual;

    [Header("UI")]
    [SerializeField] private BossHealthBar healthBar;

    [Header("Componentes")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform pontoDisparo;
    [SerializeField] private GameObject venenoPrefab;

    public FaseBoss FaseAtual { get; private set; } = FaseBoss.Fase1_DuasCabecas;

    private void Start()
    {
        vidaAtual = vidaMaxima;
        if (healthBar != null) healthBar.AtualizarBarra(vidaAtual, vidaMaxima);
    }

    public void TomarDano(float quantidade)
    {
        vidaAtual -= quantidade;
        if (vidaAtual < 0) vidaAtual = 0;

        if (healthBar != null) healthBar.AtualizarBarra(vidaAtual, vidaMaxima);

        VerificarMudancaDeFase();

        if (vidaAtual <= 0)
        {
            DerrotarBoss();
        }
    }

    private void VerificarMudancaDeFase()
    {
        float percentualVida = vidaAtual / vidaMaxima;

        if (percentualVida <= 0.33f && FaseAtual != FaseBoss.Fase3_Rabada)
        {
            FaseAtual = FaseBoss.Fase3_Rabada;
            animator.SetTrigger("MudarFaseRabada"); // Ativa animação do sprite rabada-spriteSheet
        }
        else if (percentualVida <= 0.66f && percentualVida > 0.33f && FaseAtual != FaseBoss.Fase2_UmaCabeca)
        {
            FaseAtual = FaseBoss.Fase2_UmaCabeca;
            animator.SetTrigger("MudarFaseUmaCabeca"); // Ativa animação do sprite Cuspida sem 1 cabeça
        }
    }

    // Chamado via Animation Event ou Co-rotina de Ataque
    public void ExecutarAtaque()
    {
        switch (FaseAtual)
        {
            case FaseBoss.Fase1_DuasCabecas:
                animator.SetTrigger("AtacarCuspidaDupla");
                break;

            case FaseBoss.Fase2_UmaCabeca:
                animator.SetTrigger("AtacarCuspidaSimples");
                break;

            case FaseBoss.Fase3_Rabada:
                animator.SetTrigger("AtacarRabada");
                break;
        }
    }

    // Evento disparado no frame exato da cuspida na animação
    public void DispararProjetilVeneno()
    {
        if (venenoPrefab != null && pontoDisparo != null)
        {
            Instantiate(venenoPrefab, pontoDisparo.position, pontoDisparo.rotation);
        }
    }

    private void DerrotarBoss()
    {
        animator.SetTrigger("Morrer");
        this.enabled = false;
    }
}