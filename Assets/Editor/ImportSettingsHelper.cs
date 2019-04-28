using UnityEditor;
using UnityEngine;

public class ImportSettingsHelper : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        var textureImporter = assetImporter as TextureImporter;
        if (textureImporter != null)
        {
            TextureImporterSettings texSettings = new TextureImporterSettings();
            textureImporter.compressionQuality = 0;
            textureImporter.spritePixelsPerUnit = 20;
            textureImporter.filterMode = FilterMode.Point;
            if (textureImporter.assetPath.Contains("Chips"))
            {
                textureImporter.ReadTextureSettings(texSettings);
                texSettings.spriteAlignment = (int)SpriteAlignment.Custom;
                textureImporter.spritePivot = new Vector2(0.5f, 0.425f);
                textureImporter.SetTextureSettings(texSettings);
            }
        }
    }
}