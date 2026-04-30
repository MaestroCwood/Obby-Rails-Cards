using System;
using UnityEngine;
//using YG;

public class BikeRewarded : MonoBehaviour
{
    public enum BikeType
    {
        Skate, Scooter, Bicycle
    }
    public GameObject iconRewarded;
    public BikeType bikeType;

    public static Action<BikeType> OnBikeRewarded;

    bool isRewardedActive = false;

    private void OnEnable()
    {
      //  YG2.onRewardAdv += OnRewardedShow;
    }

    private void OnDisable()
    {
        //YG2.onRewardAdv -= OnRewardedShow;
    }

    private void OnRewardedShow(string obj)
    {
        if (obj == bikeType.ToString())
        {
            isRewardedActive = true;
            iconRewarded.SetActive(false);
        }

        Debug.Log("ON REWARDED!!!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isRewardedActive)
            {
                switch(bikeType)
                {
                    case BikeType.Skate:
                     //   YG2.RewardedAdvShow("Skate");
                        break;
                    case BikeType.Scooter:
                       // YG2.RewardedAdvShow("Scooter");
                        break;
                    case BikeType.Bicycle:
                       // YG2.RewardedAdvShow("Bicycle");
                        break;
                }
            }
            else 
            {
                OnBikeRewarded?.Invoke(bikeType);
                
            }
        }
    }
}
