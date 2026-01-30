using StarterAssets;
using System;
using UnityEngine;
using UnityEngine.Splines;
using YG;




public class GoToRainds : MonoBehaviour
{
    [SerializeField] MakeRaidsManager makeRaidsManager;
    [SerializeField] ThirdPersonController playerController;
    [SerializeField] StarterAssetsInputs assetsInputs;
    [SerializeField] SplineAnimate splineAnimateObj;
    [SerializeField] GameObject[] raids;
    public float speed = 0.3f;

    public static event EventHandler OnStartRaindGoMove;
    public static event EventHandler OnStopRaindGoMove;

    float currentSpeedMultiplier = 1f;

    private bool rotateActive;
    public bool isRiding { get; private set; }
    public bool isMoveNow { get; private set; }

    public SplineContainer splineContainer; // Текущий выбранный сплайн
    SplineContainer[] allSplineContainers;
    int nearestKnotIndex = -1;
    bool isNearbyFindTelejka = false;
    float nearestDistance = float.MaxValue;
    float nearestNormalizedTime = 0f; // Нормализованная позиция на сплайне

    float input = 0f;
    int curreintActiveRaids;

    public AudioSource audoSourceRaid;
    private void Start()
    {
        assetsInputs.OnStartJumpPlayer += PlayerController_OnJump;
        YG2.onRewardAdv += OnRewardedYg;

      
    }

    private void OnDisable()
    {
        assetsInputs.OnStartJumpPlayer -= PlayerController_OnJump;
        YG2.onRewardAdv -= OnRewardedYg;
    }

  

    private void PlayerController_OnJump()
    {
        if (isRiding)
            StopGoAndResetSatate();
    }

    public void StopGoAndResetSatate()
    {
        if (makeRaidsManager.RaidsElementCount < 2) return;
        var anim = playerController.GetComponent<Animator>();
        anim.SetLayerWeight(1, 0);
        isRiding = false;
        playerController.enabled = true;
        playerController.GetComponent<Animator>().enabled = true;
        playerController.gameObject.transform.SetParent(null);
        playerController.gameObject.transform.localScale = Vector3.one;

        for (int i = 0; i < raids.Length; i++)
        {
            raids[i].SetActive(false);
        }

        if (splineAnimateObj != null)
        {
            splineAnimateObj.Restart(false);
            splineAnimateObj.Completed -= SplineAnimateObj_Completed;
        }
        audoSourceRaid.Stop();
        OnStopRaindGoMove?.Invoke(this, EventArgs.Empty);
    }
    private void OnRewardedYg(string obj)
    {
        if(obj == "Speed")
        {
            currentSpeedMultiplier *= 1.3f;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GoToTelejkaRain();
        }

        if (isRiding && splineAnimateObj != null && splineContainer != null)
        {
            // ---------- INPUT ----------
            if (Input.GetKey(KeyCode.W) || assetsInputs.move.y > 0.01f)
                input = 1f;
            else if (Input.GetKey(KeyCode.S) || assetsInputs.move.y < -0.1f)
                input = -1f;
            else
                input = 0f;

            // ---------- SPEED ----------
            float splineLength = splineContainer.CalculateLength();
            float speedMultiplier = currentSpeedMultiplier; // из награды
            float normalizedSpeed = (speed * speedMultiplier) / splineLength;

            float delta = input * normalizedSpeed * Time.deltaTime;
            splineAnimateObj.NormalizedTime += delta;

            // ---------- SPLINE TYPE ----------
            if (splineContainer.Spline.Closed)
            {
                // Замкнутый — зацикливаем
                if (splineAnimateObj.NormalizedTime > 1f)
                    splineAnimateObj.NormalizedTime -= 1f;
                else if (splineAnimateObj.NormalizedTime < 0f)
                    splineAnimateObj.NormalizedTime += 1f;
            }
            else
            {
                // Незамкнутый — останавливаемся в конце
                if (splineAnimateObj.NormalizedTime >= 1f)
                {
                    splineAnimateObj.NormalizedTime = 1f;
                    SplineAnimateObj_Completed();
                }
                else if (splineAnimateObj.NormalizedTime <= 0f)
                {
                    splineAnimateObj.NormalizedTime = 0f;
                }
            }

        
            
        }

        if (Mathf.Abs(input) > 0.5f)
        {
            isMoveNow = true;
            float curPit = audoSourceRaid.pitch;
            float tarrgPot = Mathf.Abs(input * 1.5f);
            audoSourceRaid.pitch = Mathf.Lerp(curPit, tarrgPot, 1f * Time.deltaTime);
        }
        else
        {
            isMoveNow = false;
            audoSourceRaid.pitch = Mathf.Lerp(
                audoSourceRaid.pitch,
                1f,
                3f * Time.deltaTime);
        }
     
        input = 0f;
    }


