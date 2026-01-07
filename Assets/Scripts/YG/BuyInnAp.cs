using UnityEngine;
using YG;
using YG.Utils.Pay;

public class BuyInnAp : MonoBehaviour
{
    private IdSkinGameObject _pendingSkin;
    private string _pendingPaymentId;

    private void OnEnable()
    {
        InnapTrigger.OnTryBuySkin += TryBuy;
        YG2.onPurchaseSuccess += SuccessPurchased;
        YG2.onPurchaseFailed += FailedPurchased;
    }

    private void OnDisable()
    {
        InnapTrigger.OnTryBuySkin -= TryBuy;
        YG2.onPurchaseSuccess -= SuccessPurchased;
        YG2.onPurchaseFailed -= FailedPurchased;
    }

    private void TryBuy(IdSkinGameObject skin, string paymentId)
    {
        _pendingSkin = skin;
        _pendingPaymentId = paymentId;
        YG2.BuyPayments(paymentId);
    }

    private void SuccessPurchased(string id)
    {
        // 1️⃣ если это обычная покупка
        if (_pendingSkin != null && id == _pendingPaymentId)
        {
            SaveAndApply(_pendingSkin, id);
            return;
        }

        // 2️⃣ если это консумирование (pending = null)
        var skin = FindSkinById(id);
        if (skin != null)
        {
            SaveAndApply(skin, id);
        }
    }

    private void SaveAndApply(IdSkinGameObject skin, string id)
    {
        YG2.saves.IDskins[skin.Index] = id;
        YG2.SaveProgress();

        ActivateInnap.Instance.ApplySkin(skin);
        YG2.ConsumePurchaseByID(id);
    }
    private IdSkinGameObject FindSkinById(string id)
    {
        foreach (var skin in ActivateInnap.Instance.skins)
        {
            if (skin.Id == id)
                return skin;
        }
        return null;
    }


    private void FailedPurchased(string id)
    {
        Debug.Log($"Покупка отменена или не прошла: {id}");
    }
}
