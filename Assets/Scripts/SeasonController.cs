using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[Flags]
public enum Season {
    Any = 0,
    Summer = 1 << 0,
    Fall = 1 << 1,
    Winter = 1 << 2,
    Spring = 1 << 3
}

public class SeasonController : MonoBehaviour {
    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    public Season CurrentSeason = Season.Summer;

    [SerializeField] [FoldoutGroup("Status")] [ReadOnly]
    private int SeasonIndex = 0;

    public static readonly UnityEvent<Season> OnChangeSeason = new();

    [Button]
    private void ChangeSeason() {
        SeasonIndex++;
        SeasonIndex %= 4;
        CurrentSeason = (Season)(1 << SeasonIndex);
        OnChangeSeason.Invoke(CurrentSeason);
    }

    private void Start() {
        OnChangeSeason.Invoke(CurrentSeason);
    }
}