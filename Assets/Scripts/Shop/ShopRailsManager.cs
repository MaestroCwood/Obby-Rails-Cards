
using UnityEngine;
using UnityEngine.Splines;

public class ShopRailsManager : MonoBehaviour
{


    [SerializeField] SplineInstantiate splineInstantiatePlayer;

  

    private void OnEnable()
    {
        GameEvents.OnBuyedRails += OnBuedRails;
    }

    private void OnDisable()
    {
        GameEvents.OnBuyedRails -= OnBuedRails;
    }


    void OnBuedRails(ItemRails id)
    {
       
        if (!id.isBued) return;
        Debug.Log("OnBuerd ID+" + id);
        var items = splineInstantiatePlayer.itemsToInstantiate;

        for (int i = 0; i < items.Length; i++)
        {
            items[i].Probability = (i == id.IDBridge) ? 1f : 0f;
        }

        splineInstantiatePlayer.itemsToInstantiate = items;
        splineInstantiatePlayer.UpdateInstances();


    }
}
