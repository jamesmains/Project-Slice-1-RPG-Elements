using System;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SeasonalSprite : MonoBehaviour {
    [SerializeField] [FoldoutGroup("Dependencies")] [ReadOnly]
    private SpriteRenderer SpriteRenderer;

    [SerializeField] [FoldoutGroup("Settings")]
    private Sprite SummerSprite;

    [SerializeField] [FoldoutGroup("Settings")]
    private Sprite FallSprite;

    [SerializeField] [FoldoutGroup("Settings")]
    private Sprite WinterSprite;

    [SerializeField] [FoldoutGroup("Settings")]
    private Sprite SpringSprite;

    private void Awake() {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        SeasonController.OnChangeSeason.AddListener(HandleSeasonChange);
    }

    private void OnDisable() {
        SeasonController.OnChangeSeason.RemoveListener(HandleSeasonChange);
    }

    private void HandleSeasonChange(Season season) {
        var s = GetSeasonSprite(season);
        if (s != null)
            SpriteRenderer.sprite = s;
    }

    private Sprite GetSeasonSprite(Season season) => season switch {
        Season.Summer => SummerSprite,
        Season.Fall => FallSprite,
        Season.Winter => WinterSprite,
        Season.Spring => SpringSprite,
        _ => null
    };
}