using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using YG;



public class YGTouchscreen : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private bool invert;
    [SerializeField] private float sensitivity;
    [SerializeField] private float deadZone = 0f;

    private bool pressed = false;
    private Vector2 inputDelta = Vector2.zero;
    private bool isMobile;
    public CinemachineInputAxisController axisController;

    private void Awake()
    {
         isMobile = YG2.envir.isMobile;
        axisController.ReadControlValueOverride = ReadCameraInput;
    }

    private void Start()
    {
        //if (isMobile)
        //{
        //    sensitivity = 0.02f;

        //}
        //else
        //{
        //    sensitivity = 0.3f;
        //}
    }

    float ReadCameraInput(
     UnityEngine.InputSystem.InputAction action,
     IInputAxisOwner.AxisDescriptor.Hints hint,
     Object context,
     CinemachineInputAxisController.Reader.ControlValueReader defaultReader)
    {
        
        if (!isMobile)
        {
            if (defaultReader != null && action != null)
                return defaultReader(action, hint, context, null);

            return 0f;
        }

      
        Vector2 look = GetTouchscreenInput();
        return hint == IInputAxisOwner.AxisDescriptor.Hints.Y
            ? look.y
            : look.x;
    }

    public void SetSensitivity(float _sensitivity)
    {
        sensitivity = _sensitivity;
    }

    public void OnPointerDown(PointerEventData eventData)
    {   
       
        pressed = true;
        inputDelta = Vector2.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        inputDelta = Vector2.zero;
    }

    private void Update()
    {
        //if (Mathf.Abs(inputDelta.x) < deadZone && Mathf.Abs(inputDelta.y) < deadZone && pressed)
        //    inputDelta = Vector2.zero;
    }
    public void OnDrag(PointerEventData eventData)
    {

        Vector2 delta = eventData.delta;
        

        if (Mathf.Abs(delta.x) < deadZone && Mathf.Abs(delta.y) < deadZone)
        {
            inputDelta = Vector2.zero;
        }
        else
        {

            inputDelta = new Vector2(
                delta.x * sensitivity,
                delta.y * sensitivity
            );
        }
    }

    public Vector2 GetTouchscreenInput()
    {

        if (!pressed)
            return Vector2.zero;

        Vector2 value = invert ? -inputDelta : inputDelta;
      //  inputDelta = Vector2.zero; 
        return value;


    }

    private void LateUpdate()
    {
        inputDelta = Vector2.zero;
    }
}