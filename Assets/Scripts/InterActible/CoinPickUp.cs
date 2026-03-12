using System.Collections;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    public int minAddCoin = 1;
    public int maxAddCoin = 100;

    public float delayToDeactivate = 10f;

    SpriteRenderer meshCoin;
    BoxCollider colliderCoin;


    private void Awake()
    {
        meshCoin = GetComponent<SpriteRenderer>();
        colliderCoin = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AddCpoinPlayer();
            StartCoroutine(DelayToDeactivateCoin());
            Debug.Log("TRIGGER COIN!");
        }
    }


    void AddCpoinPlayer()
    {
        int rand = Random.Range(minAddCoin, maxAddCoin);
        GameManager.instance.AddCoin(rand);
        AudioManager.instance.PlayFx(5);
        CoinEffect.Play(rand);
    }

    void DeactivateCoin()
    {
        meshCoin.enabled = false;
        colliderCoin.enabled = false;
    }

    void ActivateCoin()
    {
        meshCoin.enabled = true;
        colliderCoin.enabled = true;
    }


    IEnumerator DelayToDeactivateCoin()
    {
        DeactivateCoin();
        yield return new WaitForSeconds(delayToDeactivate);
        ActivateCoin();
    }
}
