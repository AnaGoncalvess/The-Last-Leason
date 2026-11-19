// using UnityEngine;

// public class LimiteCameraX : MonoBehaviour
// {
//     [Header("Limites Horizontais da Câmera (Eixo X)")]
//     [SerializeField] private bool ativarLimite = true;
//     [SerializeField] private float minX = -5f;
//     [SerializeField] private float maxX = 5f;

//     // Roda no final de cada frame, após o Cinemachine/Virtual Camera calcular a posição
//     private void LateUpdate()
//     {
//         if (!ativarLimite) return;

//         Vector3 pos = transform.position;
//         pos.x = Mathf.Clamp(pos.x, minX, maxX);
//         transform.position = pos;
//     }
// }

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class LimiteCameraX : MonoBehaviour
{
    [Header("Alvo")]
    [SerializeField] private Transform player;

    [Header("Bordas Reais do Cenário (Onde começa o vazio)")]
    [Tooltip("Posição X exata da parede esquerda da sala")]
    [SerializeField] private float bordaEsquerdaSala = -10f;
    [Tooltip("Posição X exata da parede direita da sala (logo após o Boss/estante)")]
    [SerializeField] private float bordaDireitaSala = 10f;

    [Header("Suavização")]
    [SerializeField] private float suavidade = 10f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    private void LateUpdate()
    {
        if (player == null || cam == null) return;

        // Calcula automaticamente a metade da largura que a câmera enxerga na tela
        float metadeLarguraCamera = cam.orthographicSize * cam.aspect;

        // Define o mínimo e máximo que o CENTRO da câmera pode ir sem a borda da tela mostrar o vazio
        float minCentroCamera = bordaEsquerdaSala + metadeLarguraCamera;
        float maxCentroCamera = bordaDireitaSala - metadeLarguraCamera;

        float alvoX;

        // Se a sala inteira for menor ou igual à largura da tela, trava no meio exato da sala
        if (minCentroCamera >= maxCentroCamera)
        {
            alvoX = (bordaEsquerdaSala + bordaDireitaSala) * 0.5f;
        }
        else
        {
            // Segue o Player até a borda da câmera encostar na borda da sala
            alvoX = Mathf.Clamp(player.position.x, minCentroCamera, maxCentroCamera);
        }

        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, alvoX, suavidade * Time.deltaTime);
        transform.position = pos;
    }

    // Desenha duas linhas amarelas na janela Scene mostrando exatamente onde estão as bordas configuradas
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(bordaEsquerdaSala, -10f, 0f), new Vector3(bordaEsquerdaSala, 10f, 0f));
        Gizmos.DrawLine(new Vector3(bordaDireitaSala, -10f, 0f), new Vector3(bordaDireitaSala, 10f, 0f));
    }
}