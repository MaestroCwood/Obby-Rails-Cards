using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] Image offSondIco;

    [SerializeField] Toggle toggleSound;
    [SerializeField] AudioSource musicSource;


    public void SetActive()
    {
        bool isActive = toggleSound.isOn;
        offSondIco.enabled = !isActive;
        musicSource.mute = !isActive;
    }
}
