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

    private void OnBuyedBridge(ItemRails item)
    {
        Debug.Log("BUY!");
    }
}
