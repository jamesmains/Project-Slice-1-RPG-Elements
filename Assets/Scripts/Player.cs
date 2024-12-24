using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour {
    public Entity PlayerEntity;
    public Equipment PlayerEquipment;
    public Action<List<Item>> OnItemAdded;
    public Action OnPlayerStatsChanged;
    public static Player Singleton; // You sure about that champ? ONE player?
    public static Action<Player> OnPlayerCreated;
    
    private void Awake() {
        Singleton = this;
        OnPlayerCreated?.Invoke(Singleton);
    }

    private void OnEnable() {
        OnItemAdded += PlayerEquipment.SetEquippedItems;
        PlayerEntity.OnStatLeveled += UpdatePlayerStatBreakdown;
    }

    private void OnDisable() {
        OnItemAdded -= PlayerEquipment.SetEquippedItems;
        PlayerEntity.OnStatLeveled -= UpdatePlayerStatBreakdown;
    }
    
    private void UpdatePlayerStatBreakdown(Stat leveledStat)
    {
        Debug.Log($"{leveledStat.Details.SkillName} Leveled Up! {leveledStat.Level}");
        OnPlayerStatsChanged?.Invoke();
    }
}
