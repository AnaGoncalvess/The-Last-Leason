// // using UnityEngine;

// // public class ChaveVoadora : MonoBehaviour
// // {
// //     [SerializeField] private float velocidade = 5f;
// //     private Transform alvoFechadura;
// //     private PortaController porta;

// //     public void IniciarVoo(Transform destino, PortaController portaAlvo)
// //     {
// //         alvoFechadura = destino;
// //         porta = portaAlvo;
// //     }

// //     private void Update()
// //     {
// //         if (alvoFechadura == null) return;

// //         // Move a chave gradualmente em direção à fechadura
// //         transform.position = Vector3.MoveTowards(transform.position, alvoFechadura.position, velocidade * Time.deltaTime);

// //         // Quando chega à fechadura
// //         if (Vector3.Distance(transform.position, alvoFechadura.position) < 0.05f)
// //         {
// //             if (porta != null)
// //             {
// //                 porta.AbrirPorta(); // Inicia a animação da porta (Frame 3 em diante)
// //             }
// //             Destroy(gameObject); // Oculta/destrói a chave voadora
// //         }
// //     }
// // }


// using System.Collections;
// using UnityEngine;

// [RequireComponent(typeof(Rigidbody2D))]
// public class ChaveVoadora : MonoBehaviour
// {
//     [Header("Configurações do Voo")]
//     [SerializeField] private float velocidadeVoo = 8f;
//     [SerializeField] private float tempoEsperandoAposQueda = 1.0f; // Tempo no chão antes de voar

//     private Transform alvoFechadura;
//     private PortaController porta;
//     private Rigidbody2D rb;
//     private bool estaVoando = false;

//     private void Awake()
//     {
//         rb = GetComponent<Rigidbody2D>();
//     }

//     // Chamado pelo ColetaveisManager assim que todos os itens são recolhidos
//     public void IniciarQuedaEVoo(Transform destinoFechadura, PortaController portaAlvo)
//     {
//         alvoFechadura = destinoFechadura;
//         porta = portaAlvo;

//         // Inicia a contagem para começar o voo até a porta após cair
//         StartCoroutine(RotinaQuedaEVoo());
//     }

//     private IEnumerator RotinaQuedaEVoo()
//     {
//         // 1. A chave cai livremente com a gravidade da Unity
//         yield return new WaitForSeconds(tempoEsperandoAposQueda);

//         // 2. Prepara a chave para voar
//         rb.bodyType = Rigidbody2DType.Kinematic; // Desativa a física de gravidade/colisão dura
//         rb.linearVelocity = Vector2.zero;
//         estaVoando = true;
//     }

//     private void Update()
//     {
//         if (!estaVoando || alvoFechadura == null) return;

//         // Move a chave do chão em direção à fechadura
//         transform.position = Vector3.MoveTowards(
//             transform.position, 
//             alvoFechadura.position, 
//             velocidadeVoo * Time.deltaTime
//         );

//         // Quando chega na fechadura
//         if (Vector3.Distance(transform.position, alvoFechadura.position) < 0.08f)
//         {
//             if (porta != null)
//             {
//                 porta.AbrirPorta(); // Dispara a animação da porta (Frame 3 em diante)
//             }
//             Destroy(gameObject); // Oculta a chave
//         }
//     }
// }

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ChaveVoadora : MonoBehaviour
{
    [Header("Configurações do Voo")]
    [SerializeField] private float velocidadeVoo = 8f;
    [SerializeField] private float tempoEsperandoAposQueda = 1.0f;

    private Transform alvoFechadura;
    private PortaController porta;
    private Rigidbody2D rb;
    private bool estaVoando = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Mantido com o nome IniciarVoo para ser compatível com o ColetaveisManager
    public void IniciarVoo(Transform destinoFechadura, PortaController portaAlvo)
    {
        alvoFechadura = destinoFechadura;
        porta = portaAlvo;

        StartCoroutine(RotinaQuedaEVoo());
    }

    private IEnumerator RotinaQuedaEVoo()
    {
        // 1. Aguarda cair e quicar no chão
        yield return new WaitForSeconds(tempoEsperandoAposQueda);

        // 2. Prepara para voar (muda o tipo de Rigidbody2D corretamente)
        rb.bodyType = RigidbodyType2D.Kinematic; 
        rb.linearVelocity = Vector2.zero;
        estaVoando = true;
    }

    private void Update()
    {
        if (!estaVoando || alvoFechadura == null) return;

        // Move a chave até a fechadura
        transform.position = Vector3.MoveTowards(
            transform.position, 
            alvoFechadura.position, 
            velocidadeVoo * Time.deltaTime
        );

        // Quando chega na fechadura
        if (Vector3.Distance(transform.position, alvoFechadura.position) < 0.08f)
        {
            if (porta != null)
            {
                porta.AbrirPorta();
            }
            Destroy(gameObject);
        }
    }
}