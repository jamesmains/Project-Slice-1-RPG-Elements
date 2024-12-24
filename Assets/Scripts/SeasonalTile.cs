using System;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Tilemaps;

public class SeasonalTile : Tile {
    
    [SerializeField] [FoldoutGroup("Settings")]
    public Sprite SummerSprite;

    [SerializeField] [FoldoutGroup("Settings")]
    public Sprite FallSprite;

    [SerializeField] [FoldoutGroup("Settings")]
    public Sprite WinterSprite;

    [SerializeField] [FoldoutGroup("Settings")]
    public Sprite SpringSprite;

    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private Season CurrentSeason;

    private void OnEnable() {
        SeasonController.OnChangeSeason.AddListener(HandleSeasonChange);
    }

    private void OnDisable() {
        SeasonController.OnChangeSeason.RemoveListener(HandleSeasonChange);
    }

    private void HandleSeasonChange(Season season) {
        CurrentSeason = season;
        var s = GetSeasonSprite(season);
        if (s != null) {
            sprite = s;
        }
    }

    private Sprite GetSeasonSprite(Season season) => season switch {
        Season.Summer => SummerSprite,
        Season.Fall => FallSprite,
        Season.Winter => WinterSprite,
        Season.Spring => SpringSprite,
        _ => null
    };
#if UNITY_EDITOR
    [MenuItem("Assets/Create/2D/SeasonalTile")]
    public static void CreateSeasonalTile() {
        string path = EditorUtility.SaveFilePanelInProject("Save Seasonal Tile", "New Seasonal Tile", "Asset","Save Seasonal Tile", "Assets");
        if (path == "") return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<SeasonalTile>(), path);
    }
    #endif
}