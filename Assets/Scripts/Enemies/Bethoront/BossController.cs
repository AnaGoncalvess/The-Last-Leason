// using UnityEngine;
// using System.Collections;

// public class BossController : MonoBehaviour
// {
//     [Header("Configurações de Vida do Boss (Golpes)")]
//     [SerializeField] private int vidaMaximaGolpes = 100;
//     [SerializeField] private int vidaAtualGolpes;

//     [Header("Dano e Alcances de Ataque")]
//     [SerializeField] private float danoAtaque = 25f;
//     [SerializeField] private float alcanceProximidade = 14f;
//     [SerializeField] private float alcanceRabadaCurta = 3.0f;
//     [SerializeField] private float tempoEntreAtaques = 3.0f;
//     private float proximoTempoAtaque;

//     [Header("Área de Impacto dos Ataques (Cuspe e Rabada)")]
//     [SerializeField] private Transform pontoImpactoAtaque; // Arraste o PontoAtaque aqui
//     [SerializeField] private Vector2 tamanhoCaixaAtaque = new Vector2(3.5f, 2.5f);
//     [SerializeField] private LayerMask playerLayer;

//     [Header("UI e Referências")]
//     [SerializeField] private BossHealthBar healthBar;
//     [SerializeField] private Animator animator;
//     [SerializeField] private GameObject telaVitoria;
//     [SerializeField] private Transform playerTransform;

//     [Header("Configuração da Vitória")]
//     [SerializeField] private float atrasoAposMorte = 2.0f;
//     private bool bossDerrotado = false;

//     [Header("Colisor de Ataque Físico (Opcional)")]
//     [SerializeField] private Collider2D colliderAtaque;

//     [Header("Sons (opcionais)")]
//     [SerializeField] private AudioClip cuspidaSfx;
//     [SerializeField] private AudioClip rabadaSfx;
//     [SerializeField] private AudioClip mudarFaseSfx;
//     [SerializeField] private AudioClip hitSfx;
//     [SerializeField] private AudioClip deathSfx;

//     private int faseAtual = 1;

//     private void Start()
//     {
//         vidaAtualGolpes = vidaMaximaGolpes;

//         if (healthBar != null)
//         {
//             healthBar.AtualizarBarra(vidaAtualGolpes, vidaMaximaGolpes);
//         }

//         if (animator == null) animator = GetComponent<Animator>();

//         if (playerTransform == null)
//         {
//             Player p = FindFirstObjectByType<Player>();
//             if (p != null) playerTransform = p.transform;
//         }

//         if (colliderAtaque != null) colliderAtaque.enabled = false;

//         if (animator != null) animator.SetInteger("FaseAtual", faseAtual);
//     }

//     private void Update()
//     {
//         if (bossDerrotado || vidaAtualGolpes <= 0 || playerTransform == null) return;

//         Vector2 posicaoBoss = transform.position;
//         Collider2D col = GetComponent<Collider2D>();
//         if (col != null) posicaoBoss = col.bounds.center;

//         float distanciaAtePlayer = Vector2.Distance(posicaoBoss, playerTransform.position);

//         // if (distanciaAtePlayer <= alcanceProximidade && Time.time >= proximoTempoAtaque)
//         // {
//         //     AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
//         //     if (!stateInfo.IsName("hit") && !stateInfo.IsName("hit2") && !stateInfo.IsName("hit3"))
//         //     {
//         //         IniciarAtaque(distanciaAtePlayer);
//         //         proximoTempoAtaque = Time.time + tempoEntreAtaques;
//         //     }
//         // }

//         if (distanciaAtePlayer <= alcanceProximidade && Time.time >= proximoTempoAtaque)
//         {
//             if (animator.IsInTransition(0)) return;

//             AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
//             bool estaOcupado = stateInfo.IsName("hit") || stateInfo.IsName("hit2") || stateInfo.IsName("hit3") ||
//                                stateInfo.IsName("cuspida") || stateInfo.IsName("cuspidaUmaCabeca") || stateInfo.IsName("rabada");

