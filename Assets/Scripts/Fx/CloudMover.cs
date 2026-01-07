using UnityEngine;

public class CloudMover : MonoBehaviour
{
    [Header("Основные параметры")]
    public Vector3 direction = Vector3.right;
    [Range(1f, 100f)] public float distance = 20f;
    [Range(0.1f, 10f)] public float duration = 8f;
    public bool pingPong = true;

    [Header("Вариация высоты")]
    [Range(0f, 5f)] public float heightAmplitude = 1f;
    [Range(0.5f, 10f)] public float heightFrequency = 0.5f;

    [Header("Рандомизация начала")]
    public bool randomizeStart = true;
    public float maxRandomOffset = 5f;

    private Vector3 startPos;
    private float startYOffset;
    private LTDescr _tween;

    void Start()
    {
        startPos = transform.position;

        if (randomizeStart)
        {
            float randOffset = Random.Range(-maxRandomOffset, maxRandomOffset);
            transform.position += direction.normalized * randOffset;
        }

        startYOffset = Random.Range(-heightAmplitude, heightAmplitude);

        StartMove();
    }

    void StartMove()
    {
        Vector3 target = startPos + direction.normalized * distance;

        if (pingPong)
        {
            _tween = LeanTween.move(gameObject, target, duration)
                .setEaseLinear()
                .setLoopPingPong();
        }
        else
        {
            _tween = LeanTween.move(gameObject, target, duration)
                .setEaseLinear()
                .setLoopClamp(); // телепорт после достижения конца
        }
    }

    void Update()
    {
        // только вертикальное движение — не ломаем LeanTween
        if (heightAmplitude > 0)
        {
            float yOffset = startYOffset + Mathf.Sin(Time.time * heightFrequency) * heightAmplitude;
            Vector3 pos = transform.position;
            pos.y = startPos.y + yOffset;
            transform.position = pos;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying) return;
        Vector3 center = transform.position;
        Vector3 offset = direction.normalized * (distance * 0.5f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(center - offset, center + offset);
        Gizmos.DrawWireSphere(center - offset, 0.5f);
        Gizmos.DrawWireSphere(center + offset, 0.5f);
    }
}
