
// using UnityEngine;

// public class BossController : MonoBehaviour
// {
//     [Header("Configurações de Vida do Boss (Golpes)")]
//     [SerializeField] private int vidaMaximaGolpes = 100; // 25 + 25 + 50
//     [SerializeField] private int vidaAtualGolpes;

//     [Header("Dano e Raio de Proximidade")]
//     [SerializeField] private float danoAtaque = 20f;
//     [SerializeField] private float alcanceProximidade = 9f; // Aumentado para alcançar a sala toda
//     [SerializeField] private float tempoEntreAtaques = 2.5f;
//     private float proximoTempoAtaque;

//     [Header("UI e Referências")]
//     [SerializeField] private BossHealthBar healthBar;
//     [SerializeField] private Animator animator;
//     [SerializeField] private GameObject telaVitoria;
//     [SerializeField] private Transform playerTransform;

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
//     }

//     // private void Update()
//     // {
//     //     if (vidaAtualGolpes <= 0 || playerTransform == null) return;

//     //     // Calcula a distância apenas no eixo X para cobrir a sala horizontalmente
//     //     float distanciaX = Mathf.Abs(transform.position.x - playerTransform.position.x);

//     //     // Se o Player estiver ao alcance e o tempo de recarga passou
//     //     if (distanciaX <= alcanceProximidade && Time.time >= proximoTempoAtaque)
//     //     {
//     //         IniciarAtaque();
//     //         proximoTempoAtaque = Time.time + tempoEntreAtaques;
//     //     }
//     // }

//     private void Update()
//     {
//         if (vidaAtualGolpes <= 0 || playerTransform == null) return;

//         // Pega a posição central do collider do boss ou do próprio transform
//         Vector2 posicaoBoss = transform.position;
//         Collider2D col = GetComponent<Collider2D>();
//         if (col != null) posicaoBoss = col.bounds.center;

//         // Calcula a distância direta (Magnitude) até o Player
//         float distanciaAtePlayer = Vector2.Distance(posicaoBoss, playerTransform.position);

//         // Se o Player estiver no campo de visão e o cooldown passou
//         if (distanciaAtePlayer <= alcanceProximidade && Time.time >= proximoTempoAtaque)
//         {
//             // Garante que o Boss não está executando a animação de Hit antes de atacar
//             AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
//             if (!stateInfo.IsName("hit") && !stateInfo.IsName("hit2") && !stateInfo.IsName("hit3"))
//             {
//                 IniciarAtaque();
//                 proximoTempoAtaque = Time.time + tempoEntreAtaques;
//             }
//         }
//     }
//     private void IniciarAtaque()
//     {
//         if (animator == null) return;

//         // Limpa Triggers antigos para não accionar ataques atrasados
//         ResetarTriggersAtaque();

//         if (faseAtual == 1)
//         {
//             animator.SetTrigger("AtacarCuspidaDupla");
//         }
//         else if (faseAtual == 2)
//         {
//             animator.SetTrigger("AtacarCuspidaSimples");
//         }
//         else if (faseAtual == 3)
//         {
//             animator.SetTrigger("AtacarRabada");
//         }
//     }

//     private void ResetarTriggersAtaque()
//     {
//         if (animator == null) return;
//         animator.ResetTrigger("AtacarCuspidaDupla");
//         animator.ResetTrigger("AtacarCuspidaSimples");
//         animator.ResetTrigger("AtacarRabada");
//     }

//     public void ExecutarDanoNoPlayer()
//     {
//         if (playerTransform == null) return;

//         float distanciaX = Mathf.Abs(transform.position.x - playerTransform.position.x);
//         if (distanciaX <= alcanceProximidade + 1.5f)
//         {
//             Player scriptPlayer = playerTransform.GetComponent<Player>();
//             if (scriptPlayer == null) scriptPlayer = playerTransform.GetComponentInParent<Player>();

//             if (scriptPlayer != null)
//             {
//                 scriptPlayer.TomarDano(danoAtaque);
//                 Debug.Log("O golpe da animação atingiu o Player!");
//             }
//         }
//     }

