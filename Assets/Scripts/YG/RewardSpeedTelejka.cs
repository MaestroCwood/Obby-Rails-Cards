using System;
using UnityEngine;
using UnityEngine.Splines;
using YG;

public class RewardSpeedTelejka : MonoBehaviour
{
    public SplineAnimate splineTelejka;


    private void Start()
    {
        YG2.onRewardAdv += OnRewardedSpeed;
    }

    private void OnDisable()
    {
        YG2.onRewardAdv -= OnRewardedSpeed;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //YG2.onRewardAdv("Speed");
            YG2.RewardedAdvShow("Speed");
        }
    }

    private void OnRewardedSpeed(string obj)
    {   
        if(obj == "Speed")
            splineTelejka.MaxSpeed *= 1.7f; 
    }

}
