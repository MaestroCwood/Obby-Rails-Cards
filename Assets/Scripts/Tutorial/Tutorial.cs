using System;
using System.Collections;
using TMPro;
using UnityEngine;
using YG;

public class Tutorial : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textTurorial;
    [SerializeField] GameObject arrow3d;
    [SerializeField] AudioClip[] audioClips;

    [SerializeField] AudioSource audioSource;
    [SerializeField] PathArrowGuide pathArrowGuide;
    string language;


    private void Start()
    {
        if (PlayerPrefs.GetInt("Tutor") == 1)
        {
            DestrouObj();
        }
        language = YG2.envir.language;

        Invoke("DelayStart", 3f);
    }

    private void OnEnable()
    {
        GameEvents.OnPickUpBrainRot += OnPickUpBrain;
    }

    private void OnDisable()
    {
        GameEvents.OnPickUpBrainRot -= OnPickUpBrain;
    }

    private void OnPickUpBrain(bool pickup)
    {
        if (pickup)
        {
            textTurorial.text = language == "ru" ? "Теперь беги на базу!" : "Now run to the base!";
            Destroy(arrow3d);
        }
        else
        {
            textTurorial.text = language == "ru" ? "Попробуй собрать всех браинротов!" : "Try to collect all the Brainrotts!";
            PlaySoundTutor(1);
            DiactivateTutor();

        }
    }

    void PlaySoundTutor(int index)
    {
        audioSource.PlayOneShot(audioClips[index]);
    }

    void DelayStart()
    {
        PlaySoundTutor(0);
    }

    void DiactivateTutor()
    {
        StartCoroutine(Delay());
        PlayerPrefs.SetInt("Tutor", 1);
    }


    IEnumerator Delay()
    {
        pathArrowGuide.ClearArrows();
        pathArrowGuide.isActive = false;

        yield return new WaitForSeconds(10f);

        Destroy(gameObject);
    }

    void DestrouObj()
    {
        pathArrowGuide.ClearArrows();
        pathArrowGuide.isActive = false;
        Destroy(arrow3d);
        Destroy(gameObject);
    }
}
