using UnityEngine;

public class BrainrotItemsSpawn : MonoBehaviour
{
    [SerializeField] float delayToDestroy;

    [SerializeField] GameObject spawnFx;


    bool isDestroyed = false;
    float currentDuration;

    private void Start()
    {
        currentDuration = delayToDestroy;
    }


    private void Update()
    {
        if(!isDestroyed)
        {
            currentDuration -= Time.deltaTime;
            if(currentDuration <=0)
            {
                isDestroyed = true;
                GameObject go = Instantiate(spawnFx, transform.position, Quaternion.identity);
              
                Destroy(gameObject);
                return;
            }
        }
    }
}
