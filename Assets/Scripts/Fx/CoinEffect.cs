using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinEffect : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private GameObject coinPrefab; // Префаб монеты (желательно — круглая иконка)
    [SerializeField] private Transform coinCounterPosition; // Куда монеты будут лететь (например, иконка счёта)
    [SerializeField] private Transform canvas; // Ссылка на Canvas

    [Header("Анимация")]
    [SerializeField] private float flyDuration = 0.8f; // Время полёта
    [SerializeField] private float popUpDuration = 0.3f; // Время "всплыва" в начале
    [SerializeField] private float scaleUp = 1.5f; // Увеличение при появлении
    [SerializeField] private int coinCount = 5; // Сколько монет создавать

    private static CoinEffect instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        GameEvents.OnTimeRewardedComplited += OnTimeRewarded;
    }

    private void OnDisable()
    {
        GameEvents.OnTimeRewardedComplited -= OnTimeRewarded;
    }

    private void OnTimeRewarded(int countCoin, Enum type)
    {   
        switch (type)
        {
            case TypeNagrada.Coin:
                Play(10);
                GameManager.instance.AddCoin(countCoin);
                break;
        }
       
    }

    /// <summary>
    /// Вызывай этот метод откуда угодно: CoinEffect.Play(10);
    /// </summary>
    /// <param name="amount">Количество монет (будет 5–10 штук, не больше)</param>
    public static void Play(int amount)
    {
        if (instance == null) return;

        instance.StartCoroutine(instance.SpawnCoins(amount));
    }

    private System.Collections.IEnumerator SpawnCoins(int amount)
    {
        Vector3 startPosition = canvas.position; // Можно уточнить: центр экрана

        // Немного случайности в стартовой позиции
        Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-100f, 100f), UnityEngine.Random.Range(-100f, 100f), 0);

        int count = Mathf.Clamp(amount, 1, coinCount); // Ограничим количество

        List<GameObject> coins = new List<GameObject>();

        // Создаём монеты
        for (int i = 0; i < count; i++)
        {
            GameObject coin = Instantiate(coinPrefab, startPosition + randomOffset, Quaternion.identity, canvas);
            coins.Add(coin);

            // Анимация появления
            coin.transform.localScale = Vector3.zero;
            LeanTween.scale(coin, Vector3.one * scaleUp, popUpDuration)
                .setEase(LeanTweenType.easeOutElastic)
                .setOnComplete(() =>
                {
                    LeanTween.scale(coin, Vector3.one, 0.1f); // Вернуть к норме
                    GameManager.instance.AddCoin(GameManager.instance.GetCurrentGenerate());
                });

            yield return new WaitForSeconds(0.05f); // Задержка между появлением
        }

        // Анимация полёта
        Vector3 targetPosition = coinCounterPosition.position;
        float delay = 0f;

        foreach (GameObject coin in coins)
        {
            // Полёт по дуге
            LeanTween.move(coin, targetPosition, flyDuration)
                .setDelay(delay)
                .setEase(LeanTweenType.easeInQuad);

            // Поворот монеты
            LeanTween.rotateZ(coin, 1080f, flyDuration)
                .setDelay(delay)
                .setEase(LeanTweenType.linear);

            // Уничтожаем после анимации
            Destroy(coin, flyDuration + delay + 0.5f);

            delay += 0.1f;
        }

        // Можно добавить звук

    }
}