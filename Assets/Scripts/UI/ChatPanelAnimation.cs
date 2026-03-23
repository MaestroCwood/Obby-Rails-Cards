using UnityEngine;

public class ChatPanelAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.5f;  // длительность анимации
    [SerializeField] private LeanTweenType easeType = LeanTweenType.easeOutQuad;  // тип плавности
    [SerializeField] private float hiddenYOffset = -500f;     // смещение вниз дл€ скрытого состо€ни€ (в локальных координатах)
    [SerializeField] private bool startHidden = true;         // начинать ли со скрытым состо€нием

    private RectTransform rectTransform;
    private Vector2 shownPosition;
    private Vector2 hiddenPosition;
    private bool isVisible = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("ChatPanelAnimation requires RectTransform component!");
            enabled = false;
            return;
        }

        // «апоминаем начальное положение (показанное)
        shownPosition = rectTransform.anchoredPosition;
        // —крытое положение: сдвигаем вниз на hiddenYOffset (в локальных координатах)
        hiddenPosition = shownPosition + new Vector2(0, hiddenYOffset);

        if (startHidden)
        {
            // ”станавливаем скрытое положение без анимации
            rectTransform.anchoredPosition = hiddenPosition;
            isVisible = false;
        }
        else
        {
            isVisible = true;
        }
    }

    /// <summary>
    /// ѕоказать чат (выезжает снизу)
    /// </summary>
    public void ShowChat()
    {
        if (isVisible) return;

        LeanTween.cancel(gameObject); // отмен€ем предыдущие анимации
        // јнимируем от текущего положени€ к показанному
        rectTransform.anchoredPosition = hiddenPosition; // гарантируем стартовую позицию
        LeanTween.move(rectTransform, shownPosition, animationDuration).setEase(easeType).setOnComplete(() => isVisible = true);
        isVisible = true; // можно установить сразу, чтобы избежать повторных вызовов
    }

    /// <summary>
    /// —крыть чат (уезжает вниз)
    /// </summary>
    public void HideChat()
    {
        if (!isVisible) return;

        LeanTween.cancel(gameObject);
        LeanTween.move(rectTransform, hiddenPosition, animationDuration).setEase(easeType).setOnComplete(() => isVisible = false);
        isVisible = false;
    }

    /// <summary>
    /// ѕереключить видимость чата
    /// </summary>
    public void ToggleChat()
    {
        if (isVisible)
            HideChat();
        else
            ShowChat();
    }

    /// <summary>
    /// ѕроверить, виден ли чат
    /// </summary>
    public bool IsVisible => isVisible;
}