using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsResolution : MonoBehaviour
{
    public TMP_Dropdown ResDropDown;
    public Toggle toggleFullScreen;

    Resolution[] AllResolution;
    int SellectedResolution;
    List<Resolution> SelcdtedResolutionsList = new List<Resolution>();
    bool isFullScreen;
    private void Start()
    {
        AllResolution = Screen.resolutions;

        List<string> resolutionStringList = new List<string>();
        string newRes;
        foreach (Resolution res in AllResolution)
        {
            newRes = res.width.ToString() + " x " + res.height.ToString();
            if(!resolutionStringList.Contains(newRes) )
            {
                resolutionStringList.Add(newRes);
                SelcdtedResolutionsList.Add(res);
            }
           
        }

        ResDropDown.AddOptions(resolutionStringList);
    }


    public void ChangeResol()
    {
        SellectedResolution = ResDropDown.value;
        Screen.SetResolution(SelcdtedResolutionsList[SellectedResolution].width, SelcdtedResolutionsList[SellectedResolution].height, isFullScreen);
    }


    public void IsChangeFullScreen()
    {
        isFullScreen = toggleFullScreen.isOn;
        Screen.SetResolution(SelcdtedResolutionsList[SellectedResolution].width, SelcdtedResolutionsList[SellectedResolution].height, isFullScreen);
    }
}