//             if (!estaOcupado)
//             {
//                 IniciarAtaque(distanciaAtePlayer);
//                 proximoTempoAtaque = Time.time + tempoEntreAtaques;
//             }
//         }
//     }

//     // private void IniciarAtaque(float distanciaAtePlayer)
//     // {
//     //     if (animator == null || bossDerrotado) return;

//     //     ResetarTriggersAtaque();

//     //     if (faseAtual == 3 || distanciaAtePlayer <= alcanceRabadaCurta)
//     //     {
//     //         animator.SetTrigger("AtacarRabada");
//     //         PlaySfx(rabadaSfx);
//     //     }
//     //     else if (faseAtual == 1)
//     //     {
//     //         animator.SetTrigger("AtacarCuspidaDupla");
//     //         PlaySfx(cuspidaSfx);
//     //     }
//     //     else if (faseAtual == 2)
//     //     {
//     //         animator.SetTrigger("AtacarCuspidaSimples");
//     //         PlaySfx(cuspidaSfx);
//     //     }
//     // }

//     private void IniciarAtaque(float distanciaAtePlayer)
//     {
//         if (animator == null || bossDerrotado) return;

//         ResetarTriggersAtaque();

//         if (faseAtual == 3 || distanciaAtePlayer <= alcanceRabadaCurta)
//         {
//             PlaySfx(rabadaSfx);
//             // Toca a animação da rabada diretamente sem deixar Trigger preso na fila
//             animator.Play("rabada", 0, 0f);
//         }
//         else if (faseAtual == 1)
//         {
//             animator.SetTrigger("AtacarCuspidaDupla");
//             PlaySfx(cuspidaSfx);
//         }
//         else if (faseAtual == 2)
//         {
//             animator.SetTrigger("AtacarCuspidaSimples");
//             PlaySfx(cuspidaSfx);
//         }
//     }

//     private void ResetarTriggersAtaque()
//     {
//         if (animator == null) return;
//         animator.ResetTrigger("AtacarCuspidaDupla");
//         animator.ResetTrigger("AtacarCuspidaSimples");
//         animator.ResetTrigger("AtacarRabada");
//     }

//     // Chamado pelo Animation Event no frame de impacto (tanto do Cuspe quanto da Rabada)
//     public void ExecutarDanoNoPlayer()
//     {
//         if (bossDerrotado) return;

//         Vector2 pontoOriginal = pontoImpactoAtaque != null ? (Vector2)pontoImpactoAtaque.position : (Vector2)transform.position;
//         Collider2D playerAtingido = Physics2D.OverlapBox(pontoOriginal, tamanhoCaixaAtaque, 0f, playerLayer);

//         if (playerAtingido != null)
//         {
//             Player scriptPlayer = playerAtingido.GetComponent<Player>();
//             if (scriptPlayer == null) scriptPlayer = playerAtingido.GetComponentInParent<Player>();

//             if (scriptPlayer != null)
//             {
//                 scriptPlayer.TomarDano(danoAtaque);
//             }
//         }
//     }

//     public void TomarDano(int quantidade = 1)
//     {
//         if (bossDerrotado || vidaAtualGolpes <= 0) return;

//         vidaAtualGolpes -= quantidade;
//         if (vidaAtualGolpes < 0) vidaAtualGolpes = 0;

//         if (healthBar != null)
//         {
//             healthBar.AtualizarBarra(vidaAtualGolpes, vidaMaximaGolpes);
//         }

//         if (vidaAtualGolpes <= 0)
//         {
//             DerrotarBoss();
//             return;
//         }

//         PlaySfx(hitSfx);
//         ResetarTriggersAtaque();

//         if (animator != null)
//         {
//             animator.SetTrigger("Hit");
//         }

//         VerificarMudancaDeFase();
//     }

//     private void VerificarMudancaDeFase()
//     {
//         int ataquesSofridos = vidaMaximaGolpes - vidaAtualGolpes;

