using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;

public class ActivateInnap : MonoBehaviour
{
    public static ActivateInnap Instance;
    public List<IdSkinGameObject> skins;
    public Animator playerAvatar;
    public IdSkinGameObject CurrentSkin { get; private set; }
    private void Awake() => Instance = this;

    private void OnEnable()
    {
        YG2.onGetSDKData += OnProgressLoaded;
    }

    private void OnDisable()
    {
        YG2.onGetSDKData -= OnProgressLoaded;
     
    }

    private void OnProgressLoaded()
    {
        //Debug.Log("YG2 progress loaded, now checking saved skins...");
        LoadSavedSkin();
    }

    private void LoadSavedSkin()
    {
        for (int i = 0; i < skins.Count; i++)
        {
            string savedId = YG2.saves.IDskins[i];
          //  Debug.Log($"Checking index {i}, savedId={savedId}");

            if (!string.IsNullOrEmpty(savedId) && savedId != "0")
            {
                var skin = skins.FirstOrDefault(x => x.Id == savedId);
                if (skin != null)
                {
             //       Debug.Log("Found saved skin: " + skin.Id);
                    ApplySkin(skin);
                    return;
                }
            }
        }
      // Debug.Log("No saved skin found, default model stays active");
    }

    public void ApplySkin(IdSkinGameObject skin)
    {
        foreach (var s in skins)
            s.gameObject.SetActive(false);

        skin.gameObject.SetActive(true);

        if (skin.Avatar != null)
        {
            playerAvatar.avatar = skin.Avatar;
            playerAvatar.Rebind();
            playerAvatar.Update(0);
           // Debug.Log("Applied Avatar: " + skin.Id);
        }
        else
        {
            //Debug.LogWarning("No Avatar set on skin: " + skin.Id);
        }
        CurrentSkin = skin; // сохраняем текущий активный
    }
}
