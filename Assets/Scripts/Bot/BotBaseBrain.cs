using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

public class BotBaseBrain : MonoBehaviour
{
    public Transform telejkaBot;

    NavMeshAgent agent;
    Animator animator;

    RandActivateTelejka randActivate;
    SplineAnimate splineAnimate;
    bool isGoToTelejkaBOt = false;
    bool isGoingToZero = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        agent.SetDestination(telejkaBot.position);
        SetAnimate("Run");
    }

    private void Update()
    {
        if (!isGoToTelejkaBOt && !isGoingToZero)
        {
            float dist = Vector3.Distance(agent.transform.position, telejkaBot.position);
            if (dist <= 2f)
            {
                StartRide();
            }
        }

        if (isGoingToZero && AgentReached())
        {
            isGoingToZero = false;
            StartCoroutine(WaitAgent());
        }
    }

    bool AgentReached()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    return true;
            }
        }
        return false;
    }

    void StartRide()
    {
        isGoToTelejkaBOt = true;
        SetAnimate("Drive");

        randActivate = telejkaBot.GetComponent<RandActivateTelejka>();
        splineAnimate = telejkaBot.GetComponent<SplineAnimate>();

        splineAnimate.Completed += SplineAnimate_Completed;

        randActivate.ActivateTel();

        animator.applyRootMotion = false;
        agent.enabled = false;

        agent.transform.SetParent(telejkaBot.transform, false);
        agent.transform.localPosition = Vector3.zero;
        agent.transform.localRotation = Quaternion.identity;

        splineAnimate.Restart(false);
        splineAnimate.Play();
    }

    private void SplineAnimate_Completed()
    {
        splineAnimate.Completed -= SplineAnimate_Completed;

        randActivate.DeactivateTelejka();
        agent.transform.SetParent(null, true); // Изменено на true для сохранения мировых координат
        agent.enabled = true;
        animator.applyRootMotion = true;

        // Устанавливаем флаг и запускаем движение к точке (0,0,0)
        isGoingToZero = true;
        isGoToTelejkaBOt = false;

        SetAnimate("Run"); // Включаем анимацию бега
        agent.SetDestination(Vector3.zero);
    }

    IEnumerator WaitAgent()
    {
        // Стоим в точке (0,0,0) с анимацией Idle
        SetAnimate("Idle");

        yield return new WaitForSeconds(3f);

        // Начинаем новый цикл - бежим к тележке
        isGoToTelejkaBOt = false;
        isGoingToZero = false;

        SetAnimate("Run");
        agent.SetDestination(telejkaBot.position);
    }

    void SetAnimate(string state)
    {
        animator.SetBool("Run", false);
        animator.SetBool("Drive", false);

        switch (state)
        {
            case "Run":
                animator.SetBool("Run", true);
                break;
            case "Drive":
                animator.SetBool("Drive", true);
                break;
            case "Idle":
                // Все анимации уже выключены
                break;
        }
    }
}