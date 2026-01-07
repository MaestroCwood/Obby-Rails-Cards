using UnityEngine;

public class RotatorObj : MonoBehaviour
{
    [SerializeField] float speedRotate;
    [SerializeField] Vector3 direction = Vector3.up;


    private void Update()
    {
        transform.Rotate(direction * speedRotate * Time.deltaTime);
    }
}
