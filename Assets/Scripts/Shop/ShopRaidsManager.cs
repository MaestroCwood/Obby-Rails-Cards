using System.Collections.Generic;
using UnityEngine;

public class ShopRaidsManager : MonoBehaviour
{
    [SerializeField] List<GameObject> raidsList;

    private void OnEnable()
    {
        TriggerActivateShop.OnBuyRails += TriggerActivateShop_OnBuyRaidsCard;
    }

    private void OnDisable()
    {
        TriggerActivateShop.OnBuyRails -= TriggerActivateShop_OnBuyRaidsCard;
    }
    private void TriggerActivateShop_OnBuyRaidsCard(int id)
    {
        // Выключаем всё

        Debug.Log("Triger Sob!!!");
        foreach (var obj in raidsList)
        {
            obj.SetActive(false);
            obj.GetComponent<IsSelectedRaids>().isSelctedCard =false;
        }
            

        // Ищем объект по ID
        foreach (var obj in raidsList)
        {
            var comp = obj.GetComponent<ItemIdmoust>();
            var sel = obj.GetComponent<IsSelectedRaids>();
            if (comp != null && comp.ID == id)
            {
                //obj.SetActive(true);
                sel.isSelctedCard = true;

                Debug.Log("Triger Sob!!! +" + comp.ID );
                return;
            }
        }

        Debug.LogWarning("Не найден объект с ID: " + id);

    }
}

