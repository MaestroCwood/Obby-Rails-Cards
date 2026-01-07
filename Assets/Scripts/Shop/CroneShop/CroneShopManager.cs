using System.Collections.Generic;
using UnityEngine;

public class CroneShopManager : MonoBehaviour
{
    [SerializeField] List<GameObject> croneList;
    //[SerializeField] TriggerActivateShop triggerShop;

    private void OnEnable()
    {
        TriggerActivateShop.OnBuyCrone += TriggerActivateShop_OnBuyCrone;
    }


    private void OnDisable()
    {
        TriggerActivateShop.OnBuyCrone -= TriggerActivateShop_OnBuyCrone;
    }

    private void TriggerActivateShop_OnBuyCrone(int obj)
    {
        for(int i = 0; i < croneList.Count; i++)
        {
            croneList[i].SetActive(false);
        }

        croneList[obj].SetActive(true);

        Debug.Log("OnBuyCrone!");
    }
}
