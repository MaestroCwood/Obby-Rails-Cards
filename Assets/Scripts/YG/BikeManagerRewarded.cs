using StarterAssets;
using System;
using UnityEngine;
using YG;
using static BikeRewarded;

public class BikeManagerRewarded : MonoBehaviour
{

    [SerializeField] GameObject[] bikees;
    [SerializeField] GameObject[] offsetMeshPlayer;
    [SerializeField] GameObject btnExit;
    [SerializeField] Animator animatorPlayer;
   // Skate, Scooter, Bicycle

   
    PressAsset assetsInputs;
    
    private void OnEnable()
    {
        YG2.onRewardAdv += OnRewardedShow;
        BikeRewarded.OnBikeRewarded += OnActivateBike;

        GoToRainds.OnStartRaindGoMove += GoToRainds_OnStartRaindGoMove;
        assetsInputs.Player.PressF.performed += PressF_performed;
    }

 
    private void OnDisable()
    {
        YG2.onRewardAdv += OnRewardedShow;
        BikeRewarded.OnBikeRewarded -= OnActivateBike;

        GoToRainds.OnStartRaindGoMove -= GoToRainds_OnStartRaindGoMove;
        assetsInputs.Player.PressF.performed -= PressF_performed;
    }

    private void Awake()
    {
        assetsInputs = new PressAsset();
        assetsInputs.Enable(); 
    }
    private void OnActivateBike(BikeRewarded.BikeType type)
    {
        switch (type)
        {
            case BikeType.Skate:
                ActivateByke(0);
                break;
            case BikeType.Scooter:
                ActivateByke(1);
                break;
            case BikeType.Bicycle:
                ActivateByke(2);
                break;
        }
    }

    private void OnRewardedShow(string bykeType)
    {
        switch (bykeType)
        {
            case "Skate":
                ActivateByke(0);
                OffsetMeshPlayer(.5f);
                break;
            case "Scooter":
                ActivateByke(1);
                OffsetMeshPlayer(.7f);
                break;
            case "Bicycle":
                ActivateByke(2);
                OffsetMeshPlayer(1.6f);
                break;
        }
    }

    void ActivateByke(int index)
    {
        for (int i = 0; i < bikees.Length; i++)
            bikees[i].SetActive(false);

        bikees[index].SetActive(true);

        


        SetWeghtAnitaor(1);
        ActivateUiSkate(true);
    }

    void SetWeghtAnitaor(float inex)
    {
        animatorPlayer.SetLayerWeight(2, inex);
    }

    void ActivateUiSkate(bool isActve)
    {
        btnExit.SetActive(isActve);
    }

    void OffsetMeshPlayer(float offset)
    {
        for (int i = 0; i < offsetMeshPlayer.Length; i++)
        {
            offsetMeshPlayer[i].transform.localPosition = new Vector3(0, offset, 0);
        }
    }

    public void ExitByke()
    {

        for (int i = 0; i < bikees.Length; i++)
            bikees[i].SetActive(false);
        SetWeghtAnitaor(0);
        ActivateUiSkate(false);

        OffsetMeshPlayer(0);
    }

    private void PressF_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        ExitByke();
        Debug.Log("PRESS F");
    }
    private void GoToRainds_OnStartRaindGoMove(object sender, EventArgs e)
    {
        ExitByke();
    }

}
