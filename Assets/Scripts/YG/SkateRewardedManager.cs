using StarterAssets;
using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class SkateRewardedManager : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    [SerializeField] CharacterController playerController;
    [SerializeField] GameObject[] playerMeshs;
    [SerializeField] Transform holdSkatePos;
    [SerializeField] Material[] skateMaterials;
    [SerializeField] GameObject skateObj1;
    [SerializeField] GameObject skateObj2;
    [SerializeField] GameObject skateObj3;
    [SerializeField] GameObject skateObj4;
    [SerializeField] GameObject skateObj5;
    [SerializeField] GameObject skateObj6;
    [SerializeField] GameObject skateObj7;
    [SerializeField] GameObject skateObj8;
    [SerializeField] GameObject skateObj9;
    [SerializeField] GameObject skateObj10;
    [SerializeField] Button exitSkateBtn;
    [SerializeField] ThirdPersonController thirdPlayerCont;
    [SerializeField] float boostSpeed = 3f;

    Vector3 playerDefoultCenterCollider;
    GameObject currentObjSkate;
    float defaultSprintSpeed;
    public bool isCurrentSkateActive { get; private set; }
    private void OnEnable()
    {
        YG2.onRewardAdv += OnRewardedAdw;
        GameEvents.OnActivateSkate += OnActiveateSkate;
        exitSkateBtn.onClick.AddListener(() =>
        {
            ExitSkate();
        });
    }

    private void OnDisable()
    {
        YG2.onRewardAdv -= OnRewardedAdw;
        GameEvents.OnActivateSkate -= OnActiveateSkate;
    }

    private void Start()
    {
       // playerController.center = new Vector3(0,1,0);
        defaultSprintSpeed = thirdPlayerCont.SprintSpeed;
    }

    private void OnActiveateSkate(YgRewardedSkate.SkateBoardReward reward)
    {
        // SpawnSkate();

        switch (reward)
        {
            case YgRewardedSkate.SkateBoardReward.Skate1:
                SpawnSkate(skateObj1);
                break;
            case YgRewardedSkate.SkateBoardReward.Skate2:
                SpawnSkate(skateObj2);
                break;
            case YgRewardedSkate.SkateBoardReward.Skate3:
                SpawnSkate(skateObj3);
                break;
            case YgRewardedSkate.SkateBoardReward.Skate4:
                SpawnSkate(skateObj4);
                break;
            case YgRewardedSkate.SkateBoardReward.Skate5:
                SpawnSkate(skateObj6);
                break;
            case YgRewardedSkate.SkateBoardReward.Skate7:
                SpawnSkate(skateObj7);
                break;
            case YgRewardedSkate.SkateBoardReward.Skate8:
                SpawnSkate(skateObj8);
                break;
            case YgRewardedSkate.SkateBoardReward.Skate9:
                SpawnSkate(skateObj9);
                break;
            case YgRewardedSkate.SkateBoardReward.Skate10:
                SpawnSkate(skateObj10);
                break;
        }
    }

    private void OnRewardedAdw(string obj)
    {
        switch (obj)
        {
            case "Skate1":
                SpawnSkate(skateObj1);
                break;
            case "Skate2":
                SpawnSkate(skateObj2);
                break;
            case "Skate3":
                SpawnSkate(skateObj3);
                break;
            case "Skate4":
                SpawnSkate(skateObj4);
                break;
            case "Skate5":
                SpawnSkate(skateObj5);
                break;
            case "Skate6":
                SpawnSkate(skateObj6);
                break;
            case "Skate7":
                SpawnSkate(skateObj7);
                break;
            case "Skate8":
                SpawnSkate(skateObj8);
                break;
            case "Skate9":
                SpawnSkate(skateObj9);
                break;
            case "Skate10":
                SpawnSkate(skateObj10);
                break;

        }

        
    }

    void ExitSkate()
    {
        exitSkateBtn.gameObject.SetActive(false);
        playerController.Move(Vector3.up * 10f);
       // playerController.center = playerDefoultCenterCollider;
        playerAnimator.SetLayerWeight(2, 0);
        isCurrentSkateActive = false;
        if (currentObjSkate != null)
        {
            Destroy(currentObjSkate);
            currentObjSkate = null;
        }
        thirdPlayerCont.SprintSpeed = defaultSprintSpeed;
        OfssetMesh(0);

    }


    void SpawnSkate(GameObject skateIndex)
    {
        RaycastHit hit;
        Vector3 spawnPos = holdSkatePos.position;
        if (Physics.Raycast(playerController.transform.position, Vector3.down, out hit, 5f))
        {
            spawnPos = hit.point + Vector3.up * 0.05f;
            // currentObjSkate.transform.position = pos;
        }
        if (currentObjSkate != null)
        {
            Destroy(currentObjSkate);
            currentObjSkate = null;
        }
            
       
        
        playerAnimator.SetLayerWeight(2, 1);
        currentObjSkate = Instantiate(skateIndex, spawnPos, Quaternion.identity);
        
        
        
        //   currentObjSkate.transform.localRotation = Quaternion.Euler(0, -90, 0);

        currentObjSkate.transform.position = hit.point + Vector3.up * 0.05f;
        playerDefoultCenterCollider = playerController.center;
     //   playerController.center = new Vector3(0, 1.12f, 0);
        exitSkateBtn.gameObject.SetActive(true);
        isCurrentSkateActive = true;
        currentObjSkate.transform.SetParent(holdSkatePos.transform, false);
        currentObjSkate.transform.localPosition = Vector3.zero;
        thirdPlayerCont.Teleport(new Vector3(-12f, 5f, -26f));
        thirdPlayerCont.SprintSpeed = defaultSprintSpeed + boostSpeed;
        OfssetMesh(.5f);
    }

    void OfssetMesh(float offsetY)
    {
        for (int i = 0; i < playerMeshs.Length; i++)
        {
            playerMeshs[i].transform.localPosition = new Vector3(0, offsetY);
        }
    }
}
