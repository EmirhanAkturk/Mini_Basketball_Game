using UnityEngine;

public class BasketCheckher : MonoBehaviour
{
    public static event GameplayUIController.BasketAction BasketListener;

    private void OnTriggerEnter(Collider other)
    {
        BasketListener?.Invoke();
    }

}
