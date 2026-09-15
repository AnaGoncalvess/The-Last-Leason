using UnityEngine;

public class HealthUIAnimator : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void UpdateHealthUI(int hits)
    {
        if (anim != null)
        {
            // Atualiza o parâmetro "hits" no Animator
            anim.SetInteger("hits", hits);
        }
    }
}