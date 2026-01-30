using System;
using TMPro;
using UnityEngine;
using YG;

public class VisualRaids : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textCountRaid;
    [SerializeField] TextMeshProUGUI textBtnMakeUI;
    [SerializeField] MakeRaidsManager makeRaidsManager;

    string language;
    private void Awake()
    {
        makeRaidsManager.OnMakeRaids += MakeRaidsManager_OnMakeRaids;
        makeRaidsManager.OnIsStartMake += MakeRaidsManager_OnIsStartMake;
       
    }

    private void Start()
    {
        language = YG2.envir.language;
    }
    private void OnDisable()
    {
        makeRaidsManager.OnMakeRaids -= MakeRaidsManager_OnMakeRaids;
        makeRaidsManager.OnIsStartMake -= MakeRaidsManager_OnIsStartMake;
    }


    private void MakeRaidsManager_OnMakeRaids(int currentEl, int maxEl)
    {
        textCountRaid.text = $"{RailsTxt()}: {currentEl}/{maxEl}";
        
    }

    private void MakeRaidsManager_OnIsStartMake(bool obj)
    {
        textBtnMakeUI.text = MakeOrStopTxt(obj);
        Debug.Log(obj + "MakeRaidsManager_OnIsStartMake");
    }

    string MakeOrStopTxt(bool ismake)
    {
        string ru = language == "ru" ? "—“–Œ»“‹ –≈À‹—€" : "BUILD RAILS";
        if (!ismake)
        {
            ru = language == "ru" ? "—“–Œ»“‹ –≈À‹—€" : "MAKE READ <Color=red>";
            return ru ;
        }
        else
        {
            {
                ru = ru = language == "ru" ? "Œ—“¿ÕŒ¬»“‹ —“–Œ… ”" : "STOP CONSTRUCTION";
                return ru;
            }
        }
    }

    string RailsTxt()
    {
        string txt = language == "ru" ? "–ÂÎ¸Ò˚" : "Rails";
        return txt;
    }
    
}
