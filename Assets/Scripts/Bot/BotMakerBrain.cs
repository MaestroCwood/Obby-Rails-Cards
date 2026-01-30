using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

public class BotMakerBrain : MonoBehaviour
{
    [Header("Spline Settings")]
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] SplineInstantiate splineInstantiateBot;
    [SerializeField] SplineAnimate splineAnimateBotMaker;
    [SerializeField] private int maxKnots = 30;
    [SerializeField] private float buildDistance = 2.0f;
    [SerializeField] private float offsetY = 0.5f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpCooldown = 6f;
    [SerializeField] private int minJumpCount = 2;
    [SerializeField] private int maxJumpCount = 4;
    [SerializeField] private float jumpForceUp = 6f;
    [SerializeField] private float jumpForceForward = 3f;
    [SerializeField] private float timeBetweenJumps = 0.35f;
    [SerializeField] LayerMask layerMask;


    [Header("Movement Settings")]
    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float minWanderDistance = 5f;
    [SerializeField] private float wanderInterval = 3f;

    private NavMeshAgent agent;
    private Animator animator;
    private Spline spline;
    private int knotsMade = 0;
    private float lastWanderTime;
    private Vector3 lastKnotPosition;
    private bool isGoToTelejka = false;
    private bool buildingComplete = false;

    private float lastJumpTime = -999f;
    private bool isJumping = false;
    private Rigidbody rb;

    Coroutine corutineJump;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Start()
    {
        if (splineContainer == null)
        {
            Debug.LogError("SplineContainer не назначен!");
            return;
        }

        spline = splineContainer.Spline;
        spline.Clear(); // Очищаем сплайн перед началом

        RandSplineInstatiateRails();
        StartBuilding();
    }

    void RandSplineInstatiateRails()
    {
        var items = splineInstantiateBot.itemsToInstantiate;

        // Сбрасываем все вероятности
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            item.Probability = 0f;
            items[i] = item;
        }

        int randIndex = Random.Range(0, items.Length);

        var selectedItem = items[randIndex];
        selectedItem.Probability = 1f;
        items[randIndex] = selectedItem;

        // Применяем обратно
        splineInstantiateBot.itemsToInstantiate = items;

        Debug.Log($"Selected rail index: {randIndex}");
    }

    private void Update()
    {
        if (buildingComplete) return;

        if (knotsMade >= maxKnots && !isGoToTelejka)
        {
            CompleteBuilding();
            return;
        }

        if(isGoToTelejka) return;

        if (Time.time - lastWanderTime > wanderInterval && agent.isActiveAndEnabled)
        {
            SetRandomDestination();
            lastWanderTime = Time.time;
        }

        CheckAndCreateKnot();
        UpdateAnimation();
        
    }

    private void CreateFirstKnot()
    {
        Vector3 startPos = GetValidPosition(transform.position);
        startPos.y += offsetY;

        spline.Add(new BezierKnot(startPos));
        knotsMade = 1;
        lastKnotPosition = startPos;

      
        Debug.Log($"First knot created at: {startPos}");
    }

    private void CheckAndCreateKnot()
    {
        if (knotsMade >= maxKnots || buildingComplete) return;

        float distanceToLastKnot = Vector3.Distance(GetValidPosition(transform.position), lastKnotPosition);

        if (distanceToLastKnot >= buildDistance)
        {
            CreateNewKnot();
        }
    }

    private void CreateNewKnot()
    {
        Vector3 newKnotPos = GetValidPosition(transform.position);

        // Проверяем, что позиция валидная и не NaN
        if (float.IsNaN(newKnotPos.x) || float.IsNaN(newKnotPos.y) || float.IsNaN(newKnotPos.z))
        {
            Debug.LogWarning("Invalid position for new knot, skipping");
            return;
        }

        newKnotPos.y += offsetY;
        spline.Add(new BezierKnot(newKnotPos));
        spline.SetTangentMode(TangentMode.AutoSmooth);
        knotsMade++;
        lastKnotPosition = newKnotPos;

        Debug.Log($"Knot added: {knotsMade}/{maxKnots} at position: {newKnotPos}");
    }

    private void CompleteBuilding()
    {
        buildingComplete = true;
        StopBuilding();
        StopAllCoroutines();
        // Проверяем и закрываем сплайн если нужно
        if (spline.Count > 2)
        {
            Vector3 firstPos = spline[0].Position;
            Vector3 lastPos = spline[spline.Count - 1].Position;
            float distance = Vector3.Distance(firstPos, lastPos);

            if (distance < 5f)
            {
                spline.Closed = true;
                Debug.Log("Spline closed - start and end are close");
            }
        }
        DeleteRb();
        GoToTelejka();
    }

    private void StopBuilding()
    {
        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        animator.SetBool("Run", false);
        animator.SetBool("Idle", false);
        animator.SetBool("Drive", true);

        Debug.Log($"Building complete. Total knots: {knotsMade}");
    }

    void GoToTelejka()
    {
        
        splineAnimateBotMaker.enabled = true;

        isGoToTelejka = true;
        animator.applyRootMotion = false;
        // Отключаем навмеш агента
        if (agent != null) agent.enabled = false;

        // Проверяем, что сплайн не пустой
        if (spline.Count == 0)
        {
            Debug.LogError("Spline is empty! Cannot start animation.");
            return;
        }

        // Настраиваем SplineAnimate
        splineAnimateBotMaker.Container = splineContainer;
        splineAnimateBotMaker.enabled = true;

        agent.enabled = false;
        transform.SetParent(splineAnimateBotMaker.transform, false);
        agent.transform.localPosition = Vector3.zero;
        agent.transform.localRotation = Quaternion.identity;
       
        // Запускаем анимацию
        var randActivateTelejka = splineAnimateBotMaker.GetComponent<RandActivateTelejka>();
        if (randActivateTelejka != null)
        {
            randActivateTelejka.ActivateTel();
        }

        splineAnimateBotMaker.Restart(true);
        splineAnimateBotMaker.Play();

        Debug.Log("Bot started moving on spline");
    }

    private Vector3 GetValidPosition(Vector3 position)
    {
        // Проверяем на NaN значения
        if (float.IsNaN(position.x) || float.IsNaN(position.y) || float.IsNaN(position.z))
        {
            Debug.LogWarning("Invalid position detected, resetting to zero");
            return Vector3.zero;
        }
        return position;
    }

    private void SetRandomDestination()
    {
        if (!agent.isActiveAndEnabled) return;

        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            float distanceToTarget = Vector3.Distance(transform.position, hit.position);

            if (distanceToTarget > minWanderDistance)
            {
                agent.SetDestination(GetValidPosition(hit.position));
            }
        }
    }


    IEnumerator JumpAgent()
    {
        isJumping = true;
        lastJumpTime = Time.time;

        // Жёстко останавливаем агента
        agent.isStopped = true;
        agent.updatePosition = false;
        agent.updateRotation = false;

        // СНАЧАЛА выравниваем по NavMesh
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1.5f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }

        // ВКЛЮЧАЕМ физику
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;

        int jumps = Random.Range(minJumpCount, maxJumpCount + 1);

        for (int i = 0; i < jumps; i++)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            rb.AddForce(Vector3.up * jumpForceUp, ForceMode.Impulse);
            animator.SetTrigger("Jump1");
            yield return new WaitForSeconds(0.1f);
            float elapsed = 0;
            while(elapsed < 1)
            {
                rb.AddForce(transform.forward * jumpForceForward, ForceMode.Acceleration);
                elapsed += Time.deltaTime;
                yield return null;
            }
           
            elapsed = 0;
           
          


            yield return new WaitForSeconds(timeBetweenJumps);
        }

        // Ждём приземления
        yield return new WaitUntil(IsGrounded);
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;

        // 🔥 СНАЧАЛА гарантируем NavMesh
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hitpos, 1.5f, NavMesh.AllAreas))
        {
            agent.Warp(hitpos.position);
        }
        else
        {
            // fallback — НЕ включаем агента
            Debug.LogWarning("Jump end: agent not on NavMesh");
            isJumping = false;
            yield break;
        }

        // ВОЗВРАЩАЕМ управление агенту
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.isStopped = false;

        isJumping = false;
    }

    void DeleteRb()
    {   
        if(rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            Destroy(rb);
        }     
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.1f,
                                Vector3.down,
                                1.5f, layerMask);
    }
    private void UpdateAnimation()
    {
        if (buildingComplete || !agent.isActiveAndEnabled || isJumping)
            return;

        if (agent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("Run", true);
            animator.SetBool("Idle", false);

            if (Time.time - lastJumpTime > jumpCooldown)
            {
                if (Random.value < 0.25f) // шанс прыжка
                {
                    if (corutineJump != null)
                        StopCoroutine(corutineJump);

                    corutineJump = StartCoroutine(JumpAgent());
                }
            }
        }
        else
        {
            animator.SetBool("Run", false);
            animator.SetBool("Idle", true);
        }
    }

    private void StartBuilding()
    {
        if (agent != null && agent.isActiveAndEnabled)
        {
            SetRandomDestination();
            lastWanderTime = Time.time;
        }
        Debug.Log("Started spline building");
    }

    // Визуализация в редакторе
    private void OnDrawGizmosSelected()
    {
        if (lastKnotPosition != Vector3.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(lastKnotPosition, 0.3f);
            Gizmos.DrawLine(transform.position, lastKnotPosition);
        }
    }
}