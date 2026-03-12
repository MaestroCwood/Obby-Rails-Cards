
using StarterAssets;
using SuperHorizon.EnemyNPCai;
using Unity.Cinemachine;
using UnityEngine;

public class SahurFx : MonoBehaviour
{
    [SerializeField] Vector3 maxScale;
    [SerializeField] float time;
    [SerializeField] CinemachineImpulseSource impulseSource;
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] AIController aiControllerSahur;
    [SerializeField] ThirdPersonController playerController;

    public float maxDistance = 20f; // дистанция, после которой импульс почти нулевой
    public float maxImpulse = 1f;   // максимальная сила тряски
    public float minImpulse = 0.05f; // минимальная
    Vector3 defaultScale;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        defaultScale = transform.localScale;
    }

    private void OnEnable()
    {
        GameEvents.OnPickUpBrainRot += OnPickUpBrain;
    }

    private void OnDisable()
    {
        GameEvents.OnPickUpBrainRot -= OnPickUpBrain;
    }

    private void OnPickUpBrain(bool obj)
    {
        if (obj) Scale();
         else DefaultScale();
    }

    public void Scale()
    {
        PlayOneShot(0);
        transform.LeanScale(maxScale, time);
    }

    public void DefaultScale()
    {
        transform.LeanScale(defaultScale, time);
    }


    public void PlayOneShot(int index)
    {
        audioSource.PlayOneShot(audioClips[index]);
    }

    public void ImpulseShake()
    {   
        if(aiControllerSahur.currentState == AIController.AIState.Chase)
        {   

            float distance = Vector3.Distance(playerController.transform.position, transform.position);
            float t = 1f - Mathf.InverseLerp(0, maxDistance, distance);

            float impulse = Mathf.Lerp(minImpulse, maxImpulse, t);

            impulseSource.GenerateImpulse(impulse);
            PlayOneShot(1);
            

        }

        

    }
}