//     public void TomarDano(int quantidade = 1)
//     {
//         if (vidaAtualGolpes <= 0) return;

//         vidaAtualGolpes -= quantidade;

//         if (animator != null)
//         {
//             animator.SetTrigger("Hit");
//         }

//         Debug.Log($"Boss sofreu dano! Vida restante: {vidaAtualGolpes}/100");

//         if (healthBar != null)
//         {
//             healthBar.AtualizarBarra(vidaAtualGolpes, vidaMaximaGolpes);
//         }

//         VerificarMudancaDeFase();
//     }

//     private void VerificarMudancaDeFase()
//     {
//         int ataquesSofridos = vidaMaximaGolpes - vidaAtualGolpes;

//         if (faseAtual == 1 && ataquesSofridos >= 25)
//         {
//             faseAtual = 2;
//             ResetarTriggersAtaque();
//             if (animator != null) animator.SetTrigger("MudarFaseUmaCabeca");
//             Debug.Log("Fase 2 ativada!");
//         }
//         else if (faseAtual == 2 && ataquesSofridos >= 50)
//         {
//             faseAtual = 3;
//             ResetarTriggersAtaque();
//             if (animator != null) animator.SetTrigger("MudarFaseRabada");
//             Debug.Log("Fase 3 ativada!");
//         }
//         else if (vidaAtualGolpes <= 0)
//         {
//             DerrotarBoss();
//         }
//     }

//     private void DerrotarBoss()
//     {
//         ResetarTriggersAtaque();
//         if (animator != null) animator.SetTrigger("Morrer");
//         if (telaVitoria != null) telaVitoria.SetActive(true);
//     }

//     private void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.cyan;
//         Gizmos.DrawWireSphere(transform.position, alcanceProximidade);
//     }

//     [Header("Colisor de Ataque (Opcional)")]
//     [SerializeField] private Collider2D colliderAtaque; // Arraste o collider do ataque aqui no Inspector

//     // Função para ATIVAR a colisão (chamada no início do impacto da animação)
//     public void AtivarColliderAtaque()
//     {
//         if (colliderAtaque != null)
//         {
//             colliderAtaque.enabled = true;
//         }
//     }

//     // Função para DESATIVAR a colisão (chamada no final da animação)
//     public void DesativarColliderAtaque()
//     {
//         if (colliderAtaque != null)
//         {
//             colliderAtaque.enabled = false;
//         }
//     }
// }

// using UnityEngine;
// using System.Collections;

// public class BossController : MonoBehaviour
// {
//     [Header("Configurações de Vida do Boss (Golpes)")]
//     [SerializeField] private int vidaMaximaGolpes = 100; // 25 (Fase 1) + 25 (Fase 2) + 50 (Fase 3)
//     [SerializeField] private int vidaAtualGolpes;

//     [Header("Dano e Visão (Proximidade)")]
//     [SerializeField] private float danoAtaque = 25f;
//     [SerializeField] private float alcanceProximidade = 10f; // Distância para o Boss DECIDIR iniciar o ataque
//     [SerializeField] private float tempoEntreAtaques = 3.0f; // Cooldown entre ataques
//     private float proximoTempoAtaque;

//     [Header("Área de Impacto Real do Ataque (Cuspe/Rabada)")]
//     [SerializeField] private Transform pontoImpactoAtaque; // Objeto filho posicionado onde o cuspo cai
//     [SerializeField] private Vector2 tamanhoCaixaAtaque = new Vector2(3.5f, 2.5f); // Dimensão da área de dano do cuspo
//     [SerializeField] private LayerMask playerLayer; // Selecione a Layer 'Player' no Inspector

//     [Header("UI e Referências")]
//     [SerializeField] private BossHealthBar healthBar;
//     [SerializeField] private Animator animator;
//     [SerializeField] private GameObject telaVitoria;
//     [SerializeField] private Transform playerTransform;

