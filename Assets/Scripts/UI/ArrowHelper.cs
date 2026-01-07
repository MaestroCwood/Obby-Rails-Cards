using UnityEngine;
using UnityEngine.UI;

public class ArrowHelper : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Camera cam;
    [SerializeField] private Image arrowImage; // ваш UI-спрайт стрелки


    // Настройки
    [Header("Visual")]
    [Tooltip("Если цель за камерой — стрелка переворачивается и слегка наклоняется")]
    [SerializeField] private bool flipWhenBehind = true;
    [Tooltip("Доп. поворот, когда цель сзади (в градусах)")]
    [SerializeField] private float behindTilt = 20f;
    [Tooltip("Плавность поворота (0 = мгновенно, 1 = очень плавно)")]
    [Range(0f, 1f)][SerializeField] private float smoothness = 0.1f;

    private Quaternion targetRotation;
    void Awake()
    {
        if (cam == null) cam = Camera.main;
        if (arrowImage == null) arrowImage = GetComponent<Image>();
    }
    // Update is called once per frame
    void Update()
    {
        if (target == null || cam == null || arrowImage == null) return;

        Vector3 screenPos = cam.WorldToScreenPoint(target.position);

        // Вектор от центра экрана к проекции цели
        Vector2 center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Vector2 screenDir = new Vector2(screenPos.x, screenPos.y) - center;

        // Основной угол (от оси X, против часовой)
        float angle = Mathf.Atan2(screenDir.y, screenDir.x) * Mathf.Rad2Deg;

        // Коррекция: если цель за камерой (z < 0), разворачиваем на 180° + наклон
        if (screenPos.z < 0 && flipWhenBehind)
        {
            angle += 180f; // разворот на 180°
            // Доп. наклон "вниз", чтобы показать: цель сзади
            angle += behindTilt * Mathf.Sign(screenDir.x); // если слева — влево-вниз, если справа — вправо-вниз
        }

        // Нормализуем угол, чтобы избежать резких скачков (например, 359 → 0)
        angle = Mathf.Repeat(angle, 360f);

        // Учитываем, что в UI Unity 0° = вправо, а стрелка обычно смотрит вверх (90° в спрайте)
        // Если ваша стрелка в исходнике смотрит ВПРАВО — уберите -90f
       // angle -= 90f; // ← критично! Иначе 0° = вправо, а не вверх

        targetRotation = Quaternion.Euler(0, 0, angle);

        // Плавное вращение
        arrowImage.rectTransform.rotation =
            Quaternion.Slerp(arrowImage.rectTransform.rotation, targetRotation, 1f - Mathf.Pow(1f - smoothness, Time.deltaTime * 60f));
    }
}
