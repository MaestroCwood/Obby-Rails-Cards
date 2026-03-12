using System;
using UnityEngine;

public class PickUpAndDrop : MonoBehaviour
{
    [SerializeField] Transform holdBrainr;
    [SerializeField] GameObject[] dropPosition;
    [SerializeField] GameObject trailFx;
    bool isPickUp = false;

    GameObject currentObj;
    Vector3 lastPosBrain;
    BasePoint currentBasePoint;
    private void OnEnable()
    {
        GameEvents.OnDamageToPlayer += OnDamagePlayer;
    }

    private void OnDisable()
    {
        GameEvents.OnDamageToPlayer -= OnDamagePlayer;
    }

    private void OnDamagePlayer()
    {   
        if (currentObj == null) return;
        currentObj.transform.SetParent(null);
        lastPosBrain = currentObj.transform.position; 
        isPickUp = false;
        currentObj.transform.position = lastPosBrain;
        currentObj = null;
        GameEvents.OnPickUpBrainRot?.Invoke(false); 

    }

    private void OnTriggerEnter(Collider other)
    {   
        BrainrotItem brainrot = other.GetComponent<BrainrotItem>();
        if (other.CompareTag("Brain") && !isPickUp && !brainrot.isBasePosition)
        {
            PickUp(other);
            GameEvents.OnPickUpBrainRot?.Invoke(true);
        }
        BasePoint basePoint = other.GetComponent<BasePoint>();
        if (other.CompareTag("Base") && isPickUp && currentObj != null)
        {
            currentBasePoint = basePoint;
            Drop();
            GameEvents.OnPickUpBrainRot?.Invoke(false);

        }
    }


    void PickUp(Collider oth)
    {
        isPickUp = true;
        lastPosBrain = oth.transform.position;
        oth.transform.SetParent(holdBrainr);
        oth.transform.localPosition = Vector3.zero;
        trailFx.transform.SetParent(oth.transform);
        trailFx.transform.localPosition = Vector3.zero;
        currentObj = oth.gameObject;
    }

    void Drop()
    {
        isPickUp = false;
        currentObj.transform.SetParent(null);
        Transform dropPos = PointDrop();
        if (dropPos == null)
        {
            Debug.Log("Нет свободной базы для сброса");
            return;
        }
        currentObj.transform.SetParent(dropPos);
        currentObj.transform.position = dropPos.position;
        currentObj.GetComponent<BrainrotItem>().SetBasePosition();
        currentObj = null;
    }

    Transform PointDrop()
    {
        for (int i = 0; i < dropPosition.Length; i++)
        {
            BasePoint basePoint = dropPosition[i].GetComponent<BasePoint>();
            if (basePoint != null && !basePoint.isOcupied) 
            {
                Transform dropPos = dropPosition[i].transform;
                basePoint.Ocupeited();
                return dropPos;
            } else continue;
        }
        return null;
    }
}
