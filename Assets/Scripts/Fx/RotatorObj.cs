using UnityEngine;

public class RotatorObj : MonoBehaviour
{
    [SerializeField] float speedRotate;
    [SerializeField] Vector3 direction = Vector3.up;
    [SerializeField] bool isStartRotate;

    private void Update()
    {   if(isStartRotate)
            transform.Rotate(direction * speedRotate * Time.deltaTime);
    }

    public void SetEnebled(bool enebled)
    {
        isStartRotate = enebled;
    }
}
