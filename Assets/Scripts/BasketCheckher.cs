using UnityEngine;

public class BasketCheckher : MonoBehaviour
{
    public static event GameplayUIController.BasketAction BasketListener;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball")) 
        {
            BasketListener?.Invoke();
            Debug.Log("Baskett!!");
        }
        
        other.gameObject.tag = "Untagged";
    }

}
