using UnityEngine;
using UnityEngine.EventSystems;

public class BlockAttackOnUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static bool isTouchingUI = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouchingUI = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouchingUI = false;
    }
}