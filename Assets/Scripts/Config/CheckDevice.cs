using UnityEngine;
using YG;

public class CheckDevice : MonoBehaviour
{
    bool isMobileDevice;

    private void Start()
    {
        isMobileDevice = YG2.envir.isMobile;

        ChangeActiveSelfObject();
    }



    public void ChangeActiveSelfObject()
    {
        gameObject.SetActive(isMobileDevice);
    }
}
