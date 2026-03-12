using TMPro;
using UnityEngine;

public class ManagerBrainrot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countBrainrotText;
    [SerializeField] BasePoint[] allBase;
    [SerializeField] GenerateCoin generatorCoin;

    private void Start()
    {
        // При старте синхронизируем UI и сохранение с реальным состоянием баз
        Invoke("UpdateUi", .3f);
        
    }

    private void OnEnable()
    {
        GameEvents.OnAddBrainrot += UpdateUi;
    }

    private void OnDisable()
    {
        GameEvents.OnAddBrainrot -= UpdateUi;
    }

    // Подсчёт количества занятых баз (где есть дочерний BrainrotItem)
    private int GetCurrentCount()
    {
        int count = 0;
        foreach (BasePoint basePoint in allBase)
        {
            if (basePoint.GetComponentInChildren<BrainrotItem>() != null)
                count++;
        }

        if(count > 0)
        {
            generatorCoin.IsActivateGenerator(true);
            SetCountGenerate(count);
        }
        return count;
    }

    // Обновление текста и сохранение
    private void UpdateUi()
    {
        int currentCount = GetCurrentCount();
        countBrainrotText.text = currentCount.ToString();
        SaveBrain(currentCount);
    }

    private void SaveBrain(int count)
    {
        PlayerPrefs.SetInt("Brainrot", count);
        PlayerPrefs.Save();
    }

    void SetCountGenerate(int countBrain)
    {
        switch(countBrain)
        {
            case 0:
                generatorCoin.SetCointGenerateCoin(0);
                break;
            case 1:
                generatorCoin.SetCointGenerateCoin(25);
                break;
            case 2:
                generatorCoin.SetCointGenerateCoin(55);
                break;
            case 3:
                generatorCoin.SetCointGenerateCoin(75);
                break;
            case 4:
                generatorCoin.SetCointGenerateCoin(100);
                break;
            case 5:
                generatorCoin.SetCointGenerateCoin(125);
                break;
            case 6:
                generatorCoin.SetCointGenerateCoin(155);
                break;
            case 7:
                generatorCoin.SetCointGenerateCoin(185);
                break;
            case 8:
                generatorCoin.SetCointGenerateCoin(200);
                break;
            case 9:
                generatorCoin.SetCointGenerateCoin(225);
                break;
            case 10:
                generatorCoin.SetCointGenerateCoin(250);
                break;
        }
    }
}