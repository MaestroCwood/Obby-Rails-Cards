
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using YG;
using YG.Utils.LB;

public class LeadBordTime : MonoBehaviour
{
    [SerializeField] GameObject itemPlayerPrefab;
    [SerializeField] Transform holdInstatiatePos;

    double totalTime;
    double lbSaveTime;
    List<GameObject> itemsList;
    void Start()
    {

        GetLb();
        StartCoroutine(SetTimeLbYg());
    }
    private void OnEnable()
    {   
        itemsList = new List<GameObject>();
        YG2.onGetLeaderboard += OnGetLeadrBoard;
    }

    private void OnDisable()
    {
        YG2.onGetLeaderboard -= OnGetLeadrBoard;
    }

    void GetLb()
    {
        YG2.GetLeaderboard("time", 6, 1);
        
    }

    private void OnGetLeadrBoard(LBData data)
    {   
        
        for(int i =0; i <itemsList.Count ; i++)
        {
            Destroy(itemsList[i].gameObject);
            
        }
        itemsList.Clear();
        var players = data.players;
        data.isInvertSortOrder = false;
        for (int i = 0; i < players.Length; i++)
        {
            var player = players[i];

            GameObject go = Instantiate(itemPlayerPrefab, holdInstatiatePos);
            itemsList.Add(go);
            ItemYgInfoPlayer itemSettings = go.GetComponent<ItemYgInfoPlayer>();

            itemSettings.SetName(player.name);

            itemSettings.SetMest(player.rank.ToString());
            Load(player.photo, itemSettings);
            Debug.Log(player.photo.ToString());
            TimeSpan time = TimeSpan.FromSeconds(player.score);
            itemSettings.SetTime($"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}");
        }

        lbSaveTime = data.currentPlayer.score;
    }

    private void Update()
    {
        totalTime += Time.deltaTime;
    }


    IEnumerator SetTimeLbYg()
    {   
        while (true)
        {
            yield return new WaitForSeconds(15f);

            // берем сохранённое общее время
            int totalTimeSave = PlayerPrefs.GetInt("TotalTime", 0);

            totalTimeSave += Mathf.FloorToInt((float)totalTime);
            PlayerPrefs.SetInt("TotalTime", totalTimeSave);

            // отправляем в лидерборд
            if (totalTimeSave > lbSaveTime)
            {
                YG2.SetLeaderboard("time", totalTimeSave);
                lbSaveTime = totalTimeSave;
            }
                

            
            totalTime = 0;
            yield return new WaitForSeconds(3f);
            GetLb();
        }
        
        
    }


    public void Load(string url, ItemYgInfoPlayer item)
    {
        if (string.IsNullOrEmpty(url))
            return;

        StartCoroutine(LoadIcon(url, item));
    }

    private IEnumerator LoadIcon(string url, ItemYgInfoPlayer item)
    {
        using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(url))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("Photo load failed: " + req.error);
                yield break;
            }

            Texture2D tex = DownloadHandlerTexture.GetContent(req);

            Sprite sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f)
            );

            item.SetPhoto(sprite);
        }
    }

}
