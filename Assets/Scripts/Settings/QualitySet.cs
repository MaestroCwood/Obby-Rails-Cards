
using UnityEngine;

using YG;

public class QualitySet : MonoBehaviour
{
    ParticleSystem[] allFx;

    public GameObject[] objectDeactivate;
    

    bool isMobileDivece;
    private void Awake()
    {
        allFx = FindObjectsByType<ParticleSystem>(FindObjectsSortMode.None);

        Application.targetFrameRate = 60;

    }


    private void Start()
    {
        isMobileDivece = YG2.envir.isMobile;

        if (isMobileDivece)
        {
           
            QualitySettings.SetQualityLevel(0);
            SetLowQuality();
            Invoke("DeactivateCam", 1f);
        }
      
    }
    void SetLowQuality()
    {
        foreach (ParticleSystem partycl in allFx)
        {
            var main = partycl.main;
            main.maxParticles = 10;
        }

        
    }

    void DeactivateCam()
    {
        foreach (var obj in objectDeactivate)
        {
            obj.SetActive(false);
        }
    }
}
