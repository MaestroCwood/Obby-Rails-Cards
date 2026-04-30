//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;
//using GamePush;


//public class LeadBordTime_GP : MonoBehaviour
//{
//    [SerializeField] GameObject itemPlayerPrefab;
//    [SerializeField] Transform holdInstatiatePos;

//    double totalTime;
//    int lbSaveTime;

//    List<GameObject> itemsList;

//    private void OnEnable()
//    {
//        itemsList = new List<GameObject>();
//        GP_Leaderboard.OnFetchSuccess += OnFetchSuccess;
//    }

//    private void OnDisable()
//    {
//        GP_Leaderboard.OnFetchSuccess -= OnFetchSuccess;
//    }

//    void Start()
//    {
//        GetLb();
//        StartCoroutine(SetTimeLb());
//    }

//    void GetLb()
//    {
//        GP_Leaderboard.Fetch(
//            tag: "time",
//            orderBy: "score",
//            order: "DESC",
//            limit: 6,
//            showNearest: 0,
//            withMe: "none",
//            includeFields: "rank"
//        );
//    }

//    private void OnFetchSuccess(string tag, GP_Data data)
//    {
//        if (tag != "time") return;
       
//        var players = data.GetList<GP_LeaderboardPlayer>();
//        var players = data.GetList<object>();
//        // очистка
//        foreach (var item in itemsList)
//            Destroy(item);

//        itemsList.Clear();

//        for (int i = 0; i < players.Count; i++)
//        {
//            var player = players[i];

//            GameObject go = Instantiate(itemPlayerPrefab, holdInstatiatePos);
//            itemsList.Add(go);

//            ItemYgInfoPlayer itemSettings = go.GetComponent<ItemYgInfoPlayer>();

//            itemSettings.SetName(player.name);
//            itemSettings.SetMest(player.position.ToString());

//            TimeSpan time = TimeSpan.FromSeconds(player.score);
//            itemSettings.SetTime($"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}");

//            if (!string.IsNullOrEmpty(player.avatar))
//                Load(player.avatar, itemSettings);

//            if (player.isMe)
//                lbSaveTime = player.score;
//        }
//    }

//    private void Update()
//    {
//        totalTime += Time.deltaTime;
//    }

//    IEnumerator SetTimeLb()
//    {
//        while (true)
//        {
//            yield return new WaitForSeconds(15f);

//            int totalTimeSave = PlayerPrefs.GetInt("TotalTime", 0);
//            totalTimeSave += Mathf.FloorToInt((float)totalTime);
//            PlayerPrefs.SetInt("TotalTime", totalTimeSave);

//            if (totalTimeSave > lbSaveTime)
//            {
//                GP_Player.Set("score", totalTimeSave);
//                lbSaveTime = totalTimeSave;
//            }

//            totalTime = 0;

//            yield return new WaitForSeconds(3f);
//            GetLb();
//        }
//    }

//    public void Load(string url, ItemYgInfoPlayer item)
//    {
//        if (string.IsNullOrEmpty(url))
//            return;

//        StartCoroutine(LoadIcon(url, item));
//    }

//    private IEnumerator LoadIcon(string url, ItemYgInfoPlayer item)
//    {
//        using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(url))
//        {
//            yield return req.SendWebRequest();

//            if (req.result != UnityWebRequest.Result.Success)
//            {
//                Debug.LogWarning("Photo load failed: " + req.error);
//                yield break;
//            }

//            Texture2D tex = DownloadHandlerTexture.GetContent(req);

//            Sprite sprite = Sprite.Create(
//                tex,
//                new Rect(0, 0, tex.width, tex.height),
//                new Vector2(0.5f, 0.5f)
//            );

//            item.SetPhoto(sprite);
//        }
//    }
//}