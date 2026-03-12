using StarterAssets;
using SuperHorizon.EnemyNPCai;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class SahurAttack : MonoBehaviour
{
    [SerializeField] ThirdPersonController playerController;
    [SerializeField] float attackRange = 5f;
    [SerializeField] GameObject fxDamage;
    [SerializeField] Transform targetPosCreatedFx;
    [SerializeField] CinemachineCamera ciniemachine;
    [SerializeField] Perception perception;
    Vector3 startPosPlayer;

    SahurFx sahurFx;

    private void Awake()
    {
        sahurFx = GetComponent<SahurFx>();
    }
    private void Start()
    {
        startPosPlayer = playerController.transform.position;
    }

    public void AttackSahur()
    {
        float distance = Vector3.Distance(transform.position, playerController.transform.position);
       
        if (distance < attackRange)
        {
            GameEvents.OnDamageToPlayer?.Invoke();
            playerController.Teleport(startPosPlayer);            
        }
        Instantiate(fxDamage, targetPosCreatedFx);
        sahurFx.PlayOneShot(2);
    }


    public void SwitchCam()
    {
        StartCoroutine(SwitchCamera());
    }

    IEnumerator SwitchCamera()
    {
        ciniemachine.Priority = 20;
        yield return new WaitForSeconds(2);
        ciniemachine.Priority = 0;
        if (TryGetComponent(out AIController ai))
        {   
            if(perception.CanSeeTarget(playerController.transform))
                ai.SetState(AIController.AIState.Chase);
            else ai.SetState(AIController.AIState.Patrol);
        }
    }
}
