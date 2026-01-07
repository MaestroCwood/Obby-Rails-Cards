using StarterAssets;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;




public class SplineMove : MonoBehaviour
{

    public SplineAnimate splineAnimate;
    public SplineContainer splineContainer;
    public CinemachineCamera playerFreeLook;
    public CinemachineCamera splineFreeLook;
    public ThirdPersonController playerController;
    public PlayerInput _input;
    public GameObject[] uiObjectTargetDisabled;
    public bool isMoveHome = false;
  
    Vector3 worldPositionLastKnot;
    private void Start()
    {
        splineAnimate.Completed += OnSplineFinished;
       

        Spline spline = splineContainer.Splines[0];
       

        var lastSpline = splineContainer.Splines[splineContainer.Splines.Count - 1];
        var lastKnot = lastSpline[lastSpline.Count - 1];
        Vector3 localPosition = lastKnot.Position;
        worldPositionLastKnot = splineContainer.transform.TransformPoint(localPosition);

        // ѕреобразуем локальную позицию в мировую
        //Vector3 worldPosition = container.transform.TransformPoint(lastKnot.Position);

    }

   

    private void OnDisable()
    {
        CancelInvoke();
        splineAnimate.Completed -= OnSplineFinished;
       
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (isMoveHome)
            {
                playerController.enabled = false;
                playerController.GetComponent<Animator>().enabled = false;
                playerController.gameObject.transform.SetParent(splineAnimate.transform, false);
             
                playerController.transform.localPosition = new Vector3(0,2,0);
            }

            SwitchCameraToSpline();

            DisableControlPlayer();

            DisableObjectUi(false);
            GameEvents.OnSplineStarted?.Invoke();
            Debug.Log("Trigger Spline");
        }
    }
    private void OnSplineFinished()
    {
        TeleportPlayer();
        EnableMainCam();

        Invoke("ResetAnimateSpline", 3f);

        CoinEffect.Play(GameManager.instance.GetCurrentGenerate());
       
        DisableObjectUi(true);
        EnabledControlPlayer();


        if (isMoveHome)
        {
            playerController.enabled = true;
            playerController.GetComponent<Animator>().enabled = enabled;
            playerController.gameObject.transform.SetParent(null);
            playerController.gameObject.transform.localScale = Vector3.one;

            //playerController.transform.localPosition = Vector3.zero;
        }

        
        Debug.Log("FINISH SPLINE!");
    }

    void SwitchCameraToSpline()
    {

       
        playerFreeLook.Priority = 0;
        splineFreeLook.Priority = 10;
        Invoke("PlayToSlide", 1.5f);
    }


    void EnableMainCam()
    {
        playerFreeLook.Priority = 10;
        splineFreeLook.Priority = 0;
    }

    void PlayToSlide()
    {
        splineAnimate.Play();
    }

    void ResetAnimateSpline()
    {   
        splineAnimate.Restart(false);
    }

    void DisableControlPlayer()
    {
       // _input.enabled = false;
        _input.DeactivateInput();
        
    }

    void EnabledControlPlayer()
    {
        _input.ActivateInput();
    }

    void TeleportPlayer()
    {
        playerController.Teleport(worldPositionLastKnot);
    }


    void DisableObjectUi(bool isActive)
    {
        foreach (var item in uiObjectTargetDisabled)
        {
            item.SetActive(isActive);
            var joy = item.GetComponentInChildren<UIVirtualJoystick>();
            if (joy != null)
                joy.ResetJoystick();
        }
    }
}
