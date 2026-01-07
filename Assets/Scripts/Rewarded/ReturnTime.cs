using UnityEngine;

public class ReturnTime : MonoBehaviour
{
   



    double currentTime;
  

    private void Update()
    {
        currentTime += Time.deltaTime;
    }


    public double  TimeSessionAll()
    {
        return currentTime;
    }



}
