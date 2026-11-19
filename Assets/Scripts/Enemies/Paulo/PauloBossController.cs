// using System.Collections;
// using UnityEngine;

// [RequireComponent(typeof(Rigidbody2D))]
// public class PauloBossController : MonoBehaviour
// {
//     [Header("Configurações de Vida (Porcentagem)")]
//     [SerializeField] private int vidaMaximaGolpes = 16;
//     [SerializeField] private int vidaAtualGolpes;
//     [Range(0.1f, 0.9f)]
//     [SerializeField] private float porcentagemTrocaFase = 0.5f;

//     [Header("Fase 1: Ataque de Corrida (Investida)")]
//     [SerializeField] private float velocidadeCorrida = 12f;
//     [SerializeField] private float duracaoCorrida = 0.9f;
//     [SerializeField] private float cooldownCorrida = 1.8f;
//     [SerializeField] private float danoCorrida = 1f;
//     [SerializeField] private Vector2 areaDanoCorrida = new Vector2(1.8f, 3.0f);
//     [SerializeField] private LayerMask playerLayer;

//     [Header("Fase 2: Ataque de Laser Teleguiado")]
//     [SerializeField] private GameObject prefabLaser;
//     [SerializeField] private Transform pontoOlhoLaser;
//     [SerializeField] private float cooldownLaser = 2.5f;
//     [SerializeField] private float atrasoDisparoLaser = 0.35f;

//     [Header("Configurações de Drop")]
//     [SerializeField] private bool dropsCristal = true;
//     [SerializeField] private GameObject cristalPrefab;
//     [SerializeField] private Vector2 offsetSpawnCristal = new Vector2(0f, 1.0f);

//     // Propriedade lida pelo ColetaveisManager para contar +1 item na cena
//     public bool DropsCristal => dropsCristal && cristalPrefab != null;

//     [Header("Detecção e Referências")]
//     [SerializeField] private float alcanceVisao = 16f;
//     [SerializeField] private Transform playerTransform;
//     [SerializeField] private Animator animator;
//     [SerializeField] private BossHealthBar healthBar;
//     [SerializeField] private float atrasoAposMorte = 1.5f;

//     private Rigidbody2D rb;
//     private Collider2D colisorPrincipal;
//     private int faseAtual = 1;
//     private bool estaCorrendo = false;
//     private bool jaAcertouNestaCorrida = false;
//     private bool derrotado = false;
//     private float proximoAtaqueTime;
//     private float ultimoDanoRecebidoTime;
//     private float direcaoOlhar = -1f;

//     private void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         colisorPrincipal = GetComponent<Collider2D>();
//         if (animator == null) animator = GetComponent<Animator>();
//         if (animator != null) animator.applyRootMotion = false;

//         vidaAtualGolpes = vidaMaximaGolpes;

//         if (healthBar != null)
//         {
//             healthBar.AtualizarBarra(vidaAtualGolpes, vidaMaximaGolpes);
//         }

//         if (playerTransform == null)
//         {
//             Player p = FindFirstObjectByType<Player>();
//             if (p != null) playerTransform = p.transform;
//         }
//     }

//     private void Update()
//     {
//         if (derrotado || vidaAtualGolpes <= 0 || playerTransform == null) return;

//         if (estaCorrendo && !jaAcertouNestaCorrida)
//         {
//             VerificarDanoCorrida();
//         }

//         if (!estaCorrendo)
//         {
//             VirarParaPlayer();
//         }

//         float distancia = Vector2.Distance(transform.position, playerTransform.position);
//         if (distancia <= alcanceVisao && Time.time >= proximoAtaqueTime && !estaCorrendo)
//         {
//             if (faseAtual == 1)
//             {
//                 StartCoroutine(ExecutarCorrida());
//             }
//             else if (faseAtual == 2)
//             {
//                 StartCoroutine(ExecutarAtaqueLaser());
//             }
//         }
//     }

//     private void VerificarDanoCorrida()
//     {
//         Vector2 centro = colisorPrincipal != null ? (Vector2)colisorPrincipal.bounds.center : (Vector2)transform.position;
//         Collider2D atingido = Physics2D.OverlapBox(centro, areaDanoCorrida, 0f, playerLayer);

//         if (atingido != null)
//         {
//             Player scriptPlayer = atingido.GetComponent<Player>();
//             if (scriptPlayer == null) scriptPlayer = atingido.GetComponentInParent<Player>();

//             if (scriptPlayer != null)
//             {
//                 scriptPlayer.TomarDano(danoCorrida);
//                 jaAcertouNestaCorrida = true;
//             }
//         }
//     }

