using System.Collections;
using UnityEngine;

public class GenerateCoin : MonoBehaviour
{
    [SerializeField] float delayGenerate = 5f;
    [SerializeField] GameObject icoFx;
    [SerializeField] GameObject destroyFx;
 
    [SerializeField] bool isActiveGenerate = false;



    int countGenerate;

    private void Start()
    {
        StartCoroutine(Generator());

        countGenerate = GameManager.instance.GetCurrentGenerate();
    }
    IEnumerator Generator()
    {
        while(enabled)
        {
            if (isActiveGenerate)
            {
                GameObject go = Instantiate(icoFx, transform.position, Quaternion.identity);

                Animate(go);
                GameEvents.OnGenerateCoin?.Invoke(GameManager.instance.GetCurrentGenerate());
                float ranDelay = Random.Range(delayGenerate, delayGenerate + 5);
                yield return new WaitForSeconds(ranDelay);
            }

            yield return null;
        }
    }



    void Animate(GameObject go)
    {   

        Vector3 target = new Vector3(go.transform.position.x, go.transform.position.y +20, go.transform.position.z);
        float rand = Random.Range(2f, 3f);
        go.transform.LeanMove(target, rand).setOnComplete(() =>
        {
            Instantiate(destroyFx, go.transform.position, Quaternion.identity);
            Destroy(go, 0.1f);
        });
        go.transform.LeanRotateY(920f, rand).setEase(LeanTweenType.linear);
    }


    public void SetCointGenerateCoin(int coint)
    {
        countGenerate = coint;
    }

    public int GetCurrentGenetate()
    {
        return countGenerate;
    }
}
