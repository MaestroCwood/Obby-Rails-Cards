using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum TypeItem
{
    Crone,
    Moust,
    Vest
}
public class TriggerActivateShop : MonoBehaviour
{
    [SerializeField] GameObject buyOkObj;
    
    [SerializeField] CroneItem croneItenScrObject;
    [SerializeField] Image fillImageBuy;
    [SerializeField] Image frame;
    [SerializeField] TextMeshProUGUI priceCroneTxt;
    [SerializeField] float delayToBuyEvent = 3f;

    private LTDescr _currentTween;

    string prefix = "CroneItem_";
    public static event Action<int> OnBuyCrone;
    public static event Action<int> OnBuyMust;
    public static event Action<int> OnBuyVest;

    public bool isBuyed = false;


    Color startFrameColor;
   

    public TypeItem typeItem;
    private void Start()
    {
        //priceCroneTxt.text = priceCrone.ToString();
        priceCroneTxt.text = croneItenScrObject.priceCrone.ToString();

        // „итаем сохранЄнную покупку (0 Ч не куплено, 1 Ч куплено)
        isBuyed = PlayerPrefs.GetInt($"{prefix}{croneItenScrObject.IDCrone}", 0) == 1;

        // ≈сли уже куплено Ч скрываем цену и показываем галочку
        if (isBuyed)
        {
            ChangeBuyToSelectObj();
        }


        startFrameColor = frame.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        FillBuyImage();
      

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;

        ClearFillBuyImage();
        

    }
    void FillBuyImage()
    {
        float startValue = 0f;
        float endValue = 1;

        _currentTween = LeanTween.value(gameObject, (float val) =>
        {
            fillImageBuy.fillAmount = val;
        }, startValue, endValue, delayToBuyEvent)
          .setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
          {
              // TO DOO
             
              if (GameManager.instance.curentCountCoin >= croneItenScrObject.priceCrone && !isBuyed)
              {   
                  isBuyed = true;
                  GameManager.instance.DecreamenteCoin(croneItenScrObject.priceCrone);
                  //OnBuyCrone?.Invoke(croneItenScrObject.IDCrone);
                  // Checkitem(typeItem);
                  if (TypeItem.Crone == typeItem)
                      OnBuyCrone?.Invoke(croneItenScrObject.IDCrone);
                  else if (TypeItem.Moust == typeItem)
                      OnBuyMust?.Invoke(croneItenScrObject.IDCrone);
                  else if (TypeItem.Vest == typeItem)
                      OnBuyVest?.Invoke(croneItenScrObject.IDCrone);

                  ChangeBuyToSelectObj();
                  PlayerPrefs.SetInt($"{prefix}{croneItenScrObject.IDCrone}", 1);
                  Debug.Log("BUY!" +  typeItem + croneItenScrObject.IDCrone);
              } else if(GameManager.instance.curentCountCoin < croneItenScrObject.priceCrone && !isBuyed)
              {
                  DontHaveMany();
              }

              if (isBuyed)
              {     
                  if(TypeItem.Crone == typeItem)
                    OnBuyCrone?.Invoke(croneItenScrObject.IDCrone);
                  else if(TypeItem.Moust == typeItem)
                      OnBuyMust?.Invoke(croneItenScrObject.IDCrone);
                  else if (TypeItem.Vest == typeItem)
                      OnBuyVest?.Invoke(croneItenScrObject.IDCrone);
                  //Checkitem(typeItem);
              }
          });
        
    }


    void DontHaveMany()
    {
        frame.color = Color.red;
    }

 
    void ChangeBuyToSelectObj()
    {
        buyOkObj.SetActive(true);
        priceCroneTxt.gameObject.SetActive(false);
        fillImageBuy.fillAmount = 1f;
        frame.color = Color.yellow; // например выделить UI

    }
    void ClearFillBuyImage()
    {
        LeanTween.cancel(_currentTween.uniqueId);
        fillImageBuy.fillAmount = 0;
        frame.color = startFrameColor;
    }

    void Checkitem(TypeItem type)
    {
        switch(type)
        {
            case TypeItem.Crone:
                OnBuyCrone?.Invoke(croneItenScrObject.IDCrone);
                break;
            case TypeItem.Moust:
                OnBuyMust?.Invoke(croneItenScrObject.IDCrone);
                break;
        }

    }

}