//     private void VirarParaPlayer()
//     {
//         if (playerTransform.position.x < transform.position.x)
//         {
//             direcaoOlhar = -1f;
//             transform.eulerAngles = new Vector3(0f, 0f, 0f);
//         }
//         else
//         {
//             direcaoOlhar = 1f;
//             transform.eulerAngles = new Vector3(0f, 180f, 0f);
//         }
//     }

//     private IEnumerator ExecutarCorrida()
//     {
//         estaCorrendo = true;
//         jaAcertouNestaCorrida = false;

//         VirarParaPlayer();
//         float direcaoInvestida = direcaoOlhar;

//         if (animator != null) animator.SetBool("Correndo", true);

//         float tempoFim = Time.time + duracaoCorrida;
//         while (Time.time < tempoFim && !derrotado && faseAtual == 1)
//         {
//             rb.linearVelocity = new Vector2(direcaoInvestida * velocidadeCorrida, rb.linearVelocity.y);
//             yield return null;
//         }

//         rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
//         if (animator != null) animator.SetBool("Correndo", false);

//         estaCorrendo = false;
//         proximoAtaqueTime = Time.time + cooldownCorrida;
//     }

//     private IEnumerator ExecutarAtaqueLaser()
//     {
//         proximoAtaqueTime = Time.time + cooldownLaser;

//         if (animator != null)
//         {
//             animator.ResetTrigger("AtacarLaser");
//             animator.SetTrigger("AtacarLaser");
//         }

//         yield return new WaitForSeconds(atrasoDisparoLaser);

//         if (!derrotado && vidaAtualGolpes > 0)
//         {
//             DispararLaser();
//         }
//     }

//     public void DispararLaser()
//     {
//         if (derrotado || prefabLaser == null || pontoOlhoLaser == null) return;
//         Instantiate(prefabLaser, pontoOlhoLaser.position, Quaternion.identity);
//     }

//     public void TomarDano(int quantidade = 1)
//     {
//         if (derrotado || vidaAtualGolpes <= 0) return;

//         if (Time.time < ultimoDanoRecebidoTime + 0.15f) return;
//         ultimoDanoRecebidoTime = Time.time;

//         vidaAtualGolpes -= quantidade;

//         if (healthBar != null)
//         {
//             healthBar.AtualizarBarra(vidaAtualGolpes, vidaMaximaGolpes);
//         }

//         if (animator != null)
//         {
//             animator.ResetTrigger("Hit");
//             animator.SetTrigger("Hit");
//         }

//         int golpesSofridos = vidaMaximaGolpes - vidaAtualGolpes;
//         int limiteTrocaFase = Mathf.RoundToInt(vidaMaximaGolpes * porcentagemTrocaFase);

//         if (faseAtual == 1 && golpesSofridos >= limiteTrocaFase)
//         {
//             faseAtual = 2;
//             estaCorrendo = false;
//             rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
//             if (animator != null) animator.SetBool("Correndo", false);
//             proximoAtaqueTime = Time.time + 0.8f;
//         }

//         if (vidaAtualGolpes <= 0)
//         {
//             Morrer();
//         }
//     }

//     private void Morrer()
//     {
//         if (derrotado) return;
//         derrotado = true;
//         estaCorrendo = false;
//         rb.linearVelocity = Vector2.zero;

//         // Dropa o cristal ao morrer
//         if (dropsCristal && cristalPrefab != null)
//         {
//             Vector3 posSpawn = (Vector2)transform.position + offsetSpawnCristal;
//             Instantiate(cristalPrefab, posSpawn, Quaternion.identity);
//         }

//         foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
//         {
//             col.enabled = false;
//         }
//         rb.simulated = false;

//         if (animator != null)
//         {
//             animator.SetBool("Correndo", false);
//             animator.SetTrigger("Morrer");
//         }

//         StartCoroutine(FinalizarMorte());
//     }

//     private IEnumerator FinalizarMorte()
//     {
//         yield return null;
//         float duracaoMorte = 1.5f;

//         if (animator != null)
//         {
//             while (animator.IsInTransition(0)) yield return null;
//             duracaoMorte = animator.GetCurrentAnimatorStateInfo(0).length;
//         }

//         yield return new WaitForSeconds(duracaoMorte + atrasoAposMorte);

//         Destroy(gameObject);
//     }

