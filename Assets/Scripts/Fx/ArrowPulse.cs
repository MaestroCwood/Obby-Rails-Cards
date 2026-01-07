using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ArrowPulse : MonoBehaviour
{
    [Header("Pulse")]
    public float scaleFrom = 1f;
    public float scaleTo = 1.15f;
    public float scaleTime = 0.6f;

    [Header("Color")]
    public Color colorFrom = Color.white;
    public Color colorTo = Color.yellow;
    public float colorTime = 0.6f;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        StartPulse();
    }

    void OnDisable()
    {
        LeanTween.cancel(gameObject);
    }

    void StartPulse()
    {
        // SCALE PULSE
        LeanTween.scale(gameObject, Vector3.one * scaleTo, scaleTime)
            .setEaseInOutSine()
            .setLoopPingPong();

        // COLOR PULSE
        LeanTween.value(gameObject, colorFrom, colorTo, colorTime)
            .setEaseInOutSine()
            .setLoopPingPong()
            .setOnUpdate((Color c) =>
            {
                sr.color = c;
            });
    }
}
