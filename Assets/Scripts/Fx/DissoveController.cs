using UnityEngine;
//using UnityEngine.Splines.Interpolators;
using UnityEngine.UI;

public class DissoveController : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Image fillImageBuy;


    [SerializeField] float delayToBuyEvent = 3f;


    private LTDescr _currentTween;
    Material dissoveMat;
    private void Start()
    {
        dissoveMat = meshRenderer.material;
    }
    private void OnTriggerEnter(Collider other)
    {
       if(other.tag != "Player") return;

        FillBuyImage();
        FillDissoveEffect();


    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag != "Player") return;
        ClearFillBuyImage();
        ClearFillDissoveEffect();

    }


    void FillDissoveEffect()
    {
        
        if (dissoveMat != null)
        {
            // Получаем текущее значение (на случай, если оно уже частично изменено)
            float startValue = dissoveMat.GetFloat("_RangeDissove");
            float endValue = 1; // целевое значение


            LeanTween.value(gameObject, (float val) =>
            {
                dissoveMat.SetFloat("_RangeDissove", val);
            }, startValue, endValue, 1.0f)
              .setEase(LeanTweenType.easeInOutQuad);
        }
    }

    void ClearFillDissoveEffect()
    {
        
        if (dissoveMat != null)
        {
            // Получаем текущее значение (на случай, если оно уже частично изменено)
            float startValue = dissoveMat.GetFloat("_RangeDissove");
            float endValue = -1;


            LeanTween.value(gameObject, (float val) =>
            {
                dissoveMat.SetFloat("_RangeDissove", val);
            }, startValue, endValue, 1.0f) //
              .setEase(LeanTweenType.easeInOutQuad);
        }
    }


    void FillBuyImage()
    {
        float startValue = 0f;
        float endValue = 1;

        int currentPrice = GetComponentInParent<ItemBridge>().priceBridge;
        int currentID = GetComponentInParent<ItemBridge>().IDBridge;
        _currentTween = LeanTween.value(gameObject, (float val) =>
        {
            fillImageBuy.fillAmount = val;
        }, startValue, endValue, delayToBuyEvent)
          .setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
          {
              GameEvents.OnBuyedRails?.Invoke(currentPrice, currentID);
          });

    }

    void ClearFillBuyImage()
    {
        LeanTween.cancel(_currentTween.uniqueId);
        fillImageBuy.fillAmount = 0;
    }

}