//     [Header("Colisor de Ataque Físico (Opcional)")]
//     [SerializeField] private Collider2D colliderAtaque;

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
//     }

//     private void Update()
//     {
//         if (vidaAtualGolpes <= 0 || playerTransform == null) return;

//         // Pega a posição central do boss para medição de distância
//         Vector2 posicaoBoss = transform.position;
//         Collider2D col = GetComponent<Collider2D>();
//         if (col != null) posicaoBoss = col.bounds.center;

//         float distanciaAtePlayer = Vector2.Distance(posicaoBoss, playerTransform.position);

//         // Se o Player estiver na área de visão e o tempo de recarga passou
//         if (distanciaAtePlayer <= alcanceProximidade && Time.time >= proximoTempoAtaque)
//         {
//             // Impede iniciar animação de ataque se estiver reproduzindo animação de Hit
//             AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
//             if (!stateInfo.IsName("hit") && !stateInfo.IsName("hit2") && !stateInfo.IsName("hit3"))
//             {
//                 IniciarAtaque();
//                 proximoTempoAtaque = Time.time + tempoEntreAtaques;
//             }
//         }
//     }

//     private void IniciarAtaque()
//     {
//         if (animator == null) return;

//         ResetarTriggersAtaque();

//         if (faseAtual == 1)
//         {
//             animator.SetTrigger("AtacarCuspidaDupla");
//         }
//         else if (faseAtual == 2)
//         {
//             animator.SetTrigger("AtacarCuspidaSimples");
//         }
//         else if (faseAtual == 3)
//         {
//             animator.SetTrigger("AtacarRabada");
//         }
//     }

//     private void ResetarTriggersAtaque()
//     {
//         if (animator == null) return;
//         animator.ResetTrigger("AtacarCuspidaDupla");
//         animator.ResetTrigger("AtacarCuspidaSimples");
//         animator.ResetTrigger("AtacarRabada");
//     }

//     // MÉTIDO CHAMADO PELO ANIMATION EVENT NO FRAME DE IMPACTO DO GOLPE
//     // public void ExecutarDanoNoPlayer()
//     // {
//     //     Vector2 pontoOriginal = pontoImpactoAtaque != null ? (Vector2)pontoImpactoAtaque.position : (Vector2)transform.position;

//     //     // Verifica colisão da OverlapBox exclusivamente no ponto de impacto visual
//     //     Collider2D playerAtingido = Physics2D.OverlapBox(pontoOriginal, tamanhoCaixaAtaque, 0f, playerLayer);

//     //     if (playerAtingido != null)
//     //     {
//     //         Player scriptPlayer = playerAtingido.GetComponent<Player>();
//     //         if (scriptPlayer == null) scriptPlayer = playerAtingido.GetComponentInParent<Player>();

//     //         if (scriptPlayer != null)
//     //         {
//     //             scriptPlayer.TomarDano(danoAtaque);
//     //             Debug.Log("O cuspe/golpe atingiu o Player com sucesso!");
//     //         }
//     //     }
//     // }

//     // MÉTODO CHAMADO PELO ANIMATION EVENT NO FRAME EXATO DE IMPACTO DO GOLPE
//     public void ExecutarDanoNoPlayer()
//     {
//         // Ponto onde a caixa verde do cuspo está localizada
//         Vector2 pontoOriginal = pontoImpactoAtaque != null ? (Vector2)pontoImpactoAtaque.position : (Vector2)transform.position;

//         // Detecta se o colisor do Player está tocando na caixa do cuspo no momento da animação,
//         // INDEPENDENTE de quão longe o Player esteja do corpo do Boss!
//         Collider2D playerAtingido = Physics2D.OverlapBox(pontoOriginal, tamanhoCaixaAtaque, 0f, playerLayer);

//         if (playerAtingido != null)
//         {
//             Player scriptPlayer = playerAtingido.GetComponent<Player>();
//             if (scriptPlayer == null) scriptPlayer = playerAtingido.GetComponentInParent<Player>();

