using System;
using TMPro;
using UnityEngine;
using YG;

public class RewardedLinitRails : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI limitTxt;
   [SerializeField] MakeRaidsManager makeRaidsManager;
    
    private void Start()
    {

        Invoke("SetTxt", 1f);
    }


    private void OnEnable()
    {
        YG2.onRewardAdv += OnRewardedShow;
    }

    private void OnDisable()
    {
        YG2.onRewardAdv -= OnRewardedShow;
    }

 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            YG2.RewardedAdvShow("Limit");
        }
    }

    private void OnRewardedShow(string obj)
    {
        if(obj == "Limit")
            Invoke("SetTxt", .3f);
    }
    void SetTxt()
    {
        int currentLimit = makeRaidsManager.maxElement;
        int nexLimit = makeRaidsManager.maxElement + makeRaidsManager.addRewardedLimit;
        limitTxt.text = $"{currentLimit}=> <color=red>{nexLimit}</color> ";
    }
}
