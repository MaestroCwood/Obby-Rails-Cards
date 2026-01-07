using System.Collections.Generic;
using UnityEngine;

public class VestShopManager : MonoBehaviour
{
    [SerializeField] List<GameObject> moustList;
    //[SerializeField] TriggerActivateShop triggerShop;
    ItemIdmoust[] itemIdmoust;
    private void OnEnable()
    {
        TriggerActivateShop.OnBuyVest += TriggerActivateShop_OnBuyVest;
    }


    private void OnDisable()
    {
        TriggerActivateShop.OnBuyVest -= TriggerActivateShop_OnBuyVest;
    }

    private void TriggerActivateShop_OnBuyVest(int id)
    {
        // Выключаем всё
        foreach (var obj in moustList)
            obj.SetActive(false);

        // Ищем объект по ID
        foreach (var obj in moustList)
        {
            var comp = obj.GetComponent<ItemIdmoust>();
            if (comp != null && comp.ID == id)
            {
                obj.SetActive(true);
                return;
            }
        }

        Debug.LogWarning("Не найден объект с ID: " + id);

    }
}
