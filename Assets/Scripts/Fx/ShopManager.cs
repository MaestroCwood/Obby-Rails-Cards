using System;
using UnityEngine;

public class ShopManager : MonoBehaviour
{




    private void Start()
    {
        GameEvents.OnBuyedRails += OnBuyedBridge;
    }


    private void OnDisable()
    {
        GameEvents.OnBuyedRails -= OnBuyedBridge;
    }

    private void OnBuyedBridge(int obj, int ID)
    {
        Debug.Log("BUY!");
    }
}