//             if (scriptPlayer != null)
//             {
//                 scriptPlayer.TomarDano(danoAtaque);
//                 Debug.Log("O cuspo/projétil atingiu o Player com sucesso!");
//             }
//         }
//     }
//     // MÉTODO CHAMADO PELO PLAYER AO ACERTAR O GOLPE COM A ESPADA
//     public void TomarDano(int quantidade = 1)
//     {
//         if (vidaAtualGolpes <= 0) return;

//         vidaAtualGolpes -= quantidade;

//         ResetarTriggersAtaque();

//         if (animator != null)
//         {
//             animator.SetTrigger("Hit");
//         }

//         Debug.Log($"Boss sofreu dano! Vida restante: {vidaAtualGolpes}/100");

//         if (healthBar != null)
//         {
//             healthBar.AtualizarBarra(vidaAtualGolpes, vidaMaximaGolpes);
//         }

//         VerificarMudancaDeFase();
//     }

//     private void VerificarMudancaDeFase()
//     {
//         int ataquesSofridos = vidaMaximaGolpes - vidaAtualGolpes;

//         if (faseAtual == 1 && ataquesSofridos >= 25)
//         {
//             faseAtual = 2;
//             ResetarTriggersAtaque();
//             if (animator != null) animator.SetTrigger("MudarFaseUmaCabeca");
//             Debug.Log("Fase 2 (Uma Cabeça) ativada!");
//         }
//         else if (faseAtual == 2 && ataquesSofridos >= 50)
//         {
//             faseAtual = 3;
//             ResetarTriggersAtaque();
//             if (animator != null) animator.SetTrigger("MudarFaseRabada");
//             Debug.Log("Fase 3 (Rabada) ativada!");
//         }
//         else if (vidaAtualGolpes <= 0)
//         {
//             DerrotarBoss();
//         }
//     }



//     // private void DerrotarBoss()
//     // {
//     //     ResetarTriggersAtaque();
//     //     if (animator != null) animator.SetTrigger("Morrer");

//     //     // Inicia a transição com um pequeno atraso para ver a animação de derrota do Boss
//     //     StartCoroutine(SequenciaVitoria());
//     // }

//     // private System.Collections.IEnumerator SequenciaVitoria()
//     // {
//     //     // Aguarda 1.5 segundos para a animação de morte do Boss acontecer
//     //     yield return new WaitForSeconds(1.5f);

//     //     // Desativa o Player para ele não se mover ou tomar dano durante o vídeo
//     //     if (playerTransform != null)
//     //     {
//     //         playerTransform.gameObject.SetActive(false);
//     //     }

//     //     // Ativa o painel TelaVitoria (que dispara automaticamente o OnEnable do VideoPlayer)
//     //     if (telaVitoria != null)
//     //     {
//     //         telaVitoria.SetActive(true);
//     //     }
//     // }

//     [Header("Configuração da Vitória")]
// [SerializeField] private float atrasoAposMorte = 2.0f; // Tempo extra após o fim da animação de morte
// private bool bossDerrotado = false;

// private void DerrotarBoss()
// {
//     if (bossDerrotado) return;
//     bossDerrotado = true;

//     ResetarTriggersAtaque();
//     if (animator != null) animator.SetTrigger("Morrer");

//     StartCoroutine(SequenciaVitoria());
// }

// private System.Collections.IEnumerator SequenciaVitoria()
// {
//     // Aguarda 1 frame para o Animator iniciar a transição para a animação de morte
//     yield return null;

//     float duracaoAnimacaoMorte = 2.0f; // Valor padrão de segurança

//     if (animator != null)
//     {
//         // Se ainda estiver em transição, espera a transição concluir
//         while (animator.IsInTransition(0))
//         {
//             yield return null;
//         }

//         // Obtém a duração exata do clipe de morte atual
//         AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
//         duracaoAnimacaoMorte = stateInfo.length;
//     }

//     // 1. Espera a animação de morte rodar por completo
//     yield return new WaitForSeconds(duracaoAnimacaoMorte);

