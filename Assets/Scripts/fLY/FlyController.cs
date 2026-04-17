using StarterAssets;
using System.Collections;
using UnityEngine;

public class FlyController : MonoBehaviour
{
    [SerializeField] float durationDelay;
    [SerializeField] float speed;
    [SerializeField] AudioSource audioSource;
    [SerializeField]Vector3 startPosition;
    [SerializeField]Vector3 endPosition;
    [SerializeField] ThirdPersonController playerController;
    [SerializeField] GameObject[] spawnBrainrot;
    bool isDelay = false;
    bool isPlayingSoundFx;
    float currentDelay;

    Coroutine coroutine;
    private void Start()
    {
        transform.position = startPosition; 
    }
    private void Update()
    {
        if (!isDelay)
        {
            Move();
            if (transform.position.z >= endPosition.z)
            {
                transform.position = startPosition;
                OffsetX();
            }

            float distance = Vector3.Distance(transform.position, playerController.transform.position);
            if(distance <= 70)
            {
                PlaySoundFx();
                if(coroutine == null)
                {
                    coroutine = StartCoroutine(Spawn());
                }
            }
           
        }


        if (currentDelay < durationDelay && isDelay)
        {
            currentDelay += Time.deltaTime;

        }
        else
        {
            currentDelay = 0;
            isDelay = false;
        }
       

    }

    void Move()
    {
        transform.position += Vector3.forward * speed * Time.deltaTime;
    }

    void OffsetX()
    {   
        float randXoffset = Random.Range(-10f, 10f);
        transform.position = new Vector3(transform.position.x + randXoffset, transform.position.y,transform.position.z);
        isDelay = true;
    }

    void PlaySoundFx()
    {   
        if(!audioSource.isPlaying) 
            audioSource.Play();
    }

    IEnumerator Spawn()
    {
        for (int i = 0; i < 10; i++)
        {   
            int index = Random.Range(0, spawnBrainrot.Length);
            Instantiate(spawnBrainrot[index], transform.position, Quaternion.identity);
            yield return new WaitForSeconds(.5f);
        }

        coroutine = null;
        yield return null;
    }

}
