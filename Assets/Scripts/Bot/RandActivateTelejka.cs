using System.Collections.Generic;
using UnityEngine;

public class RandActivateTelejka : MonoBehaviour
{
    public List<GameObject> telejka = new List<GameObject>();



    private void Start()
    {

        
    }


    int RandomInexe()
    {
        int rand = Random.Range(0, telejka.Count - 1);
        return rand;
    }


    public void ActivateTel()
    {
        foreach (var item in telejka)
        {
            item.SetActive(false);
        }
        telejka[RandomInexe()].SetActive(true);
    }

    public void DeactivateTelejka()
    {
        foreach (var item in telejka)
        {
            item.SetActive(false);
        }
    }
}
