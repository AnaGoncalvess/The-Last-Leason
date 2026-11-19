// using UnityEngine;

// public class DanoLaserDireto : MonoBehaviour
// {
//     [SerializeField] private float dano = 1f;
//     private bool jaAcertouNesteDisparo = false;

//     private void OnEnable()
//     {
//         jaAcertouNesteDisparo = false;
//     }

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (jaAcertouNesteDisparo) return;

//         if (collision.CompareTag("Player") || collision.GetComponentInParent<Player>() != null)
//         {
//             Player scriptPlayer = collision.GetComponent<Player>();
//             if (scriptPlayer == null) scriptPlayer = collision.GetComponentInParent<Player>();

//             if (scriptPlayer != null)
//             {
//                 scriptPlayer.TomarDano(dano);
//                 jaAcertouNesteDisparo = true;
//             }
//         }
//     }
// }

using UnityEngine;

public class DanoLaserDireto : MonoBehaviour
{
    [SerializeField] private float dano = 1f;
    private bool jaAcertouNesteDisparo = false;

    private void OnEnable()
    {
        jaAcertouNesteDisparo = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TentarAplicarDano(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TentarAplicarDano(collision);
    }

    private void TentarAplicarDano(Collider2D collision)
    {
        if (jaAcertouNesteDisparo) return;

        if (collision.CompareTag("Player") || collision.GetComponentInParent<Player>() != null)
        {
            Player scriptPlayer = collision.GetComponent<Player>();
            if (scriptPlayer == null) scriptPlayer = collision.GetComponentInParent<Player>();

            if (scriptPlayer != null)
            {
                scriptPlayer.TomarDano(dano);
                jaAcertouNesteDisparo = true;
            }
        }
    }
}