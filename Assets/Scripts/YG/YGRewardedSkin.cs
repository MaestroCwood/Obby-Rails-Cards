using System;
using UnityEngine;
using YG;

public class YGRewardedSkin : MonoBehaviour
{
    [SerializeField] Animator animatorPlayer;
    [SerializeField] Avatar avatar;
    [SerializeField] GameObject skinHuggy;

    [SerializeField] GameObject[] meshOtherPLayers;
    [SerializeField] GameObject ico;

    public bool isRewardedComlited = false;

    public enum Rewardedskin
    {
        Huggi, Banana, Andrew
    }

    public Rewardedskin rewarded = Rewardedskin.Huggi;

    private void Start()
    {
        ico.SetActive(!isRewardedComlited);
        
    }

    private void OnGetSK()
    {
        isRewardedComlited = YG2.saves.skinHuggi == 0 ? false : true;
    }

    private void OnEnable()
    {
        YG2.onRewardAdv += OnRewarded;
        YG2.onGetSDKData += OnGetSK;
    }

    private void OnDisable()
    {
        YG2.onRewardAdv -= OnRewarded;
        YG2.onGetSDKData -= OnGetSK;
    }

    private void OnRewarded(string obj)
    {
        if (obj != rewarded.ToString())
            return;

        ActivateSkin();
        isRewardedComlited = true;

        switch (rewarded)
        {
            case Rewardedskin.Huggi:
                YG2.saves.skinHuggi = 1;
                break;

            case Rewardedskin.Banana:
                YG2.saves.skinBanana = 1;
                break;
            case Rewardedskin.Andrew:
                YG2.saves.skinBanana = 1;
                break;
        }

        YG2.SaveProgress();
        ico.SetActive(!isRewardedComlited);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isRewardedComlited)
        {
            switch (rewarded)
            {
                case Rewardedskin.Banana:
                    YG2.RewardedAdvShow("Banana");
                    break;
                case Rewardedskin.Huggi:
                    YG2.RewardedAdvShow("Huggi");
                    break;
                case Rewardedskin.Andrew:
                    YG2.RewardedAdvShow("Andrew");
                    break;
            }
        } 
        else if (isRewardedComlited)
        {
            ActivateSkin();
        }

        
    }


    void ActivateSkin()
    {
        for(int i = 0; i < meshOtherPLayers.Length; i++)
        {
            meshOtherPLayers[i].SetActive(false);

        }

        animatorPlayer.avatar = avatar;
        skinHuggy.SetActive(true);
        
        Debug.Log("TRIGGER!!!");
    }
}