//     private void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.red;
//         Vector2 centro = GetComponent<Collider2D>() != null ? (Vector2)GetComponent<Collider2D>().bounds.center : (Vector2)transform.position;
//         Gizmos.DrawWireCube(centro, areaDanoCorrida);
//     }
// }

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PauloBossController : MonoBehaviour
{
    [Header("Configurações de Vida (Porcentagem)")]
    [SerializeField] private int vidaMaximaGolpes = 16;
    [SerializeField] private int vidaAtualGolpes;
    [Range(0.1f, 0.9f)]
    [SerializeField] private float porcentagemTrocaFase = 0.5f;

    [Header("Fase 1: Ataque de Corrida (Investida)")]
    [SerializeField] private float velocidadeCorrida = 12f;
    [SerializeField] private float duracaoCorrida = 0.9f;
    [SerializeField] private float cooldownCorrida = 1.8f;
    [SerializeField] private float danoCorrida = 1f;
    [SerializeField] private Vector2 areaDanoCorrida = new Vector2(1.8f, 3.0f);
    [SerializeField] private LayerMask playerLayer;

    [Header("Fase 2: Ataque de Laser (Integrado na Animação)")]
    [SerializeField] private GameObject hitboxLaser; // Arraste o objeto filho HitboxLaser aqui
    [SerializeField] private float cooldownLaser = 2.5f;
    [SerializeField] private float duracaoAnimacaoLaser = 1.2f;

    [Header("Configurações de Drop")]
    [SerializeField] private bool dropsCristal = true;
    [SerializeField] private GameObject cristalPrefab;
    [SerializeField] private Vector2 offsetSpawnCristal = new Vector2(0f, 1.0f);

    // Propriedade lida pelo ColetaveisManager para contar +1 item na cena
    public bool DropsCristal => dropsCristal && cristalPrefab != null;

    [Header("Detecção e Referências")]
    [SerializeField] private float alcanceVisao = 16f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private BossHealthBar healthBar;
    [SerializeField] private float atrasoAposMorte = 1.5f;

    private Rigidbody2D rb;
    private Collider2D colisorPrincipal;
    private int faseAtual = 1;
    private bool estaCorrendo = false;
    private bool estaAtacandoLaser = false;
    private bool jaAcertouNestaCorrida = false;
    private bool derrotado = false;
    private float proximoAtaqueTime;
    private float ultimoDanoRecebidoTime;
    private float direcaoOlhar = -1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colisorPrincipal = GetComponent<Collider2D>();
        if (animator == null) animator = GetComponent<Animator>();
        if (animator != null) animator.applyRootMotion = false;

        // Garante que o colisor do laser comece desligado
        if (hitboxLaser != null) hitboxLaser.SetActive(false);

        vidaAtualGolpes = vidaMaximaGolpes;

        if (healthBar != null)
        {
            healthBar.AtualizarBarra(vidaAtualGolpes, vidaMaximaGolpes);
        }

        if (playerTransform == null)
        {
            Player p = FindFirstObjectByType<Player>();
            if (p != null) playerTransform = p.transform;
        }
    }

    private void Update()
    {
        if (derrotado || vidaAtualGolpes <= 0 || playerTransform == null) return;

        if (estaCorrendo && !jaAcertouNestaCorrida)
        {
            VerificarDanoCorrida();
        }

        // Só vira para o Player quando não estiver correndo nem no meio do disparo do laser
        if (!estaCorrendo && !estaAtacandoLaser)
        {
            VirarParaPlayer();
        }

        float distancia = Vector2.Distance(transform.position, playerTransform.position);
        if (distancia <= alcanceVisao && Time.time >= proximoAtaqueTime && !estaCorrendo && !estaAtacandoLaser)
        {
            if (faseAtual == 1)
            {
                StartCoroutine(ExecutarCorrida());
            }
            else if (faseAtual == 2)
            {
                StartCoroutine(ExecutarAtaqueLaser());
            }
        }
    }

    private void VerificarDanoCorrida()
    {
        Vector2 centro = colisorPrincipal != null ? (Vector2)colisorPrincipal.bounds.center : (Vector2)transform.position;
        Collider2D atingido = Physics2D.OverlapBox(centro, areaDanoCorrida, 0f, playerLayer);

        if (atingido != null)
        {
            Player scriptPlayer = atingido.GetComponent<Player>();
            if (scriptPlayer == null) scriptPlayer = atingido.GetComponentInParent<Player>();

            if (scriptPlayer != null)
            {
                scriptPlayer.TomarDano(danoCorrida);
                jaAcertouNestaCorrida = true;
            }
        }
    }

    private void VirarParaPlayer()
    {
        if (playerTransform.position.x < transform.position.x)
        {
            direcaoOlhar = -1f;
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else
        {
            direcaoOlhar = 1f;
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private IEnumerator ExecutarCorrida()
    {
        estaCorrendo = true;
        jaAcertouNestaCorrida = false;

        VirarParaPlayer();
        float direcaoInvestida = direcaoOlhar;

        if (animator != null) animator.SetBool("Correndo", true);

        float tempoFim = Time.time + duracaoCorrida;
        while (Time.time < tempoFim && !derrotado && faseAtual == 1)
        {
            rb.linearVelocity = new Vector2(direcaoInvestida * velocidadeCorrida, rb.linearVelocity.y);
            yield return null;
        }

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        if (animator != null) animator.SetBool("Correndo", false);

        estaCorrendo = false;
        proximoAtaqueTime = Time.time + cooldownCorrida;
    }

    private IEnumerator ExecutarAtaqueLaser()
    {
        estaAtacandoLaser = true;
        proximoAtaqueTime = Time.time + cooldownLaser;

        // Vira para o lado onde o Player está no início do ataque
        VirarParaPlayer();

        if (animator != null)
        {
            animator.ResetTrigger("AtacarLaser");
            animator.SetTrigger("AtacarLaser");
        }

        yield return new WaitForSeconds(duracaoAnimacaoLaser);

        // Garante que o colisor seja desligado ao fim do ataque
        DesativarHitboxLaser();
        estaAtacandoLaser = false;
    }

    // --- MÉTODOS PARA CHAMAR NO ANIMATION EVENT DA ANIMAÇÃO PauloLaser ---
    public void AtivarHitboxLaser()
    {
        if (!derrotado && hitboxLaser != null)
        {
            hitboxLaser.SetActive(true);
        }
    }

    public void DesativarHitboxLaser()
    {
        if (hitboxLaser != null)
        {
            hitboxLaser.SetActive(false);
        }
    }
    // ---------------------------------------------------------------------

    public void TomarDano(int quantidade = 1)
    {
        if (derrotado || vidaAtualGolpes <= 0) return;

        if (Time.time < ultimoDanoRecebidoTime + 0.15f) return;
        ultimoDanoRecebidoTime = Time.time;

        vidaAtualGolpes -= quantidade;

        if (healthBar != null)
        {
            healthBar.AtualizarBarra(vidaAtualGolpes, vidaMaximaGolpes);
        }

        if (animator != null)
        {
            animator.ResetTrigger("Hit");
            animator.SetTrigger("Hit");
        }

        int golpesSofridos = vidaMaximaGolpes - vidaAtualGolpes;
        int limiteTrocaFase = Mathf.RoundToInt(vidaMaximaGolpes * porcentagemTrocaFase);

        if (faseAtual == 1 && golpesSofridos >= limiteTrocaFase)
        {
            faseAtual = 2;
            estaCorrendo = false;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            if (animator != null) animator.SetBool("Correndo", false);
            proximoAtaqueTime = Time.time + 0.8f;
        }

        if (vidaAtualGolpes <= 0)
        {
            Morrer();
        }
    }

    private void Morrer()
    {
        if (derrotado) return;
        derrotado = true;
        estaCorrendo = false;
        estaAtacandoLaser = false;
        rb.linearVelocity = Vector2.zero;

        // Desliga o laser imediatamente se morrer durante o ataque
        DesativarHitboxLaser();

        // Dropa o cristal ao morrer
        if (dropsCristal && cristalPrefab != null)
        {
            Vector3 posSpawn = (Vector2)transform.position + offsetSpawnCristal;
            Instantiate(cristalPrefab, posSpawn, Quaternion.identity);
        }

        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }
        rb.simulated = false;

        if (animator != null)
        {
            animator.SetBool("Correndo", false);
            animator.SetTrigger("Morrer");
        }

        StartCoroutine(FinalizarMorte());
    }

    private IEnumerator FinalizarMorte()
    {
        yield return null;
        float duracaoMorte = 1.5f;

        if (animator != null)
        {
            while (animator.IsInTransition(0)) yield return null;
            duracaoMorte = animator.GetCurrentAnimatorStateInfo(0).length;
        }

        yield return new WaitForSeconds(duracaoMorte + atrasoAposMorte);

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 centro = GetComponent<Collider2D>() != null ? (Vector2)GetComponent<Collider2D>().bounds.center : (Vector2)transform.position;
        Gizmos.DrawWireCube(centro, areaDanoCorrida);
    }
}