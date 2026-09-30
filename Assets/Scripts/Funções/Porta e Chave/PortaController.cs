using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortaController : MonoBehaviour
{
    [Header("Configurações da Porta")]
    [SerializeField] private Animator anim;
    [SerializeField] private Transform pontoEntrada;
    [SerializeField] private string proximaCenaName = "Nivel2";

    private bool portaAberta = false;
    private bool playerTransicionando = false;
    private Transform playerTransform;

    public Transform PontoFechadura => pontoEntrada;

    public void AbrirPorta()
    {
        if (anim != null)
        {
            anim.SetTrigger("abrir"); // Toca a animação da porta abrindo
        }
        portaAberta = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Se a porta estiver aberta e o Player se aproximar dela
        if (portaAberta && !playerTransicionando && (collision.CompareTag("Player") || collision.GetComponentInParent<Player>() != null))
        {
            playerTransicionando = true;
            playerTransform = collision.transform;

            // Desativa o controle do Player para travar o movimento
            Player scriptPlayer = collision.GetComponentInParent<Player>();
            if (scriptPlayer != null) scriptPlayer.enabled = false;

            StartCoroutine(RotinaTransicaoNivel());
        }
    }

    private void Update()
    {
        // Centraliza suavemente o Player em frente à porta
        if (playerTransicionando && playerTransform != null && pontoEntrada != null)
        {
            playerTransform.position = Vector3.MoveTowards(
                playerTransform.position,
                new Vector3(pontoEntrada.position.x, playerTransform.position.y, playerTransform.position.z),
                3f * Time.deltaTime
            );
        }
    }

    private IEnumerator RotinaTransicaoNivel()
    {
        yield return new WaitForSeconds(1.0f); // Aguarda a animação e o alinhamento
        SceneManager.LoadScene(proximaCenaName); // Carrega o próximo nível
    }
}