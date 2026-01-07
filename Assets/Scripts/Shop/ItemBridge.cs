using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemBridge : MonoBehaviour
{
    public bool isBued = false;

    public bool isSelected = false;
    public int IDBridge;

    public Sprite icoCurrentBridgeElement;

    [SerializeField] GameObject buyOkObj;
    [SerializeField] GameObject buyFailedObj;
    [SerializeField] GameObject priceTxtobj;



    public int priceBridge;

    Coroutine deactivate;

    const string prefix = "BridgesElement_";
    private void Start()
    {
        string key = $"{prefix}{IDBridge}";
        // 1. Если сохранения нет — сохранить дефолт из инспектора
        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetInt(key, isBued ? 1 : 0);
        }
        else
        {
            isBued = PlayerPrefs.GetInt(key) == 1;
        }

        // 2. UI для купленного
        if (isBued)
        {
            priceTxtobj.SetActive(false);
            buyOkObj.SetActive(true);
        }
        else
        {
            priceTxtobj.GetComponent<TextMeshPro>().text = priceBridge.ToString();
        }

        // 3. Подписки
        GameEvents.OnBuyedRails += OnBuedBridge;
        GameEvents.OnSelectedBridge += OnSeletedBridge;

        // 4. ЕСЛИ ЭТО ДЕФОЛТНЫЙ ВЫБРАННЫЙ ЭЛЕМЕНТ
        if (isBued && isSelected)
        {
            buyOkObj.GetComponent<Image>().color = Color.yellow;
            GameEvents.OnSelectedBridge?.Invoke(IDBridge, icoCurrentBridgeElement);
        }
    }

    private void OnDisable()
    {
        GameEvents.OnBuyedRails -= OnBuedBridge;
        GameEvents.OnSelectedBridge -= OnSeletedBridge;
    }

    private void OnSeletedBridge(int ID, Sprite ico)
    {
        if (IDBridge != ID)
        {
            isSelected = false;
            buyOkObj.GetComponent<Image>().color = Color.green;
            return;
        }

        isSelected = true;
        buyOkObj.GetComponent<Image>().color = Color.yellow;

        Debug.Log("Selected!");
    }

    private void OnBuedBridge(int obj, int ID)
    {
        if(IDBridge != ID) return;

        if(isBued && !isSelected)
        {   
            isSelected = true;
            GameEvents.OnSelectedBridge?.Invoke(IDBridge, icoCurrentBridgeElement);
            buyOkObj.GetComponent<Image>().color = Color.yellow;
        }

        if (isBued ) return;
        if(GameManager.instance.curentCountCoin >= priceBridge && !isBued)
        {
            isBued=true;
            priceTxtobj.SetActive(false);
            buyOkObj.SetActive(true);
            PlayerPrefs.SetInt($"{prefix}{IDBridge}", 1);
            GameManager.instance.DecreamenteCoin(priceBridge);
            GameEvents.OnSelectedBridge?.Invoke(IDBridge, icoCurrentBridgeElement);
            Debug.Log("bued!!!!");
        } else if(!isBued)
        {
            priceTxtobj.SetActive(false);
            buyFailedObj.SetActive(true);
            deactivate = StartCoroutine(DeactivateDelay());
            Debug.Log("NO MANY!!!");
        }
    }



    IEnumerator DeactivateDelay()
    {
        yield return new WaitForSeconds(3f);
        buyFailedObj.SetActive(false);
        priceTxtobj.SetActive(true);
    }


    void SelectedObj()
    {

    }
}
