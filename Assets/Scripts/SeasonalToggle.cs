using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class SeasonalToggle : MonoBehaviour
{
    [SerializeField] [BoxGroup("Settings")] 
    private Season TargetSeason;

    [SerializeField] [BoxGroup("Settings")]
    private UnityEvent OnTargetSeason;
    
    [SerializeField] [BoxGroup("Settings")]
    private UnityEvent OnOffSeason;

    private void OnEnable() {
        SeasonController.OnChangeSeason.AddListener(Toggle);
    }

    private void OnDisable() {
        SeasonController.OnChangeSeason.RemoveListener(Toggle);
    }

    private void Toggle(Season season) {
        if ((season & TargetSeason) != 0) {
            OnTargetSeason.Invoke();
        }
        else OnOffSeason.Invoke();
    }
}
