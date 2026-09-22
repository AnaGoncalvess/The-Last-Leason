using UnityEngine;

/// <summary>
/// Plataforma que oscila entre a posição inicial e um deslocamento (em coordenadas locais).
/// Requer Rigidbody2D Kinematic (não confia em física, só se move por transform/MovePosition).
/// O Player "gruda" nela virando filho temporário enquanto estiver em cima (ver PlatformPassenger).
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector2 offset = new Vector2(3f, 0f);
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool startMovingToOffset = true;

    private Rigidbody2D rb;
    private Vector2 pointA;
    private Vector2 pointB;
    private Vector2 target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        pointA = rb.position;
        pointB = rb.position + offset;
        target = startMovingToOffset ? pointB : pointA;
    }

    private void FixedUpdate()
    {
        Vector2 newPosition = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        if (Vector2.Distance(newPosition, target) < 0.01f)
        {
            target = target == pointA ? pointB : pointA;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 a = Application.isPlaying ? (Vector3)pointA : transform.position;
        Vector3 b = a + (Vector3)offset;
        Gizmos.DrawLine(a, b);
        Gizmos.DrawWireSphere(a, 0.2f);
        Gizmos.DrawWireSphere(b, 0.2f);
    }
}
