using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using System.Collections;

public class DebugLoader : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private AssetReference _sceneToLoad;
    [SerializeField] private Text _logText; // UI Text для вывода логов

    [Header("Debug")]
    [SerializeField] private bool _dontDestroy = true;

    private void Awake()
    {
        if (_dontDestroy)
            DontDestroyOnLoad(gameObject); // ❗ КРИТИЧНО: чтобы объект не умер при смене сцены

        Log("🔧 Loader initialized");
    }

    public void LoadScene()
    {
        Log("🚀 Start loading scene...");

        // Загружаем сцену
        var handle = _sceneToLoad.LoadSceneAsync();

        // Подписываемся на прогресс
        handle.Completed += OnSceneLoaded;

        // Опционально: мониторим прогресс (для длинных загрузок)
        StartCoroutine(MonitorProgress(handle));
    }

    private IEnumerator MonitorProgress(AsyncOperationHandle<SceneInstance> handle)
    {
        while (!handle.IsDone)
        {
            Log($"⏳ Loading progress: {handle.PercentComplete * 100:F1}%");
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> handle)
    {
        Log($"✅ Load completed. Status: {handle.Status}");

        try
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Log("🎉 Scene loaded SUCCESS!");

                // Опционально: уничтожаем лоадер после успешной загрузки
                // Destroy(gameObject);
            }
            else
            {
                string error = handle.OperationException?.Message ?? "Unknown error";
                Log($"❌ LOAD FAILED: {error}", true);

                // Stack trace если есть
                if (handle.OperationException != null)
                    Log($"📋 Stack: {handle.OperationException.StackTrace}");
            }
        }
        catch (System.Exception ex)
        {
            Log($"💥 CRASH in callback: {ex.Message}", true);
            Log($"📋 Stack: {ex.StackTrace}");
        }
    }

    // === UI Логгер ===
    public void Log(string message, bool isError = false)
    {
        string timestamp = $"[{System.DateTime.Now:HH:mm:ss.fff}]";
        string prefix = isError ? "❌ " : "ℹ️ ";
        string fullMsg = $"{timestamp} {prefix}{message}";

        Debug.Log(fullMsg); // Дублируем в консоль

        if (_logText != null)
        {
            _logText.text += fullMsg + "\n";
            // Прокрутка вниз (если Text в ScrollView)
            // Canvas.ForceUpdateCanvases();
        }
    }

    // Кнопка для теста (можно повесить на UI-кнопку)
    public void OnButtonClick() => LoadScene();
}