using UnityEngine;

public class TriggerDestroyBridge : MonoBehaviour
{
    [SerializeField] WindowAnimate windowAnimate;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            windowAnimate.ShowWithAnimation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            windowAnimate?.HideWithAnimation();
    }
}
