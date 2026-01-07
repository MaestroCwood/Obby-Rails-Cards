using System.Collections.Generic;
using UnityEngine;

public class MoustShopManager : MonoBehaviour
{
    [SerializeField] List<GameObject> moustList;
    //[SerializeField] TriggerActivateShop triggerShop;
    ItemIdmoust[] itemIdmoust;
    private void OnEnable()
    {
        TriggerActivateShop.OnBuyMust += TriggerActivateShop_OnBuyCrone;
    }


    private void OnDisable()
    {
        TriggerActivateShop.OnBuyMust -= TriggerActivateShop_OnBuyCrone;
    }

    private void TriggerActivateShop_OnBuyCrone(int id)
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
