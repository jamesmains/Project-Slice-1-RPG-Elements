using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SeasonalObjectToggle : SerializedMonoBehaviour
{
    [SerializeField] [BoxGroup("Settings")]
    private List<SeasonalObjectGroup> SeasonalGroups = new();
    
    private void OnEnable() {
        SeasonController.OnChangeSeason.AddListener(Toggle);
    }

    private void OnDisable() {
        SeasonController.OnChangeSeason.RemoveListener(Toggle);
    }

    private void Toggle(Season season) {
        foreach (var group in SeasonalGroups) {
            var isSeason = (season & group.TargetSeason) != 0;
            group.SetObjectState(isSeason);
        }
    }
}

[Serializable]
public class SeasonalObjectGroup {
    public Season TargetSeason;
    public List<GameObject> Objects = new();

    public void SetObjectState(bool state) {
        foreach (var obj in Objects) {
            obj.SetActive(state);
        }
    }
}
