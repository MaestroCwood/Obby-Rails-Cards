using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public static class DisableAllShadows
{
    [MenuItem("Tools/Shadows/Disable ALL Shadows")]
    public static void DisableShadows()
    {
        int count = 0;

        // MeshRenderer
        MeshRenderer[] meshRenderers = Object.FindObjectsOfType<MeshRenderer>(true);
        foreach (var mr in meshRenderers)
        {
            Undo.RecordObject(mr, "Disable Shadows");
            mr.shadowCastingMode = ShadowCastingMode.Off;
            mr.receiveShadows = false;
            count++;
        }

        // SkinnedMeshRenderer
        SkinnedMeshRenderer[] skinnedRenderers = Object.FindObjectsOfType<SkinnedMeshRenderer>(true);
        foreach (var smr in skinnedRenderers)
        {
            Undo.RecordObject(smr, "Disable Shadows");
            smr.shadowCastingMode = ShadowCastingMode.Off;
            smr.receiveShadows = false;
            count++;
        }

        Debug.Log($"[Shadows] Disabled shadows on {count} renderers");
    }

    [MenuItem("Tools/Shadows/Enable ALL Shadows")]
    public static void EnableShadows()
    {
        int count = 0;

        MeshRenderer[] meshRenderers = Object.FindObjectsOfType<MeshRenderer>(true);
        foreach (var mr in meshRenderers)
        {
            Undo.RecordObject(mr, "Enable Shadows");
            mr.shadowCastingMode = ShadowCastingMode.On;
            mr.receiveShadows = true;
            count++;
        }

        SkinnedMeshRenderer[] skinnedRenderers = Object.FindObjectsOfType<SkinnedMeshRenderer>(true);
        foreach (var smr in skinnedRenderers)
        {
            Undo.RecordObject(smr, "Enable Shadows");
            smr.shadowCastingMode = ShadowCastingMode.On;
            smr.receiveShadows = true;
            count++;
        }

        Debug.Log($"[Shadows] Enabled shadows on {count} renderers");
    }
}
