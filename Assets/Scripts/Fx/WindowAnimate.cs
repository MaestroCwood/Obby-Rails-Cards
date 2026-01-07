using UnityEngine;

public class WindowAnimate : MonoBehaviour
{
    [SerializeField] float strengPunch, duration;
    [SerializeField] GameObject panelAnimateObject;
    [SerializeField] GameObject panelSetActive;
    [SerializeField] LeanTweenType easy;
    public void ShowWithAnimation()
    {
        panelSetActive.SetActive(true);
        panelAnimateObject.transform.localScale = Vector3.zero;
        panelAnimateObject.transform.LeanScale(Vector3.one, duration)
            .setEase(easy);
    }

    public void HideWithAnimation()
    {   
        panelAnimateObject.transform.LeanScale(Vector3.zero, duration)
            .setEase(easy)
            .setOnComplete(() =>
            {
                panelSetActive.SetActive(false);
            });
    }
    private void OnDisable()
    {

    }
}