//     // 2. Espera mais 2 segundos com o Boss já derretido no chão
//     yield return new WaitForSeconds(atrasoAposMorte);

//     // Desativa o Player para ele não se mover ou sofrer dano durante o vídeo
//     if (playerTransform != null)
//     {
//         playerTransform.gameObject.SetActive(false);
//     }

//     // Abre a TelaVitoria e inicia o vídeo
//     if (telaVitoria != null)
//     {
//         telaVitoria.SetActive(true);
//     }
// }

//     // Suporte para Animation Events de colisor físico, se necessário
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
//         // Raio de proximidade/visão (Círculo Azul)
//         Gizmos.color = Color.cyan;
//         Gizmos.DrawWireSphere(transform.position, alcanceProximidade);

//         // Área real do golpe (Caixa Verde)
//         Gizmos.color = Color.green;
//         Vector2 pontoOriginal = pontoImpactoAtaque != null ? (Vector2)pontoImpactoAtaque.position : (Vector2)transform.position;
//         Gizmos.DrawWireCube(pontoOriginal, tamanhoCaixaAtaque);
//     }
// }

// using UnityEngine;
// using System.Collections;

// public class BossController : MonoBehaviour
// {
//     [Header("Configurações de Vida do Boss (Golpes)")]
//     [SerializeField] private int vidaMaximaGolpes = 100; // 25 (Fase 1) + 25 (Fase 2) + 50 (Fase 3)
//     [SerializeField] private int vidaAtualGolpes;

//     [Header("Dano e Alcances de Ataque")]
//     [SerializeField] private float danoAtaque = 25f;
//     [SerializeField] private float alcanceProximidade = 10f; // Distância máxima de visão/cuspe
//     [SerializeField] private float alcanceRabadaCurta = 3.0f; // Distância para ataque curto/rabada
//     [SerializeField] private float tempoEntreAtaques = 3.0f; // Cooldown entre ataques
//     private float proximoTempoAtaque;

//     [Header("Projétil de Cuspe Teleguiado")]
//     [SerializeField] private GameObject prefabProjetilCuspe; // Arraste o Prefab com ProjetilBoss.cs aqui
//     [SerializeField] private Transform pontoBocaCuspe;       // Objeto vazio na boca do Boss onde o cuspe nasce

//     [Header("Área de Impacto Corpo a Corpo (Rabada)")]
//     [SerializeField] private Transform pontoImpactoAtaque; // Objeto filho para o alcance da rabada
//     [SerializeField] private Vector2 tamanhoCaixaAtaque = new Vector2(3.5f, 2.5f);
//     [SerializeField] private LayerMask playerLayer; // Selecione a Layer 'Player' no Inspector

//     [Header("UI e Referências")]
//     [SerializeField] private BossHealthBar healthBar;
//     [SerializeField] private Animator animator;
//     [SerializeField] private GameObject telaVitoria;
//     [SerializeField] private Transform playerTransform;

//     [Header("Configuração da Vitória")]
//     [SerializeField] private float atrasoAposMorte = 2.0f; // Tempo extra após o fim da animação de morte
//     private bool bossDerrotado = false;

//     [Header("Colisor de Ataque Físico (Opcional)")]
//     [SerializeField] private Collider2D colliderAtaque;

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

//     // private void Start()
//     // {
//     //     rb = GetComponent<Rigidbody2D>();

//     //     GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
//     //     if (playerObj != null)
//     //     {
//     //         direcaoFixa = ((Vector2)playerObj.transform.position - (Vector2)transform.position).normalized;
//     //     }
//     //     else
//     //     {
//     //         direcaoFixa = Vector2.left;
//     //     }

//     //     // Mantém a rotação zerada (0,0,0) para o sprite não virar de cabeça para baixo nem deslocar
//     //     transform.rotation = Quaternion.identity;

//     //     // Só espelha horizontalmente se o Player estiver à direita do Boss
//     //     SpriteRenderer sr = GetComponent<SpriteRenderer>();
//     //     if (sr != null)
//     //     {
//     //         sr.flipX = direcaoFixa.x > 0f;
//     //     }

