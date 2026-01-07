using System;
using UnityEngine;

public class OnDestroyFx : MonoBehaviour
{
    [SerializeField] GameObject fxOnDestroy;




    private void OnEnable()
    {
        GameEvents.OnDestrouBridgeElement += OnDestroyBridgeElement;
    }

    private void OnDisable()
    {
        GameEvents.OnDestrouBridgeElement -= OnDestroyBridgeElement;
    }

    private void OnDestroyBridgeElement(Vector3 position)
    {
        Instantiate(fxOnDestroy, position, Quaternion.identity);
    }
}
