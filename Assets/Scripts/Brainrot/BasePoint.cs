using UnityEngine;

public class BasePoint : MonoBehaviour
{
    public bool isOcupied { get; private set; }

    public int baseID;

    const string PREFIX = "Base";
    public void Ocupeited()
    {
        isOcupied = true;
        Save();
    }
    private void Start()
    {
        int checkSave = PlayerPrefs.GetInt(PREFIX+ baseID);
        if(checkSave == 1)
        {
            Ocupeited();
        }
    }
   
    public void Save()
    {
        PlayerPrefs.SetInt(PREFIX+baseID, 1);
        PlayerPrefs.Save();
    }

}
