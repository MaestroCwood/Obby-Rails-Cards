using UnityEngine;

public class ChatPanelAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private LeanTweenType easeType = LeanTweenType.easeOutQuad;
    [SerializeField] private float hiddenYOffset = -500f;
    [SerializeField] private bool startHidden = true;
    [SerializeField] GameObject panelBg;
    [SerializeField] GameObject animatedTargetObject;   // объект, который анимируем

    private RectTransform targetRectTransform;   // RectTransform анимируемого объекта
    private Vector2 shownPosition;
    private Vector2 hiddenPosition;
    private bool isVisible = false;

    private void Awake()
    {
        if (animatedTargetObject == null)
        {
            Debug.LogError("animatedTargetObject не назначен в инспекторе!");
            enabled = false;
            return;
        }

        targetRectTransform = animatedTargetObject.GetComponent<RectTransform>();
        if (targetRectTransform == null)
        {
            Debug.LogError("animatedTargetObject не имеет компонента RectTransform!");
            enabled = false;
            return;
        }

        // Запоминаем начальное положение (показанное)
        shownPosition = targetRectTransform.anchoredPosition;
        // Скрытое положение: сдвигаем вниз на hiddenYOffset
        hiddenPosition = shownPosition + new Vector2(0, hiddenYOffset);

        if (startHidden)
        {
            targetRectTransform.anchoredPosition = hiddenPosition;
            isVisible = false;
            panelBg.SetActive(false);
        }
        else
        {
            isVisible = true;
        }
    }

    public void ShowChat()
    {
        if (isVisible) return;

        LeanTween.cancel(animatedTargetObject);  // отменяем предыдущие анимации на этом объекте
        panelBg.SetActive(true);
        targetRectTransform.anchoredPosition = hiddenPosition; // гарантируем стартовую позицию
        LeanTween.move(targetRectTransform, shownPosition, animationDuration)
            .setEase(easeType)
            .setOnComplete(() => isVisible = true);
    }

    public void HideChat()
    {
        if (!isVisible) return;

        LeanTween.cancel(animatedTargetObject);
        LeanTween.move(targetRectTransform, hiddenPosition, animationDuration)
            .setEase(easeType)
            .setOnComplete(() =>
            {
                isVisible = false;
                if (panelBg != null)
                    panelBg.SetActive(false);
            });
    }

    public void ToggleChat()
    {
        if (isVisible)
            HideChat();
        else
            ShowChat();
    }

    public bool IsVisible => isVisible;
}