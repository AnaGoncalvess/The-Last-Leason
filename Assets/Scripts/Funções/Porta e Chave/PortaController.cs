using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortaController : MonoBehaviour
{
    [Header("Configurações da Porta")]
    [SerializeField] private Animator anim;
    [SerializeField] private Transform pontoEntrada; // Objeto vazio posicionado no centro da porta
    [SerializeField] private string proximaCenaName = "Nivel2"; // Nome da próxima cena no Build Settings

    private bool portaAberta = false;
    private bool playerMoverParaPorta = false;
    private Transform playerTransform;

    public Transform PontoFechadura => pontoEntrada;

    // Chamado pelo script da Chave ao chegar na fechadura
    public void AbrirPorta()
    {
        if (anim != null)
        {
            anim.SetTrigger("abrir"); // Toca do Frame 3 ao 10
        }

        portaAberta = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Se a porta estiver aberta e o Player tocar no Trigger da porta
        if (portaAberta && (collision.CompareTag("Player") || collision.GetComponentInParent<Player>() != null))
        {
            playerTransform = collision.transform;
            
            // Trava o script do Player para que o jogador não se mova mais
            Player scriptPlayer = collision.GetComponentInParent<Player>();
            if (scriptPlayer != null) scriptPlayer.enabled = false;

            playerMoverParaPorta = true;
            StartCoroutine(CarregarProximaFase());
        }
    }

    private void Update()
    {
        // Se ativado, conduz o Player suavemente até o centro da porta
        if (playerMoverParaPorta && playerTransform != null && pontoEntrada != null)
        {
            playerTransform.position = Vector3.MoveTowards(
                playerTransform.position,
                new Vector3(pontoEntrada.position.x, playerTransform.position.y, playerTransform.position.z),
                3f * Time.deltaTime
            );
        }
    }

    private IEnumerator CarregarProximaFase()
    {
        // Aguarda 1 segundo com o player parado na porta antes de trocar de cena
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(proximaCenaName);
    }
}