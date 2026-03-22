
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[System.Serializable]
public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
{
    public AssetReferenceAudioClip(string guid) : base(guid)
    {
    }
}

public class MusicLoader : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    [SerializeField] AssetReferenceAudioClip assetMusicBg;
    
    // [SerializeField] TextMeshProUGUI textLog;

    private void Start()
    {
        //Log("Start!");
        assetMusicBg.LoadAssetAsync().Completed += MusicLoader_Completed1;
        Debug.Log("SSS");
        //Log("Subscribe!");
    }

    private void MusicLoader_Completed1(AsyncOperationHandle<AudioClip> handle)
    {
        musicSource.clip = handle.Result;
        musicSource.Play();
    }

    private void MusicLoader_Completed(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<AudioClip> clip)
    {
        // Log("OnMusicLoader_Completed");

        // ✅ ПРОВЕРЯЕМ СТАТУС И РЕЗУЛЬТАТ:
        if (clip.Status == AsyncOperationStatus.Succeeded && clip.Result != null)
        {
            musicSource.clip = clip.Result;

            //Log("✅ Play!");
        }
        else
        {
            string error = clip.OperationException?.Message ?? "Unknown error";
            //   Log($"❌ Failed to load music: {error}");

            // Опционально: пробуем воспроизвести дефолтный звук или молчим
            // musicSource.clip = fallbackClip;
        }
    }





    //void Log(string msg)
    //{
    //    textLog.text = msg;
    //}
}