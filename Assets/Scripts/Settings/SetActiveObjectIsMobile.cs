using UnityEngine;
using YG;

public class SetActiveObjectIsMobile : MonoBehaviour
{
    [SerializeField] GameObject targetObj;
    [SerializeField] bool isActiveObj;

    bool isMobileDivece;



    private void Start()
    {
        isMobileDivece = YG2.envir.isMobile;

        if(isMobileDivece)
            SetActive(isActiveObj);
    }


    public void SetActive(bool active)
    {
        targetObj.SetActive(active);
    }

}