//     //     Destroy(gameObject, tempoDeVida);
//     // }
//     private void Update()
//     {
//         if (bossDerrotado || vidaAtualGolpes <= 0 || playerTransform == null) return;

//         // Pega a posição central do boss para medição de distância
//         Vector2 posicaoBoss = transform.position;
//         Collider2D col = GetComponent<Collider2D>();
//         if (col != null) posicaoBoss = col.bounds.center;

//         float distanciaAtePlayer = Vector2.Distance(posicaoBoss, playerTransform.position);

//         // Se o Player estiver na área de visão e o tempo de recarga passou
//         if (distanciaAtePlayer <= alcanceProximidade && Time.time >= proximoTempoAtaque)
//         {
//             AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
//             if (!stateInfo.IsName("hit") && !stateInfo.IsName("hit2") && !stateInfo.IsName("hit3"))
//             {
//                 IniciarAtaque(distanciaAtePlayer);
//                 proximoTempoAtaque = Time.time + tempoEntreAtaques;
//             }
//         }
//     }

//     private void IniciarAtaque(float distanciaAtePlayer)
//     {
//         if (animator == null) return;

//         ResetarTriggersAtaque();

//         // Na Fase 3 (ou se o Player colar muito perto), usa o ataque de rabada
//         if (faseAtual == 3 || distanciaAtePlayer <= alcanceRabadaCurta)
//         {
//             animator.SetTrigger("AtacarRabada");
//         }
//         else if (faseAtual == 1)
//         {
//             animator.SetTrigger("AtacarCuspidaDupla");
//         }
//         else if (faseAtual == 2)
//         {
//             animator.SetTrigger("AtacarCuspidaSimples");
//         }
//     }

//     private void ResetarTriggersAtaque()
//     {
//         if (animator == null) return;
//         animator.ResetTrigger("AtacarCuspidaDupla");
//         animator.ResetTrigger("AtacarCuspidaSimples");
//         animator.ResetTrigger("AtacarRabada");
//     }

//     // =========================================================================
//     // ANIMATION EVENT 1: Dispara o projétil teleguiado (use nas animações de Cuspe)
//     // =========================================================================
//     public void DispararCuspeTeleguiado()
//     {
//         if (bossDerrotado || prefabProjetilCuspe == null || pontoBocaCuspe == null) return;
//         Debug.Log($"Evento disparado! Prefab: {prefabProjetilCuspe} | Boca: {pontoBocaCuspe}");

//         Instantiate(prefabProjetilCuspe, pontoBocaCuspe.position, Quaternion.identity);

//         // Vector2 direcao = Vector2.left;
//         // if (playerTransform != null)
//         // {
//         //     direcao = (playerTransform.position - pontoBocaCuspe.position).normalized;
//         // }

//         // float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
//         // Instantiate(prefabProjetilCuspe, pontoBocaCuspe.position, Quaternion.Euler(0f, 0f, angulo));
//     }

//     // =========================================================================
//     // ANIMATION EVENT 2: Dano corpo a corpo (use na animação de Rabada)
//     // =========================================================================
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
//                 Debug.Log("O golpe de rabada atingiu o Player!");
//             }
//         }
//     }

//     // MÉTODO CHAMADO PELO PLAYER AO ACERTAR O GOLPE COM A ESPADA
//     public void TomarDano(int quantidade = 1)
//     {
//         if (bossDerrotado || vidaAtualGolpes <= 0) return;

//         vidaAtualGolpes -= quantidade;

//         ResetarTriggersAtaque();

//         if (animator != null)
//         {
//             animator.SetTrigger("Hit");
//         }

//         Debug.Log($"Boss sofreu dano! Vida restante: {vidaAtualGolpes}/100");

//         if (healthBar != null)
//         {
//             healthBar.AtualizarBarra(vidaAtualGolpes, vidaMaximaGolpes);
//         }

