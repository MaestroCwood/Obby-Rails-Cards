using StarterAssets;
using System;
using UnityEngine;
using UnityEngine.Splines;

public class GoToRainds : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] ThirdPersonController playerController;
    [SerializeField] StarterAssetsInputs assetsInputs;
    [SerializeField] SplineAnimate splineAnimateObj;
    [SerializeField] MakeRaidsManager makeRaidsManager;

    [Header("Settings")]
    [SerializeField] float maxEnterDistance = 6f;
    [SerializeField] float speed = 0.3f;

    public static event EventHandler OnStartRaindGoMove;
    public static event EventHandler OnStopRaindGoMove;

    public bool isRiding { get; private set; }

    private SplineContainer currentSpline;
    private int nearestKnotIndex;

    private void Start()
    {
        assetsInputs.OnStartJumpPlayer += OnJump;
    }

    private void OnDisable()
    {
        assetsInputs.OnStartJumpPlayer -= OnJump;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            TryEnterSpline();

        if (!isRiding) return;

        float input = assetsInputs.move.y;
        splineAnimateObj.NormalizedTime += input * Time.deltaTime * splineAnimateObj.MaxSpeed;

        if (currentSpline.Spline.Closed)
        {
            if (splineAnimateObj.NormalizedTime > 1f) splineAnimateObj.NormalizedTime -= 1f;
            if (splineAnimateObj.NormalizedTime < 0f) splineAnimateObj.NormalizedTime += 1f;
        }
        else
        {
            splineAnimateObj.NormalizedTime = Mathf.Clamp01(splineAnimateObj.NormalizedTime);
        }
    }

    private void OnJump()
    {
        if (isRiding)
            ExitSpline();
    }

    // ===================== CORE =====================

    void TryEnterSpline()
    {
        if (makeRaidsManager.RaidsElementCount < 2)
            return;

        if (!FindNearestSpline(out currentSpline, out nearestKnotIndex))
        {
            Debug.Log("❌ Далеко от сплайна");
            return;
        }

        EnterSpline();
    }

    bool FindNearestSpline(out SplineContainer nearestContainer, out int nearestKnot)
    {
        nearestContainer = null;
        nearestKnot = -1;

        Vector3 playerPos = playerController.transform.position;
        float minDistance = float.MaxValue;

        SplineContainer[] allSplines = FindObjectsOfType<SplineContainer>();

        foreach (var container in allSplines)
        {
            var spline = container.Spline;
            if (spline == null || spline.Count < 2) continue;

            for (int i = 0; i < spline.Count; i++)
            {
                Vector3 worldPos =
                    container.transform.TransformPoint(spline[i].Position);

                float dist = Vector3.Distance(playerPos, worldPos);

                if (dist < minDistance)
                {
                    minDistance = dist;
                    nearestContainer = container;
                    nearestKnot = i;
                }
            }
        }

        return nearestContainer != null && minDistance <= maxEnterDistance;
    }

    void EnterSpline()
    {
        OnStartRaindGoMove?.Invoke(this, EventArgs.Empty);

        splineAnimateObj.Container = currentSpline;

        float splineLength = currentSpline.CalculateLength();
        splineAnimateObj.MaxSpeed = speed / splineLength;

        int count = currentSpline.Spline.Count;
        splineAnimateObj.NormalizedTime =
            currentSpline.Spline.Closed
                ? (float)nearestKnotIndex / count
                : (float)nearestKnotIndex / (count - 1);

        playerController.enabled = false;
        playerController.GetComponent<Animator>().enabled = false;

        playerController.transform.SetParent(splineAnimateObj.transform, false);
        playerController.transform.localPosition = new Vector3(0, 2f, 0);
        playerController.transform.localRotation = Quaternion.identity;

        isRiding = true;
        splineAnimateObj.Play();
    }

    void ExitSpline()
    {
        isRiding = false;

        splineAnimateObj.Restart(false);

        playerController.transform.SetParent(null);
        playerController.enabled = true;
        playerController.GetComponent<Animator>().enabled = true;

        OnStopRaindGoMove?.Invoke(this, EventArgs.Empty);
    }
}

