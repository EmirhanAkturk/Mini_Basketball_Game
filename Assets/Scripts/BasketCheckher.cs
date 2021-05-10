using UnityEngine;

public class BasketCheckher : MonoBehaviour
{
    public static event GameplayUIController.BasketAction BasketListener;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ball")
            BasketListener?.Invoke();
        
        other.gameObject.tag = "Untagged";
    }

}