//         VerificarMudancaDeFase();
//     }

//     // private void VerificarMudancaDeFase()
//     // {
//     //     int ataquesSofridos = vidaMaximaGolpes - vidaAtualGolpes;

//     //     if (faseAtual == 1 && ataquesSofridos >= 25)
//     //     {
//     //         faseAtual = 2;
//     //         ResetarTriggersAtaque();
//     //         if (animator != null) animator.SetTrigger("MudarFaseUmaCabeca");
//     //         Debug.Log("Fase 2 (Uma Cabeça) ativada!");
//     //     }
//     //     else if (faseAtual == 2 && ataquesSofridos >= 50)
//     //     {
//     //         faseAtual = 3;
//     //         ResetarTriggersAtaque();
//     //         if (animator != null) animator.SetTrigger("MudarFaseRabada");
//     //         Debug.Log("Fase 3 (Rabada) ativada!");
//     //     }
//     //     else if (vidaAtualGolpes <= 0)
//     //     {
//     //         DerrotarBoss();
//     //     }
//     // }

//     private void VerificarMudancaDeFase()
//     {
//         int ataquesSofridos = vidaMaximaGolpes - vidaAtualGolpes;

//         // Muda para a Fase 2 quando atingir 25% de dano (2 golpes se a vida for 10, ou 25 se for 100)
//         if (faseAtual == 1 && ataquesSofridos >= Mathf.RoundToInt(vidaMaximaGolpes * 0.25f))
//         {
//             faseAtual = 2;
//             ResetarTriggersAtaque();
//             if (animator != null) animator.SetTrigger("MudarFaseUmaCabeca");
//             Debug.Log("Fase 2 (Uma Cabeça) ativada!");
//         }
//         // Muda para a Fase 3 quando atingir 50% de dano (5 golpes se a vida for 10, ou 50 se for 100)
//         else if (faseAtual == 2 && ataquesSofridos >= Mathf.RoundToInt(vidaMaximaGolpes * 0.50f))
//         {
//             faseAtual = 3;
//             ResetarTriggersAtaque();
//             if (animator != null) animator.SetTrigger("MudarFaseRabada");
//             Debug.Log("Fase 3 (Rabada) ativada!");
//         }

//         if (animator != null) animator.SetInteger("FaseAtual", faseAtual);
//         else if (vidaAtualGolpes <= 0)
//         {
//             DerrotarBoss();
//         }
//     }
//     private void DerrotarBoss()
//     {
//         if (bossDerrotado) return;
//         bossDerrotado = true;

//         ResetarTriggersAtaque();
//         if (animator != null) animator.SetTrigger("Morrer");

//         StartCoroutine(SequenciaVitoria());
//     }

//     private IEnumerator SequenciaVitoria()
//     {
//         // Aguarda 1 frame para o Animator iniciar a transição para a animação de morte
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

//         // 1. Espera a animação de morte rodar por completo
//         yield return new WaitForSeconds(duracaoAnimacaoMorte);

//         // 2. Espera mais 2 segundos com o Boss já derretido no chão
//         yield return new WaitForSeconds(atrasoAposMorte);

//         // Desativa o Player para ele não se mover ou sofrer dano durante o vídeo
//         if (playerTransform != null)
//         {
//             playerTransform.gameObject.SetActive(false);
//         }

//         // Abre a TelaVitoria e inicia o vídeo
//         if (telaVitoria != null)
//         {
//             telaVitoria.SetActive(true);
//         }
//     }

//     // Suporte para Animation Events de colisor físico, se necessário
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
//         // Raio de proximidade/cuspe (Círculo Azul)
//         Gizmos.color = Color.cyan;
//         Gizmos.DrawWireSphere(transform.position, alcanceProximidade);

//         // Raio de rabada curta (Círculo Amarelo)
//         Gizmos.color = Color.yellow;
//         Gizmos.DrawWireSphere(transform.position, alcanceRabadaCurta);

