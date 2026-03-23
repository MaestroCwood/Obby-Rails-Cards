using UnityEngine;

public class RaidCartManager : MonoBehaviour
{
    [SerializeField] GameObject[] railCarts; // все вагонетки в сцене

    private void OnEnable()
    {
        // Подписываемся на событие покупки вагонетки
        TriggerActivateShop.OnBuyRails += OnBuyRails;
    }

    private void OnDisable()
    {
        TriggerActivateShop.OnBuyRails -= OnBuyRails;
    }

    private void OnBuyRails(int purchasedId)
    {
        bool found = false;

        // Проходим по всем вагонеткам
        foreach (var rail in railCarts)
        {
            var selected = rail.GetComponent<IsSelectedRaids>();
            if (selected == null) continue;

            // Если ID вагонетки совпадает с купленным — активируем, иначе деактивируем
            if (selected.idRails == purchasedId)
            {
                selected.isSelctedCard = true;
                found = true;
                Debug.Log($"Вагонетка с ID {purchasedId} выбрана");
            }
            else
            {
                selected.isSelctedCard = false;
            }
        }

        if (!found)
        {
            Debug.LogWarning($"Вагонетка с ID {purchasedId} не найдена!");
        }
    }
}