//         if (faseAtual == 1 && ataquesSofridos >= Mathf.RoundToInt(vidaMaximaGolpes * 0.25f))
//         {
//             faseAtual = 2;
//             ResetarTriggersAtaque();
//             PlaySfx(mudarFaseSfx);
//             if (animator != null) animator.SetTrigger("MudarFaseUmaCabeca");
//         }
//         else if (faseAtual == 2 && ataquesSofridos >= Mathf.RoundToInt(vidaMaximaGolpes * 0.50f))
//         {
//             faseAtual = 3;
//             ResetarTriggersAtaque();
//             PlaySfx(mudarFaseSfx);
//             if (animator != null) animator.SetTrigger("MudarFaseRabada");
//         }

//         if (animator != null) animator.SetInteger("FaseAtual", faseAtual);
//     }

//     private void DerrotarBoss()
//     {
//         if (bossDerrotado) return;
//         bossDerrotado = true;

//         PlaySfx(deathSfx);
//         ResetarTriggersAtaque();

//         if (colliderAtaque != null) colliderAtaque.enabled = false;
//         foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
//         {
//             col.enabled = false;
//         }

//         if (animator != null)
//         {
//             animator.ResetTrigger("Hit");
//             animator.ResetTrigger("MudarFaseUmaCabeca");
//             animator.ResetTrigger("MudarFaseRabada");
//             animator.SetTrigger("Morrer");
//         }

//         StartCoroutine(SequenciaVitoria());
//     }

//     private IEnumerator SequenciaVitoria()
//     {
//         yield return null;

//         float duracaoAnimacaoMorte = 1.0f;

//         if (animator != null)
//         {
//             while (animator.IsInTransition(0))
//             {
//                 yield return null;
//             }

//             AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
//             duracaoAnimacaoMorte = stateInfo.length;
//         }

//         yield return new WaitForSeconds(duracaoAnimacaoMorte + atrasoAposMorte);

//         if (playerTransform != null)
//         {
//             playerTransform.gameObject.SetActive(false);
//         }

//         if (telaVitoria != null)
//         {
//             telaVitoria.SetActive(true);
//         }
//     }

//     private void PlaySfx(AudioClip clip)
//     {
//         if (clip == null || AudioManager.Instance == null) return;
//         AudioManager.Instance.PlaySfx(clip);
//     }

//     public void AtivarColliderAtaque()
//     {
//         if (colliderAtaque != null) colliderAtaque.enabled = true;
//     }

//     public void DesativarColliderAtaque()
//     {
//         if (colliderAtaque != null) colliderAtaque.enabled = false;
//     }

//     private void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.cyan;
//         Gizmos.DrawWireSphere(transform.position, alcanceProximidade);

//         Gizmos.color = Color.yellow;
//         Gizmos.DrawWireSphere(transform.position, alcanceRabadaCurta);

//         Gizmos.color = Color.green;
//         Vector2 pontoOriginal = pontoImpactoAtaque != null ? (Vector2)pontoImpactoAtaque.position : (Vector2)transform.position;
//         Gizmos.DrawWireCube(pontoOriginal, tamanhoCaixaAtaque);
//     }
// }

