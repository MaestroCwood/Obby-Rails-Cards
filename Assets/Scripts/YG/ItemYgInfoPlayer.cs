using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemYgInfoPlayer : MonoBehaviour
{
    public TextMeshProUGUI playerNameTxt;
    public TextMeshProUGUI totalTimePlayer;
    public TextMeshProUGUI currentMestoPlayer;
    public Image playerPhoto;


    public void SetName(string name) => playerNameTxt.text = name;
    public void SetTime(string time) => totalTimePlayer.text = time;

    public void SetMest(string mesto) => currentMestoPlayer.text = mesto;

    public void SetPhoto(Sprite photo) => playerPhoto.sprite = photo;
}