//         // Área real do golpe corpo a corpo (Caixa Verde)
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
    [SerializeField] private int vidaMaximaGolpes = 100; // 25 (Fase 1) + 25 (Fase 2) + 50 (Fase 3)
    [SerializeField] private int vidaAtualGolpes;

    [Header("Dano e Alcances de Ataque")]
    [SerializeField] private float danoAtaque = 25f;
    [SerializeField] private float alcanceProximidade = 10f; // Distância máxima de visão/cuspe
    [SerializeField] private float alcanceRabadaCurta = 3.0f; // Distância para ataque curto/rabada
    [SerializeField] private float tempoEntreAtaques = 3.0f; // Cooldown entre ataques
    private float proximoTempoAtaque;

    [Header("Projétil de Cuspe Teleguiado")]
    [SerializeField] private GameObject prefabProjetilCuspe; // Arraste o Prefab com ProjetilBoss.cs aqui
    [SerializeField] private Transform pontoBocaCuspe;       // Objeto vazio na boca do Boss onde o cuspe nasce

    [Header("Área de Impacto Corpo a Corpo (Rabada)")]
    [SerializeField] private Transform pontoImpactoAtaque; // Objeto filho para o alcance da rabada
    [SerializeField] private Vector2 tamanhoCaixaAtaque = new Vector2(3.5f, 2.5f);
    [SerializeField] private LayerMask playerLayer; // Selecione a Layer 'Player' no Inspector

    [Header("UI e Referências")]
    [SerializeField] private BossHealthBar healthBar;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject telaVitoria;
    [SerializeField] private Transform playerTransform;

    [Header("Configuração da Vitória")]
    [SerializeField] private float atrasoAposMorte = 2.0f; // Tempo extra após o fim da animação de morte
    private bool bossDerrotado = false;

    [Header("Colisor de Ataque Físico (Opcional)")]
    [SerializeField] private Collider2D colliderAtaque;

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
        if (bossDerrotado || vidaAtualGolpes <= 0 || playerTransform == null) return;

        Vector2 posicaoBoss = transform.position;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) posicaoBoss = col.bounds.center;

        float distanciaAtePlayer = Vector2.Distance(posicaoBoss, playerTransform.position);

        if (distanciaAtePlayer <= alcanceProximidade && Time.time >= proximoTempoAtaque)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName("hit") && !stateInfo.IsName("hit2") && !stateInfo.IsName("hit3"))
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
        }
        else if (faseAtual == 1)
        {
            animator.SetTrigger("AtacarCuspidaDupla");
        }
        else if (faseAtual == 2)
        {
            animator.SetTrigger("AtacarCuspidaSimples");
        }
    }

    private void ResetarTriggersAtaque()
    {
        if (animator == null) return;
        animator.ResetTrigger("AtacarCuspidaDupla");
        animator.ResetTrigger("AtacarCuspidaSimples");
        animator.ResetTrigger("AtacarRabada");
    }

    public void DispararCuspeTeleguiado()
    {
        if (bossDerrotado || prefabProjetilCuspe == null || pontoBocaCuspe == null) return;
        Instantiate(prefabProjetilCuspe, pontoBocaCuspe.position, Quaternion.identity);
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

        // Se a vida zerou, vai direto para a derrota sem disparar Hit ou troca de fase
        if (vidaAtualGolpes <= 0)
        {
            DerrotarBoss();
            return;
        }

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
            if (animator != null) animator.SetTrigger("MudarFaseUmaCabeca");
        }
        else if (faseAtual == 2 && ataquesSofridos >= Mathf.RoundToInt(vidaMaximaGolpes * 0.50f))
        {
            faseAtual = 3;
            ResetarTriggersAtaque();
            if (animator != null) animator.SetTrigger("MudarFaseRabada");
        }

        if (animator != null) animator.SetInteger("FaseAtual", faseAtual);
    }

    private void DerrotarBoss()
    {
        if (bossDerrotado) return;
        bossDerrotado = true;

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

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            duracaoAnimacaoMorte = stateInfo.length;
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