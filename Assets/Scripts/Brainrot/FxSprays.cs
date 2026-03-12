using System.Collections;
using UnityEngine;

public class FxSprays : MonoBehaviour
{
    [SerializeField] GameObject[] brainrots;
    [SerializeField] GameObject sprays;
    [SerializeField] float delayChangePos;

    private Coroutine switchCoroutine;

    private void Start()
    {
        switchCoroutine = StartCoroutine(SwitchPos());
    }

    IEnumerator SwitchPos()
    {
        while (true)
        {
            int index = GetRandomAvailablePos();

            // ≈сли свободных позиций нет, остановить корутину
            if (index == -1)
            {
                Debug.Log("¬се brainrots зан€ты. ќстановка корутины.");
                yield break; // завершает корутину
            }

            sprays.transform.position = brainrots[index].transform.position;
            yield return new WaitForSeconds(delayChangePos);
        }
    }

    // ¬озвращает индекс случайной свободной позиции или -1, если нет
    int GetRandomAvailablePos()
    {
        // —обираем все свободные позиции
        System.Collections.Generic.List<int> available = new System.Collections.Generic.List<int>();
        for (int i = 0; i < brainrots.Length; i++)
        {
            BrainrotItem item = brainrots[i].GetComponent<BrainrotItem>();
            if (item != null && !item.isBasePosition)
            {
                available.Add(i);
            }
        }

        if (available.Count == 0)
            return -1; // свободных нет

        int randIndex = Random.Range(0, available.Count);
        return available[randIndex];
    }
}