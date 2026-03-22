using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class PostDataProcessor : IPostprocessBuildWithReport, IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report) => ProcessAllRenderers();
    public void OnPostprocessBuild(BuildReport report) => ProcessAllRenderers();

    private void ProcessAllRenderers()
    {
        var asset = UniversalRenderPipeline.asset;
        if (asset == null) return;

        // Достаём приватное поле m_RendererDataList из URP-ассета
        var rendererDataListField = typeof(UniversalRenderPipelineAsset)
            .GetField("m_RendererDataList", BindingFlags.NonPublic | BindingFlags.Instance);
        if (rendererDataListField == null) return;

        var rendererDataList = rendererDataListField.GetValue(asset) as ScriptableRendererData[];
        if (rendererDataList == null) return;

        foreach (var rendererData in rendererDataList)
        {
            if (rendererData == null) continue;

            // Находим поле postProcessData у текущего рендерера
            var postProcessDataField = rendererData.GetType()
                .GetField("postProcessData", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (postProcessDataField == null) continue;

            var postProcessData = postProcessDataField.GetValue(rendererData) as PostProcessData;
            if (postProcessData == null) continue;

            // Получаем поле textures внутри PostProcessData
            var texturesField = postProcessData.GetType().GetField("textures", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (texturesField == null) continue;

            var textures = texturesField.GetValue(postProcessData);
            if (textures == null) continue;

            // Получаем все поля внутри TextureResources
            var textureResourceFields = textures.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in textureResourceFields)
            {
                string fieldNameLower = field.Name.ToLowerInvariant();

                // Пропускаем поля, отвечающие за SMAA
              //  if (fieldNameLower.Contains("area") || fieldNameLower.Contains("search"))
                 //   continue;

                // Если поле — массив текстур, обнуляем его целиком
                if (field.FieldType.IsArray && typeof(Texture).IsAssignableFrom(field.FieldType.GetElementType()))
                {
                    field.SetValue(textures, null);
                }
                // Если поле — одиночная текстура
                else if (typeof(Texture).IsAssignableFrom(field.FieldType))
                {
                    field.SetValue(textures, null);
                }
                // Другие типы (например, bool) игнорируем — их в TextureResources скорее всего нет
            }
        }
    }
}
#endif