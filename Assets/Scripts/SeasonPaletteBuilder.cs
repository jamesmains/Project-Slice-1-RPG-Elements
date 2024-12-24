using System.Collections.Generic;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
using UnityEngine;

public class SeasonPaletteBuilder : MonoBehaviour {
    public string Name;
    public List<Sprite> SummerSprites;
    public List<Sprite> FallSprites;
    public List<Sprite> WinterSprites;
    public List<Sprite> SpringSprites;
    #if UNITY_EDITOR
    [Button]
    private void BuildSeasonPalette() {
        // string path = Application.dataPath + $"/Resources/Output/{Name}";
        string path = EditorUtility.SaveFilePanelInProject("Save Seasonal Tile", "New Seasonal Tile", "","Save Seasonal Tile", "Assets");
        print(path);
        if (path == "") return;
        for (int i = 0; i < SummerSprites.Count; i++) {
            string assetName = $"{path}_{i}.Asset";
            var tile = ScriptableObject.CreateInstance<SeasonalTile>();
            tile.sprite = SummerSprites[i];
            tile.SummerSprite = SummerSprites[i];
            tile.FallSprite = FallSprites[i];
            tile.WinterSprite = WinterSprites[i];
            tile.SpringSprite = SpringSprites[i];
            AssetDatabase.CreateAsset(tile, assetName);
        }
    }
    #endif
}
