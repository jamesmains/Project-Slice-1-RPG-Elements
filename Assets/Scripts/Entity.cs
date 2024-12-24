using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Entity : SerializedMonoBehaviour {
    [SerializeField, FoldoutGroup("Dependencies")]
    private Equipment EntityEquipment;
    
    [SerializeField, FoldoutGroup("Dependencies")]
    private HitBox EntityHitBox;

    [SerializeField, FoldoutGroup("Settings")]
    private List<Stat> StartingStats = new();

    [SerializeField, FoldoutGroup("Status"), ReadOnly]
    private float MaxHealth;
    
    [SerializeField, FoldoutGroup("Status"), ReadOnly]
    private float CurrentHealth;
    
    [SerializeField, FoldoutGroup("Status"), ReadOnly]
    private List<Stat> CurrentStats = new();
    
    [SerializeField, FoldoutGroup("Status"), ReadOnly]
    private List<Stat> MaxStats = new();
    
    [SerializeField, FoldoutGroup("Status"), ReadOnly]
    private WeaponBehavior CurrentWeapon;

    [SerializeField, FoldoutGroup("Events")]
    public UnityEvent OnEntityDeath = new();
    
    
    [HideInInspector]
    public Action<Stat> OnStatLeveled;

    private void Awake() {
        CurrentStats = StartingStats;
        SetMaxStats(EntityEquipment.GetEquipmentStats());
        CurrentHealth = MaxHealth;
    }

    private void OnEnable() {
        EntityEquipment.OnEquipmentChanged += SetMaxStats;
    }

    private void OnDisable() {
        EntityEquipment.OnEquipmentChanged -= SetMaxStats;
    }

    public List<Stat> GetBaseStats() => CurrentStats;

    public List<Stat> GetMaxStats() => MaxStats;

    private void SetMaxStats(List<Stat> equipmentStats) {
        MaxStats = Stat.CombineStats(GetBaseStats(), equipmentStats);
        MaxHealth = 0f;
        MaxStats.ForEach(o =>  MaxHealth += o.Details.Category == StatCategory.Health ? o.Level : 0);
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0,MaxHealth);
    }
    
    public string UseItem() {
        var mainHand = EntityEquipment.GetEquippedItem(ItemType.MainHand);
        if (mainHand == null || mainHand.Details.ItemBehavior is not IAnimate animate) return null;
        if (mainHand.Details.ItemBehavior is not WeaponBehavior weapon) return null;
        CurrentWeapon = weapon;
        EntityHitBox.FlashHitBox();
        return animate.GetAnimation();
    }

    public float HitEntity(Entity target) {
        if (CurrentWeapon == null) return 0;
        float amount = 0;
        var weaponStats = CurrentWeapon.GetStats();
        foreach (var stat in CurrentStats) {
            foreach (var weaponStat in weaponStats.Where(weaponStat => weaponStat.Details == stat.Details)) {
                var statValue = MaxStats.FirstOrDefault(o=>o.Details == stat.Details);
                amount += statValue?.Level ?? stat.Level;
                
                // Todo: detect if train stat should cause some kind of notification on level up
                if(stat.TrainStat(weaponStat.Level + stat.Level * Stat.TrainValueMultiplier)) {
                    SetMaxStats(EntityEquipment.GetEquipmentStats());
                    OnStatLeveled?.Invoke(stat);
                }
            }
        }
        print($"{this.gameObject.name} is sending {amount} points of damage");
        return amount;
    }

    public float ResolveDamage(float damage) {
        foreach (var stat in MaxStats) {
            if(stat.Details.Category == StatCategory.Defence)
                damage -= stat.Level;
        }
        print($"{this.gameObject.name} took {damage} points of damage");
        return damage;
    }
    
    public void TakeDamage(float damage) {
        ResolveDamage(damage);
        CurrentHealth -= damage;
        if (CurrentHealth <= 0) Die();
    }

    private void Die() {
        print($"{this.gameObject.name} has died");
        OnEntityDeath.Invoke();
    }
}

