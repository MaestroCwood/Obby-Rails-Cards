using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum TypeNagrada
{
    Coin,
    Pet
}
public class TimerRewarded : MonoBehaviour
{
    [SerializeField] float totalTime;
    [SerializeField] int countAddNagradaOrIdPet;
 
    [SerializeField] TextMeshProUGUI textTimer;
    [SerializeField] TextMeshProUGUI textGetPets;
    [SerializeField] Button gettedButton;
    [SerializeField] GameObject gettedImage;
    [SerializeField] TextMeshProUGUI textValue;
    [SerializeField] ReturnTime returnTime;
     bool getted = false;
    bool unlocked = false;
    bool hasStarted = false;
    private double startTime;

    public TypeNagrada typeNagrada;
  
    void Start()
    {

        if (typeNagrada == TypeNagrada.Coin)
            textValue.text = countAddNagradaOrIdPet.ToString();
        
       
    }

    private void OnEnable()
    {
        if (!hasStarted)
        {
            startTime = returnTime.TimeSessionAll();
            hasStarted = true;
        }
        StartCoroutine(TimerReward());
    }

    private void OnDisable()
    {   
        StopAllCoroutines();
    }


    public IEnumerator TimerReward()
    {
       
        while (true)
        {
            //slolkoProshloTime -= Time.deltaTime;
            double elapsed = returnTime.TimeSessionAll() - startTime;
            double remaining = totalTime - elapsed;
            if (remaining <= 0)
            {
                Unlocked();
                yield break;
            }

            int hours = Mathf.FloorToInt((float)(remaining / 3600));
            int minutes = Mathf.FloorToInt((float)((remaining % 3600) / 60));
            int seconds = Mathf.FloorToInt((float)(remaining % 60));
            textTimer.text = $"{hours:00}:{minutes:00}:{seconds:00}";


            textTimer.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

            yield return new WaitForSecondsRealtime(0.2f); // обновляем 5 раз в секунду
        }

       
    }

    public void GettedRewarded()
    {
        if (unlocked && !getted)
        {
            getted = true;

            textGetPets.text = "Получено";
            gettedButton.interactable = false;
            gettedImage.SetActive(true);

            GameEvents.OnTimeRewardedComplited?.Invoke(countAddNagradaOrIdPet, typeNagrada);

        }
    }

    void Unlocked()
    {
        textTimer.enabled = false;
        gettedButton.interactable = true;
        unlocked = true;
        textGetPets.enabled = true;
       
    }

}
