using StarterAssets;
using UnityEngine;

public class PlayerVisualFx : MonoBehaviour
{
    [SerializeField] ThirdPersonController playerController;
    [SerializeField] ParticleSystem onGroundFx;



    private void Start()
    {

    }

    private void OnEnable()
    {
        if (playerController != null)
        {
            // —начала отписываемс€ Ч на случай дубл€
            playerController.OnLandPlayer -= OnLand;
            playerController.OnLandPlayer += OnLand;
        }
    }


    private void OnDisable()
    {
        playerController.OnLandPlayer -= OnLand;
    }


    void OnLand()
    {
        onGroundFx.Play();

       
      
        //Instantiate(onGroundFx, transform.position, Quaternion.identity);
        //Debug.Log($"OnLand() called! Frame: {Time.frameCount}, this: {GetInstanceID()}");
    }
}
