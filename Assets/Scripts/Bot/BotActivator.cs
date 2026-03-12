using System.Collections;
using UnityEngine;

public class BotActivator : MonoBehaviour
{
    [SerializeField] GameObject[] botsObj;

    [SerializeField] float minDelayActivator;
    [SerializeField] float maxDelayActivator;
    [SerializeField] GameObject[] brainrotsMesh;
    private void Start()
    {
        StartCoroutine(Activator());  
    }


    IEnumerator Activator()
    {
        for(int i = 0; i < botsObj.Length; i++)
        {   
            botsObj[i].transform.SetParent(null, true);
            botsObj[i].SetActive(true);
            GameObject go = Instantiate(brainrotsMesh[RandIndex()]);
            go.transform.SetParent(botsObj[i].transform);
            go.transform.localPosition = new Vector3(-.5f, 1, -3f);
            go.gameObject.SetActive(false);
            yield return new WaitForSeconds(Random.Range(minDelayActivator, maxDelayActivator));
            go.gameObject.SetActive(true);
        }

        yield return null;
    }


    int RandIndex()
    {
        int index = Random.Range(0,brainrotsMesh.Length);
        return index;
    }


}
