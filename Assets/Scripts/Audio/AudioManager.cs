using StarterAssets;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] ThirdPersonController playerController;
    [SerializeField] StarterAssetsInputs assetsInputs;
    public AudioSource audioSourceFx;
    public AudioClip[] audioClipsFx;


    bool isActiveJump =true;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }



    public void PlayFx(int index)
    {   
        
        audioSourceFx.PlayOneShot(audioClipsFx[index]);
    }


    private void OnEnable()
    {
        GameEvents.OnDestrouBridgeElement += OnDestroyBridgeElement;
        assetsInputs.OnStartJumpPlayer += PlayerController_OnStartJumpPlayer;
        playerController.OnLandPlayer += PlayerController_OnLandPlayer;


    }


    private void OnDisable()
    {
        GameEvents.OnDestrouBridgeElement -= OnDestroyBridgeElement;
        assetsInputs.OnStartJumpPlayer -= PlayerController_OnStartJumpPlayer;
        playerController.OnLandPlayer -= PlayerController_OnLandPlayer;
    }

    private void OnDestroyBridgeElement(Vector3 vector)
    {
        PlayFx(1); 
    }
    private void PlayerController_OnStartJumpPlayer()
    {
      
        if (isActiveJump)
        {
            PlayFx(2);
        }
        isActiveJump = false;
    }

    private void PlayerController_OnLandPlayer()
    {
        PlayFx(3);
        isActiveJump = true;
    }

}