using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("Configurações de Vida do Boss (Golpes)")]
    [SerializeField] private int vidaMaximaGolpes = 100;
    [SerializeField] private int vidaAtualGolpes;

    [Header("Dano e Alcances de Ataque")]
    [SerializeField] private float danoAtaque = 25f;
    [SerializeField] private float alcanceProximidade = 14f;
    [SerializeField] private float alcanceRabadaCurta = 3.0f;
    [SerializeField] private float tempoEntreAtaques = 3.0f;
    private float proximoTempoAtaque;

    [Header("Área de Impacto dos Ataques (Cuspe e Rabada)")]
    [SerializeField] private Transform pontoImpactoAtaque;
    [SerializeField] private Vector2 tamanhoCaixaAtaque = new Vector2(3.5f, 2.5f);
    [SerializeField] private LayerMask playerLayer;

    [Header("UI e Referências")]
    [SerializeField] private BossHealthBar healthBar;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject telaVitoria;
    [SerializeField] private Transform playerTransform;

    [Header("Configuração da Vitória")]
    [SerializeField] private float atrasoAposMorte = 2.0f;
    private bool bossDerrotado = false;

    [Header("Colisor de Ataque Físico (Opcional)")]
    [SerializeField] private Collider2D colliderAtaque;

    [Header("Sons (opcionais)")]
    [SerializeField] private AudioClip cuspidaSfx;
    [SerializeField] private AudioClip rabadaSfx;
    [SerializeField] private AudioClip mudarFaseSfx;
    [SerializeField] private AudioClip hitSfx;
    [SerializeField] private AudioClip deathSfx;

    private int faseAtual = 1;

    private void Start()
    {
        vidaAtualGolpes = vidaMaximaGolpes;

        if (healthBar != null)
        {
            healthBar.AtualizarBarra(vidaAtualGolpes, vidaMaximaGolpes);
        }

        if (animator == null) animator = GetComponent<Animator>();

        if (playerTransform == null)
        {
            Player p = FindFirstObjectByType<Player>();
            if (p != null) playerTransform = p.transform;
        }

        if (colliderAtaque != null) colliderAtaque.enabled = false;

        if (animator != null) animator.SetInteger("FaseAtual", faseAtual);
    }

    private void Update()
    {
        if (bossDerrotado || vidaAtualGolpes <= 0 || playerTransform == null || animator == null) return;

        AnimatorStateInfo estadoAtual = animator.GetCurrentAnimatorStateInfo(0);

        if (estadoAtual.IsName("rabada"))
        {
            animator.ResetTrigger("AtacarRabada");
        }

        Vector2 posicaoBoss = transform.position;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) posicaoBoss = col.bounds.center;

        float distanciaAtePlayer = Vector2.Distance(posicaoBoss, playerTransform.position);

        if (distanciaAtePlayer <= alcanceProximidade && Time.time >= proximoTempoAtaque)
        {
            if (animator.IsInTransition(0)) return;

            bool estaOcupado = estadoAtual.IsName("hit") || estadoAtual.IsName("hit2") || estadoAtual.IsName("hit3") ||
                               estadoAtual.IsName("cuspida") || estadoAtual.IsName("cuspidaUmaCabeca") || estadoAtual.IsName("rabada");

            if (!estaOcupado)
            {
                IniciarAtaque(distanciaAtePlayer);
                proximoTempoAtaque = Time.time + tempoEntreAtaques;
            }
        }
    }

    private void IniciarAtaque(float distanciaAtePlayer)
    {
        if (animator == null || bossDerrotado) return;

        ResetarTriggersAtaque();

        if (faseAtual == 3 || distanciaAtePlayer <= alcanceRabadaCurta)
        {
            animator.SetTrigger("AtacarRabada");
            PlaySfx(rabadaSfx);
        }
        else if (faseAtual == 1)
        {
            animator.SetTrigger("AtacarCuspidaDupla");
            PlaySfx(cuspidaSfx);
        }
        else if (faseAtual == 2)
        {
            animator.SetTrigger("AtacarCuspidaSimples");
            PlaySfx(cuspidaSfx);
        }
    }

    private void ResetarTriggersAtaque()
    {
        if (animator == null) return;
        animator.ResetTrigger("AtacarCuspidaDupla");
        animator.ResetTrigger("AtacarCuspidaSimples");
        animator.ResetTrigger("AtacarRabada");
    }

    public void ExecutarDanoNoPlayer()
    {
        if (bossDerrotado) return;

        Vector2 pontoOriginal = pontoImpactoAtaque != null ? (Vector2)pontoImpactoAtaque.position : (Vector2)transform.position;
        Collider2D playerAtingido = Physics2D.OverlapBox(pontoOriginal, tamanhoCaixaAtaque, 0f, playerLayer);

        if (playerAtingido != null)
        {
            Player scriptPlayer = playerAtingido.GetComponent<Player>();
            if (scriptPlayer == null) scriptPlayer = playerAtingido.GetComponentInParent<Player>();

            if (scriptPlayer != null)
            {
                scriptPlayer.TomarDano(danoAtaque);
            }
        }

        Debug.Log("EVENTO DE ATAQUE CHAMADO! Acertou Player? " + (playerAtingido != null));
    }

    public void TomarDano(int quantidade = 1)
    {
        if (bossDerrotado || vidaAtualGolpes <= 0) return;

        vidaAtualGolpes -= quantidade;
        if (vidaAtualGolpes < 0) vidaAtualGolpes = 0;

        if (healthBar != null)
        {
            healthBar.AtualizarBarra(vidaAtualGolpes, vidaMaximaGolpes);
        }

        if (vidaAtualGolpes <= 0)
        {
            DerrotarBoss();
            return;
        }

        PlaySfx(hitSfx);
        ResetarTriggersAtaque();

        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        VerificarMudancaDeFase();
    }

    private void VerificarMudancaDeFase()
    {
        int ataquesSofridos = vidaMaximaGolpes - vidaAtualGolpes;

        if (faseAtual == 1 && ataquesSofridos >= Mathf.RoundToInt(vidaMaximaGolpes * 0.25f))
        {
            faseAtual = 2;
            ResetarTriggersAtaque();
            PlaySfx(mudarFaseSfx);
            if (animator != null) animator.SetTrigger("MudarFaseUmaCabeca");
        }
        else if (faseAtual == 2 && ataquesSofridos >= Mathf.RoundToInt(vidaMaximaGolpes * 0.50f))
        {
            faseAtual = 3;
            ResetarTriggersAtaque();
            PlaySfx(mudarFaseSfx);
            if (animator != null) animator.SetTrigger("MudarFaseRabada");
        }

        if (animator != null) animator.SetInteger("FaseAtual", faseAtual);
    }

    private void DerrotarBoss()
    {
        if (bossDerrotado) return;
        bossDerrotado = true;

        PlaySfx(deathSfx);
        ResetarTriggersAtaque();

        if (colliderAtaque != null) colliderAtaque.enabled = false;
        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }

        if (animator != null)
        {
            animator.ResetTrigger("Hit");
            animator.ResetTrigger("MudarFaseUmaCabeca");
            animator.ResetTrigger("MudarFaseRabada");
            animator.SetTrigger("Morrer");
        }

        StartCoroutine(SequenciaVitoria());
    }

    private IEnumerator SequenciaVitoria()
    {
        yield return null;

        float duracaoAnimacaoMorte = 1.0f;

        if (animator != null)
        {
            while (animator.IsInTransition(0))
            {
                yield return null;
            }

            AnimatorStateInfo infoMorte = animator.GetCurrentAnimatorStateInfo(0);
            duracaoAnimacaoMorte = infoMorte.length;
        }

        yield return new WaitForSeconds(duracaoAnimacaoMorte + atrasoAposMorte);

        if (playerTransform != null)
        {
            playerTransform.gameObject.SetActive(false);
        }

        if (telaVitoria != null)
        {
            telaVitoria.SetActive(true);
        }
    }

    private void PlaySfx(AudioClip clip)
    {
        if (clip == null || AudioManager.Instance == null) return;
        AudioManager.Instance.PlaySfx(clip);
    }

    public void AtivarColliderAtaque()
    {
        if (colliderAtaque != null) colliderAtaque.enabled = true;
    }

    public void DesativarColliderAtaque()
    {
        if (colliderAtaque != null) colliderAtaque.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, alcanceProximidade);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, alcanceRabadaCurta);

        Gizmos.color = Color.green;
        Vector2 pontoOriginal = pontoImpactoAtaque != null ? (Vector2)pontoImpactoAtaque.position : (Vector2)transform.position;
        Gizmos.DrawWireCube(pontoOriginal, tamanhoCaixaAtaque);
    }
}