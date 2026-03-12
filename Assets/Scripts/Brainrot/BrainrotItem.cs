using UnityEngine;
[RequireComponent(typeof (BoxCollider))]
public class BrainrotItem : MonoBehaviour
{
    public bool isBasePosition { get; private set; }
    public int ID;

    public string PREFIX = "Brain";


    private void Awake()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
    }
    private void Start()
    {
        int checkSave = PlayerPrefs.GetInt(PREFIX+ID);
        if ( checkSave == 1)
        {
            SetBasePosition();
            SetSavePosition();
        }
    }

    public void SetBasePosition()
    {
        isBasePosition = true;
        SaveOcupatedState();
        TryGetComponent(out RotatorObj rotator);
        rotator.SetEnebled(true);
        GameEvents.OnAddBrainrot?.Invoke();
    }

    void SaveOcupatedState()
    {
        PlayerPrefs.SetInt(PREFIX + ID.ToString(), 1);
        BasePoint basePoint = GetComponentInParent<BasePoint>();
        if (basePoint != null) PlayerPrefs.SetInt("Base_"+ ID, basePoint.baseID);
        PlayerPrefs.Save();
    }

    void SetSavePosition()
    {
        int baseID = PlayerPrefs.GetInt("Base_" + ID);
        BasePoint[] basePoints = FindObjectsByType<BasePoint>(FindObjectsSortMode.None);
        foreach (BasePoint point in basePoints)
        {
            if (point.baseID == baseID)
            {
                gameObject.transform.SetParent(point.gameObject.transform);
                transform.localPosition = Vector3.zero;
            }
        }
    }

}
