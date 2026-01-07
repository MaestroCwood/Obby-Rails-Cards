using UnityEngine;
using UnityEngine.UI;

public class SettingsUi : MonoBehaviour
{
    [SerializeField] Slider sliderSensetivity;
  

    [SerializeField] YGTouchscreen yGTouchscreen;
    [SerializeField] float minValueSensetivity;
    [SerializeField] float maxValueSensetivity;

    private void Start()
    {   

        sliderSensetivity.minValue = minValueSensetivity;
        sliderSensetivity.maxValue = maxValueSensetivity;

        float average = minValueSensetivity + maxValueSensetivity / 2;
        yGTouchscreen.SetSensitivity(average);
        sliderSensetivity.value = average;

      
    }

    public void SetSens(float sens)
    {   
        
        Mathf.Clamp(sens, minValueSensetivity, maxValueSensetivity);
        yGTouchscreen.SetSensitivity(sens);
    }


}
