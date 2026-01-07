using StarterAssets;
using System;
using UnityEngine;
using UnityEngine.Splines;

public class MakeRaidsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private ThirdPersonController playerController;


    [Header("Settings")]
    [SerializeField] private int maxElement = 10;
    [SerializeField] private float distanceToMake = 2.0f;
    [SerializeField] float offsetY;

    private Spline spline;
    private bool isStartMake = false;
    bool isGoToTelejkaActive;
    private int countMakeRaidsElement = 0;
    private bool isInitialized = false;

    public event Action<int, int> OnMakeRaids;
    public event Action<bool> OnIsStartMake;


    float distanceFirstLastKnot;
    private void Awake()
    {
        Initialize();
        GameEvents.OnSplineStarted += OnSplineStarted;
        GoToRainds.OnStartRaindGoMove += GoToRainds_OnStartRaindGoMove;
        GoToRainds.OnStopRaindGoMove += GoToRainds_OnStopRaindGoMove;
    }


    private void OnDisable()
    {
        GameEvents.OnSplineStarted -= OnSplineStarted;
        GoToRainds.OnStartRaindGoMove -= GoToRainds_OnStartRaindGoMove;
        GoToRainds.OnStopRaindGoMove -= GoToRainds_OnStopRaindGoMove;
    }

    private void Start()
    {
        if (!isInitialized)
        {
            Initialize();
        }
        OnIsStartMake?.Invoke(isStartMake);
    }

    private void OnSplineStarted()
    {
        ClearRaids();
    }

    private void GoToRainds_OnStartRaindGoMove(object sender, EventArgs e)
    {
        // ClearRaids();

        isGoToTelejkaActive = true;
        StopRaid();

    }


    private void GoToRainds_OnStopRaindGoMove(object sender, EventArgs e)
    {
        isGoToTelejkaActive = false;
    }
    private void Initialize()
    {
        if (isInitialized) return;

        if (splineContainer == null)
        {
            Debug.LogError("SplineContainer не назначен в инспекторе!");
            return;
        }

        if (playerController == null)
        {
            Debug.LogError("ThirdPersonController не назначен в инспекторе!");
            return;
        }

        spline = splineContainer.Spline;
        countMakeRaidsElement = spline.Count;
        isInitialized = true;
        Debug.Log("MakeRaidsManager инициализирован");
    }

    private void OnValidate()
    {
        if (splineContainer == null)
        {
            splineContainer = GetComponent<SplineContainer>();
            if (splineContainer == null)
            {
                Debug.LogWarning("Добавьте компонент SplineContainer на этот GameObject");
            }
        }
    }

    private void Update()
    {
        if (!isInitialized) return;

        // Обработка нажатия Q
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleMakingRaids();
        }

        // Если сплайн уже начат, добавляем новые узлы при движении
        if (isStartMake)
        {
            HandleMakingRaids();
        }

        // Очистка сплайна
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    ClearRaids();
        //}
    }

    public void ToggleMakingRaids()
    {
        // Если достигли максимума - очищаем и начинаем заново

        if (isGoToTelejkaActive) return;
        if (countMakeRaidsElement >= maxElement)
        {
            ClearRaids();
            StartMakingRaids();
            return;
        }

        // Обычное переключение
        if (isStartMake)
        {
            StopMakingRaids();
        }
        else
        {
            ClearRaids();
            StartMakingRaids();
        }
    }

    private void CreateFirstKnot()
    {
        if (isGoToTelejkaActive) return;
        spline.Closed = false;
        Vector3 playerPosition = playerController.transform.position;
        playerPosition.y += offsetY;

        spline.Add(new BezierKnot(playerPosition));

        countMakeRaidsElement = 1;
        OnMakeRaids?.Invoke(countMakeRaidsElement, maxElement);
        OnIsStartMake?.Invoke(isStartMake);
        Debug.Log("Создан первый узел сплайна");
    }

    private void StartMakingRaids()
    {
        if (!isInitialized || isGoToTelejkaActive) return;

        if (spline.Count == 0)
        {
            CreateFirstKnot();
        }

        isStartMake = true;
        OnIsStartMake?.Invoke(isStartMake);
        Debug.Log("Начато создание сплайна");
    }

    private void StopMakingRaids()
    {
        isStartMake = false;
        OnIsStartMake?.Invoke(isStartMake);

        if (distanceFirstLastKnot <= 5)
        {
            spline.Closed = true;
        }
        Debug.Log("Остановлено создание сплайна");
    }

    private void HandleMakingRaids()
    {
        if (!isInitialized || playerController == null) return;

        if (countMakeRaidsElement >= maxElement)
        {
            StopMakingRaids();
            return;
        }

        int lastKnotIndex = spline.Count - 1;
        if (lastKnotIndex < 0) return;
        Vector3 firstKnot = spline[0].Position;
        Vector3 worldfirstKnotPos = splineContainer.transform.TransformPoint(firstKnot);
        Vector3 lastKnotPos = spline[lastKnotIndex].Position;
        Vector3 worldKnotPos = splineContainer.transform.TransformPoint(lastKnotPos);
        Vector3 playerPosition = playerController.transform.position;
        distanceFirstLastKnot = DistanceFirstLastKnot(worldfirstKnotPos, worldKnotPos);
        float distance = Vector3.Distance(playerPosition, worldKnotPos);

        if (distance >= distanceToMake)
        {
            CreateNewKnot(playerPosition);
        }

      
    }
    float DistanceFirstLastKnot(Vector3 firstKnot, Vector3 lastKnot)
    {
        float distance = Vector3.Distance(firstKnot, lastKnot);
        return distance;
    }
    private void CreateNewKnot(Vector3 playerPosition)
    {
        if (!isInitialized) return;

        playerPosition.y += offsetY;
        spline.Add(new BezierKnot(playerPosition));
        for (int i = 0; i < spline.Count; i++)
        {
            spline.SetTangentMode(TangentMode.AutoSmooth);
        }
        countMakeRaidsElement++;
        OnMakeRaids?.Invoke(countMakeRaidsElement, maxElement);
        Debug.Log($"Добавлен новый узел. Всего узлов: {countMakeRaidsElement}");
    }

    public void ClearRaids()
    {
        if (!isInitialized) return;

        spline.Clear();
        countMakeRaidsElement = 0;
        isStartMake = false;
        OnMakeRaids?.Invoke(countMakeRaidsElement, maxElement);
        OnIsStartMake?.Invoke(isStartMake);
        Debug.Log("Сплайн очищен");
    }

    // Методы для управления из других скриптов
    public void StartRaid()
    {
        if (!isInitialized) Initialize();
        StartMakingRaids();
    }

    public void StopRaid()
    {
        StopMakingRaids();
    }

    // Свойства для доступа из других скриптов
    public bool IsMakingRaids => isStartMake;
    public int RaidsElementCount => countMakeRaidsElement;
    public float CurrentDistanceToMake => distanceToMake;
    public bool IsInitialized => isInitialized;
}