    void RotateTelejka(float direction)
    {


        if (Mathf.Approximately(direction, 0f) || rotateActive) return;

        rotateActive = true;

        float angle = direction > 0 ? 90f : -90f;

        raids[curreintActiveRaids].transform
            .LeanRotateY(angle, 0.3f)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                rotateActive = false;
            });

    }
    // Метод для поиска ближайшего сплайна и узла
    void FindNearestSplineAndKnot()
    {
        // Находим все сплайны в сцене
        allSplineContainers = FindObjectsByType<SplineContainer>(FindObjectsSortMode.None);

        if (allSplineContainers == null || allSplineContainers.Length == 0)
        {
            Debug.Log("Сплайны не найдены в сцене");
            ResetFindings();
            return;
        }

        Vector3 playerWorldPos = playerController.transform.position;
        SplineContainer nearestContainer = null;
        int nearestKnot = -1;
        float minDistance = float.MaxValue;
        float foundNormalizedTime = 0f;
        isNearbyFindTelejka = false;

        // Проходим по всем сплайнам
        foreach (SplineContainer container in allSplineContainers)
        {
            if (container == null || container.Spline == null) continue;

            Spline spline = container.Spline;
            if (spline.Count == 0) continue;

            // Ищем ближайшую точку на всем сплайне, а не только в узлах
            // Это дает более точное определение позиции
            float closestT = 0f;
            float closestDistance = float.MaxValue;

            // Используем GetNearestPoint для более точного определения
            Vector3 nearestPoint = spline.EvaluatePosition(0f);
            float t = 0f;
            float step = 0.01f; // Шаг для поиска по сплайну

            // Итерируем по сплайну с шагом
            for (float sampleT = 0f; sampleT <= 1f; sampleT += step)
            {
                Vector3 samplePoint = spline.EvaluatePosition(sampleT);
                Vector3 worldSamplePoint = container.transform.TransformPoint(samplePoint);
                float distance = Vector3.Distance(playerWorldPos, worldSamplePoint);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestT = sampleT;
                    nearestPoint = samplePoint;
                }
            }

            // Проверяем узлы сплайна для дополнительной точности
            for (int i = 0; i < spline.Count; i++)
            {
                Vector3 localKnotPos = spline[i].Position;
                Vector3 worldKnotPos = container.transform.TransformPoint(localKnotPos);
                float distance = Vector3.Distance(playerWorldPos, worldKnotPos);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestContainer = container;
                    nearestKnot = i;
                    foundNormalizedTime = GetNormalizedTimeForKnot(spline, i);
                    isNearbyFindTelejka = true;
                }
            }

            // Если нашли точку на сплайне ближе, чем любой узел
            if (closestDistance < minDistance)
            {
                minDistance = closestDistance;
                nearestContainer = container;
                nearestKnot = -1; // Это не конкретный узел, а точка на сплайне
                foundNormalizedTime = closestT;
                isNearbyFindTelejka = true;
            }
        }

        // Сохраняем результаты
        splineContainer = nearestContainer;
        nearestKnotIndex = nearestKnot;
        nearestDistance = minDistance;
        nearestNormalizedTime = foundNormalizedTime;

        // Выводим отладочную информацию
        if (isNearbyFindTelejka && splineContainer != null)
        {
            Debug.Log($"Найден ближайший сплайн: {splineContainer.gameObject.name}, " +
                     $"узел: {nearestKnotIndex}, расстояние: {minDistance:F2}, " +
                     $"позиция на сплайне: {foundNormalizedTime:F2}");
        }
        else
        {
            Debug.Log("Ближайший сплайн не найден");
        }
    }

    // Вспомогательный метод для получения нормализованного времени для узла
    float GetNormalizedTimeForKnot(Spline spline, int knotIndex)
    {
        if (spline == null || knotIndex < 0 || knotIndex >= spline.Count)
            return 0f;

        // Для простоты предполагаем равномерное распределение узлов
        // В реальности нужно использовать GetCurveIndex или аналогичные методы
        return Mathf.Clamp01((float)knotIndex / Mathf.Max(1, spline.Count - 1));
    }

    // Сброс результатов поиска
    void ResetFindings()
    {
        isNearbyFindTelejka = false;
        splineContainer = null;
        nearestKnotIndex = -1;
        nearestDistance = float.MaxValue;
        nearestNormalizedTime = 0f;
    }

    public void GoToTelejkaRain()
    {
        // Ищем ближайший сплайн
        FindNearestSplineAndKnot();

        // Проверяем условия
        if (makeRaidsManager.RaidsElementCount < 2 || !isNearbyFindTelejka || splineContainer == null)
        {
            Debug.Log($"Условия не выполнены: элементов рейдов={makeRaidsManager.RaidsElementCount}, " +
                     $"найден сплайн={isNearbyFindTelejka}, splineContainer={splineContainer != null}");
            return;
        }

        // Проверяем расстояние
        if (nearestDistance > 13f)
        {
            Debug.Log($"Слишком далеко: {nearestDistance:F2} > 13");
            return;
        }

        // Настраиваем анимацию сплайна
        splineAnimateObj.Container = splineContainer;
        float splineLength = splineContainer.CalculateLength();
        splineAnimateObj.MaxSpeed = speed / splineLength;
        splineAnimateObj.enabled = true;

        // Устанавливаем начальную позицию на ближайшую найденную точку
        splineAnimateObj.NormalizedTime = Mathf.Clamp01(nearestNormalizedTime);

       

        if (raids.Length > 0)
        {
            for (int i = 0; i < raids.Length; i++)
            {
                var isSel = raids[i].GetComponent<IsSelectedRaids>();
                if (!isSel.isSelctedCard)
                {
                    raids[i].SetActive(false);
                }
                else if(isSel.isSelctedCard) 
                {
                    raids[i].SetActive(true);
                    curreintActiveRaids = i;
                }
            }
        }

        // Начинаем движение
        OnStartRaindGoMove?.Invoke(this, EventArgs.Empty);
        playerController.enabled = false;
       
        var anim = playerController.GetComponent<Animator>();
        anim.SetLayerWeight(1, 1);

        // Прикрепляем игрока к аниматору сплайна
        playerController.gameObject.transform.SetParent(splineAnimateObj.transform, false);
        playerController.transform.localPosition = new Vector3(0, 0.5f, 0);
        playerController.transform.localRotation = Quaternion.identity;

        isRiding = true;
        splineAnimateObj.Completed += SplineAnimateObj_Completed;
        splineAnimateObj.Play();
        AudioManager.instance.PlayFx(5);
        audoSourceRaid.Play();
        Debug.Log($"Игрок сел в тележку на сплайн: {splineContainer.gameObject.name} " +
                 $"в позиции {nearestNormalizedTime:F2}");
    }

    // Для отладки в редакторе - визуализация
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || playerController == null) return;

        // Проверяем все условия перед доступом к массиву
        if (isNearbyFindTelejka &&
            splineContainer != null &&
            splineContainer.Spline != null &&
            nearestKnotIndex >= 0 &&
            nearestKnotIndex < splineContainer.Spline.Count)
        {
            try
            {
                // Рисуем линию от игрока к ближайшему узлу
                Vector3 playerPos = playerController.transform.position;
                Vector3 knotPos = splineContainer.transform.TransformPoint(
                    splineContainer.Spline[nearestKnotIndex].Position);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(playerPos, knotPos);
                Gizmos.DrawSphere(knotPos, 0.5f);

                // Показываем расстояние
#if UNITY_EDITOR
                UnityEditor.Handles.Label(playerPos + Vector3.up * 2,
                    $"Расстояние: {nearestDistance:F2}\n" +
                    $"Позиция: {nearestNormalizedTime:F2}");
#endif
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Ошибка в OnDrawGizmosSelected: {e.Message}");
            }
        }
    }

    private void SplineAnimateObj_Completed()
    {
        //if (splineAnimateObj != null)
        //    splineAnimateObj.Completed -= SplineAnimateObj_Completed;

        //playerController.enabled = true;
        //if (playerController.GetComponent<Animator>() != null)
        //    playerController.GetComponent<Animator>().enabled = true;

        //playerController.gameObject.transform.SetParent(null);
        //playerController.gameObject.transform.localScale = Vector3.one;
        //isRiding = false;

        Debug.Log("Движение по сплайну завершено");
    }
}