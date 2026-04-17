using UnityEngine;

public class DestroyFxDelay : MonoBehaviour
{
    [SerializeField] float delay;

    float currentDelay;
    private void Awake()
    {
        currentDelay = delay;
    }

    private void Update()
    {
        if (currentDelay <= 0)
        {
            Destroy(gameObject);
            return;
        }

        currentDelay -= Time.deltaTime;
    }
}
