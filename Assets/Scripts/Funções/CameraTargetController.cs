using UnityEngine;

public class CameraTargetController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Camera mainCamera;

    [Header("Horizontal Progression")]
    [SerializeField] float forwardActivationViewportX = 0.5f;
    [SerializeField] float backwardLimitViewportX = 0.05f;

    private float targetX;
    private float fixedY;
    private float fixedZ;
    private float previousPlayerX;

    public bool IsBlockingBackwardMovement { get; private set; }

    private void Start()
    {
        // Se a câmera não for atribuída no Inspector, pega a Main Camera automaticamente
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Pega a posição inicial definida na cena
        targetX = transform.position.x;
        fixedY = transform.position.y;
        fixedZ = transform.position.z;

        if (player != null)
        {
            previousPlayerX = player.position.x;
        }
    }

    private void LateUpdate()
    {
        if (player == null || mainCamera == null)
            return;

        // Converte a posição do Player para coordenadas da viewport da câmera (0.0 a 1.0)
        Vector3 playerViewportPosition = mainCamera.WorldToViewportPoint(player.position);

        // O bloqueio ativa SOMENTE se o player alcançar a margem limite na borda esquerda
        IsBlockingBackwardMovement = playerViewportPosition.x <= backwardLimitViewportX;

        float playerDeltaX = player.position.x - previousPlayerX;

        bool reachedForwardLimit = playerViewportPosition.x >= forwardActivationViewportX;
        bool playerMovedForward = playerDeltaX > 0f;

        // Avança a câmera apenas se o player passar do limite frontal e estiver indo para a direita
        if (reachedForwardLimit && playerMovedForward)
        {
            targetX += playerDeltaX;
        }

        transform.position = new Vector3(targetX, fixedY, fixedZ);
        previousPlayerX = player.position.x;
    }
}