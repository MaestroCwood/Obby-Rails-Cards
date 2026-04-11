using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[System.Serializable]
public class AssetReferenseAudioClip : AssetReferenceT<AudioClip>
{
    public AssetReferenseAudioClip(string guid) : base(guid)
    {
    }
}
public class LoaderAdress : MonoBehaviour
{
    [SerializeField] AssetReferenseAudioClip audioClip;
    [SerializeField] AudioSource audioSource;

    AsyncOperationHandle asyncOperationHandle;

    private void Start()
    {
        asyncOperationHandle = audioClip.LoadAssetAsync();
        asyncOperationHandle.Completed += AsyncOperationHandle_Completed;
    }

    private void AsyncOperationHandle_Completed(AsyncOperationHandle obj)
    {
        if(asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            audioSource.clip = obj.Result as AudioClip;
            audioSource.Play();
        }
    }
}
