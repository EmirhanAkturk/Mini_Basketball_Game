using UnityEngine;

public class BasketCheckher : MonoBehaviour
{
    public static event GameplayUIController.BasketAction basketListener;

    private void OnTriggerEnter(Collider other)
    {
        basketListener?.Invoke();
    }

}
