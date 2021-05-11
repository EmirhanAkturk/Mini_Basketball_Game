using UnityEngine;

public class BasketCheckher : MonoBehaviour
{
    public static event GameplayUIController.BasketAction BasketListener;
    public static event CameraShake.ShakeAction ShakeListener;
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball")) 
        {
            BasketListener?.Invoke();
            ShakeListener?.Invoke();
        }
        
        other.gameObject.tag = "Untagged";
    }

}
