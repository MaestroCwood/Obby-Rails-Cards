using StarterAssets;
using UnityEngine;

public class OnJumpTramline : MonoBehaviour
{
    [SerializeField] ThirdPersonController playerController;
    [SerializeField] GameObject coinPrefabs;
    [SerializeField] GameObject fxGenerate;
    [SerializeField] GameObject fxDestroyCoin;



    float defoltJump;

    private void Start()
    {
        defoltJump = playerController.JumpHeight;
    }

    private void OnTriggerEnter(Collider other )
    {
        if(other.CompareTag("Player") && playerController.Grounded)
        {
           SpawnCoin(other.transform);
            JumpBoost();


        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            ResetJumpBoost();
        }
    }


    void SpawnCoin(Transform posSpaw)
    {
       GameObject go = Instantiate(coinPrefabs, posSpaw.position,Quaternion.identity);
        Animate(go);
        GameEvents.OnGenerateCoin?.Invoke(100);
    }

    void ResetJumpBoost()
    {
        playerController.JumpHeight = defoltJump;
    }

    void JumpBoost()
    {
      
        playerController.JumpHeight += 5f;
    }
    void Animate(GameObject go)
    {

        Vector3 target = new Vector3(go.transform.position.x, go.transform.position.y + 20, go.transform.position.z);
        float rand = Random.Range(2f, 3f);
        go.transform.LeanMove(target, rand).setOnComplete(() =>
        {
            Instantiate(fxDestroyCoin, go.transform.position, Quaternion.identity);
            Destroy(go, 0.1f);
        });
        go.transform.LeanRotateY(920f, rand).setEase(LeanTweenType.linear);
    }
}
