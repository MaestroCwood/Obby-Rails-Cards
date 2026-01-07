using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using YG;
using YG.Utils.Pay;
public class InnapTrigger : MonoBehaviour
{
    public GameObject skinObject;
    private IdSkinGameObject id;
    public TextMeshPro textPrice;
    public SpriteRenderer spriteRenderer;
    public int priceCurrentItem;
    public static event Action<IdSkinGameObject, string> OnTryBuySkin;
    public static event Action<IdSkinGameObject> OnSelected;


    string language;

    private void Awake()
    {
       
    }
    private void Start()
    {   
        language = YG2.envir.language;

        id = skinObject.GetComponent<IdSkinGameObject>();

        UpdatePriceLabel();

        if (IsPurchased())
        {
          //  textPrice.text = language == "ru" ? "Куплено" : "BOUGHT";
        } else
        {
           // textPrice.text = $"{priceCurrentItem} RUB";
          
        }
      
    }
    private void UpdatePriceLabel()
    {
        Purchase purchase = YG2.PurchaseByID(id.Id);

        if (IsPurchased())
        {
            textPrice.text = language == "ru" ? "Куплено" : "BOUGHT";
        }
        else
        {
            if (purchase != null)
            {
                textPrice.text = $"{priceCurrentItem} {purchase.priceCurrencyCode}";
                if (!string.IsNullOrEmpty(purchase.currencyImageURL))
                {
                    Load(purchase.currencyImageURL);
                }
                Debug.Log("purchase.priceCurrencyCode PURCHACE");
            }
            else
            {
                // fallback, если товар не найден (например, не настроен в YG)
                Debug.LogWarning($"Purchase with ID '{id.Id}' not found in YG2.purchases");
                textPrice.text = $"{priceCurrentItem} RUB";
            }
        }
    }

    private void OnEnable()
    {
        YG2.onPurchaseSuccess += SuccessPurchased;
        YG2.onGetPayments += UpdatePriceLabel;
    }

    private void OnDisable()
    {
        YG2.onPurchaseSuccess -= SuccessPurchased;
        YG2.onGetPayments -= UpdatePriceLabel;
    }
    private bool IsPurchased()
    {
        string saved = YG2.saves.IDskins[id.Index];
        return !string.IsNullOrEmpty(saved) && saved == id.Id;
    }
    private void SuccessPurchased(string purchasedId)
    {
        if (purchasedId == id.Id)
        {
            textPrice.text = language == "ru" ? "Куплено" : "BOUGHT";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        
        var activeSkin = ActivateInnap.Instance.CurrentSkin;
       
        if (!IsPurchased())
        {
          
                // ещё не куплено → пробуем купить
                OnTryBuySkin?.Invoke(id, id.Id);
            Debug.Log($"Try Buy Skin! ID={id.Id} Index={id.Index}");
        }
        else if (activeSkin != id)
        {
            // куплено, но это не текущий активный скин → применяем
            ActivateInnap.Instance.ApplySkin(id);
            Debug.Log($"Apply already purchased skin: {id.Id} Index={id.Index}");
            OnSelected?.Invoke(id);
        }
        else
        {
            // уже активный скин → ничего не делаем
            Debug.Log($"Skin already active: {id.Id}");
        }
    }


    public void Load(string url)
    {
        if (string.IsNullOrEmpty(url))
            return;

        StartCoroutine(LoadIcon(url));
    }

    private IEnumerator LoadIcon(string url)
    {
        using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(url))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("Currency icon load failed: " + req.error);
                yield break;
            }

            Texture2D tex = DownloadHandlerTexture.GetContent(req);

            Sprite sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f)
            );

            spriteRenderer.sprite = sprite;
        }
    }
}
