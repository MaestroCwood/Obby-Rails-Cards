using StarterAssets;
using UnityEngine;

public class DeadZoneFloor : MonoBehaviour
{
    [SerializeField] ThirdPersonController playerController;
  

    Vector3 startPosPlayer;

    private void Start()
    {
        startPosPlayer = playerController.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            LoseResetPosition();
            Debug.Log("Trigger lose!!!");
        }
    }



   void LoseResetPosition()
    {
        AudioManager.instance.PlayFx(0);
        playerController.Teleport(startPosPlayer);
    }
}
