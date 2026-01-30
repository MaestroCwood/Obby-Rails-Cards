using StarterAssets;
using System.Collections;
using TMPro;
using UnityEngine;

public class GenerateCoin : MonoBehaviour
{
    [SerializeField] float delayGenerate = 5f;
    [SerializeField] GameObject icoFx;
    [SerializeField] GameObject destroyFx;
    [SerializeField] ThirdPersonController playerController;
    [SerializeField] GoToRainds goToRainds;
    [SerializeField] bool isActiveGenerate = false;

    [SerializeField] Color[] colors;



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
                GameObject go = Instantiate(icoFx, playerController.transform.position, Quaternion.identity);

                TextMeshPro txt = go.GetComponentInChildren<TextMeshPro>();
                txt.text = $"+{GameManager.instance.GetCurrentGenerate()}";
                txt.color = colors[RandIndex()];
                Animate(go);
               
                GameEvents.OnGenerateCoin?.Invoke(GameManager.instance.GetCurrentGenerate());
                
                yield return new WaitForSeconds(delayGenerate);
            }

            yield return null;
        }
    }

    int RandIndex()
    {
        int index = Random.Range(0, colors.Length);
        return index;
    }
    private void Update()
    {
        if(goToRainds.isMoveNow)
        {
            isActiveGenerate = true;
        } else
        {
            isActiveGenerate = false;
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
