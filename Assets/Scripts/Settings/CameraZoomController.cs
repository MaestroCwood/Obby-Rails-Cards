using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class CameraZoomController : MonoBehaviour
{
    // [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private CinemachineOrbitalFollow orbitalFollow;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float smooth = 10f;
    [SerializeField] private float minRadius = 30f;
    [SerializeField] private float maxRaduis = 70f;

    [SerializeField] Slider sliderMobileZoom;
    private float targetRadius;

    private InputSystem_Actions inputSystem;

    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
        inputSystem.UI.ScrollWheel.performed += OnScroll;

     
    }

    private void Start()
    {
        sliderMobileZoom.minValue = minRadius;
        sliderMobileZoom.maxValue = maxRaduis;

        sliderMobileZoom.value = orbitalFollow.Radius;
    }

    private void OnEnable() => inputSystem.Enable();
    private void OnDisable() => inputSystem.Disable();

    private void OnScroll(InputAction.CallbackContext ctx)
    {
        float scroll = ctx.ReadValue<Vector2>().y;
        targetRadius -= scroll * zoomSpeed;
        targetRadius = Mathf.Clamp(targetRadius, minRadius, maxRaduis);

        sliderMobileZoom.SetValueWithoutNotify(targetRadius);
    }

    private void Update()
    {
        float current = orbitalFollow.Radius;
        orbitalFollow.Radius =
            Mathf.Lerp(current, targetRadius, Time.deltaTime * smooth);
    }

    public void MobileZoom(float newRadius)
    {

        targetRadius = Mathf.Clamp(newRadius, minRadius, maxRaduis);
    }
}
