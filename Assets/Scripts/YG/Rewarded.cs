using StarterAssets;
using System;
using UnityEngine;
using YG;

public class Rewarded : MonoBehaviour
{


    private void Start()
    {
        YG2.onRewardAdv += OnRewardedComlited;
    }

    private void OnDisable()
    {
        YG2.onRewardAdv -= OnRewardedComlited;
    }

    private void OnRewardedComlited(string obj)
    {


        switch (obj)
        {
            case "1":
                MultiplayX2Coin();
                break;
            case "2":
                AddCoin();
                break;
            case "3":
                BoostJump();
                break;
        }
    }

    public void MultiplayX2Coin()
    {
        GameManager.instance.SetCountGenerateCoin(GameManager.instance.GetCurrentGenerate() * 2);
        Debug.Log("X2 Genetaror!");
    }
    private void AddCoin()
    {
        GameManager.instance.AddCoin(10000);
    }
    private void BoostJump()
    {
        ThirdPersonController playerController = FindAnyObjectByType<ThirdPersonController>();
        float currentJump = playerController.JumpHeight;
        float targetJump = playerController.JumpHeight = currentJump * 1.5f;
        playerController.JumpHeight = Mathf.Clamp(targetJump, 3.5f, 30f);
    }

}



