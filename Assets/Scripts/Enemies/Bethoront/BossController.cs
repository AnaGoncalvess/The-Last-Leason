using UnityEngine;

public class BossController : MonoBehaviour
{
    public enum FaseBoss { Fase1_DuasCabecas, Fase2_UmaCabeca, Fase3_Rabada }

    [Header("Configurações de Vida")]
    [SerializeField] private float vidaMaxima = 300f;
    private float vidaAtual;

    [Header("Dano do Chefe")]
    [SerializeField] private float danoAtaque = 20f;
    [SerializeField] private Collider2D colliderAtaque; // O Collider (Trigger) da área do ataque

    [Header("UI e Componentes")]
    [SerializeField] private BossHealthBar healthBar;
    [SerializeField] private Animator animator;

    public FaseBoss FaseAtual { get; private set; } = FaseBoss.Fase1_DuasCabecas;

    private void Start()
    {
        vidaAtual = vidaMaxima;
        if (healthBar != null) healthBar.AtualizarBarra(vidaAtual, vidaMaxima);

        // Garante que o collider de dano comece desligado
        if (colliderAtaque != null) colliderAtaque.enabled = false;
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

        // Fase 3: Rabada (Abaixo de 33% de vida)
        if (percentualVida <= 0.33f && FaseAtual != FaseBoss.Fase3_Rabada)
        {
            FaseAtual = FaseBoss.Fase3_Rabada;
            animator.SetTrigger("MudarFaseRabada");
        }
        // Fase 2: Cuspida sem 1 cabeça (Entre 33% e 66% de vida)
        else if (percentualVida <= 0.66f && percentualVida > 0.33f && FaseAtual != FaseBoss.Fase2_UmaCabeca)
        {
            FaseAtual = FaseBoss.Fase2_UmaCabeca;
            animator.SetTrigger("MudarFaseUmaCabeca");
        }
    }

    // --- MÉTODOS CHAMADOS PELOS EVENTOS DE ANIMAÇÃO ---
    public void AtivarColliderAtaque()
    {
        if (colliderAtaque != null) colliderAtaque.enabled = true;
    }

    public void DesativarColliderAtaque()
    {
        if (colliderAtaque != null) colliderAtaque.enabled = false;
    }

    // Detecta se o Player encostou na área do ataque (cuspida ou rabada)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Se o collider de ataque estiver ativo e colidir com o Player
        if (collision.CompareTag("Player") || collision.GetComponentInParent<Player>() != null)
        {
            Player scriptPlayer = collision.GetComponentInParent<Player>();
            if (scriptPlayer != null)
            {
                scriptPlayer.TomarDano(danoAtaque);
            }
        }
    }

    private void DerrotarBoss()
    {
        animator.SetTrigger("Morrer");
        this.enabled = false;
    }
}