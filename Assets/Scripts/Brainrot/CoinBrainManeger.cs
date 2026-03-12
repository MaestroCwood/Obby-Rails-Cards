using System.Collections;
using TMPro;
using UnityEngine;

public class CoinBrainManeger : MonoBehaviour
{
    [SerializeField] TextMeshPro coinTextGenerate;
    [SerializeField] Color colorToGet;
    public int currentCoinNakopleno { get; private set; }

    private MeshRenderer meshRenderer;
    private Color startColor;
    private Coroutine colorCoroutine;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
            startColor = meshRenderer.material.color;
        else
            Debug.LogError("MeshRenderer component missing on " + gameObject.name);
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    void UpdateText()
    {
        if (coinTextGenerate != null)
            coinTextGenerate.text = $"${currentCoinNakopleno}";
    }

    public void AddCurrentNakopleno(int coinNakopleno)
    {
        currentCoinNakopleno += coinNakopleno;
        UpdateText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Плавно меняем цвет при входе игрока
            if (colorCoroutine != null)
                StopCoroutine(colorCoroutine);
            colorCoroutine = StartCoroutine(ChangeColorSmoothly(colorToGet));

            // Здесь можно добавить логику увеличения монет
            // currentCoinNakopleno += 1;
            // UpdateText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (colorCoroutine != null)
            {
                StopCoroutine(colorCoroutine);
                colorCoroutine = null;
            }
            if (meshRenderer != null)
                meshRenderer.material.color = startColor;
        }
    }

    private IEnumerator ChangeColorSmoothly(Color targetColor)
    {
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            if (meshRenderer != null)
                meshRenderer.material.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        if (meshRenderer != null)
            meshRenderer.material.color = targetColor; // гарантированно устанавливаем конечный цвет
        colorCoroutine = null;

        AudioManager.instance.PlayFx(4);
        CoinEffect.Play(currentCoinNakopleno);
        GameEvents.OnGenerateCoin(currentCoinNakopleno);
        currentCoinNakopleno = 0;
        UpdateText();
    }
}