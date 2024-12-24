using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public abstract class ItemBehavior {
}

public class WeaponBehavior : ItemBehavior, IStat, IAnimate, IWeapon {

    [field: SerializeField, FoldoutGroup("Settings")]
    public GameObject AttackEffect { get; set; }

    [field: SerializeField, FoldoutGroup("Settings")]
    public AnimationStates Animation { get; set; }

    [field: SerializeField, FoldoutGroup("Settings"), ListDrawerSettings(ListElementLabelName = "Details")]
    public List<Stat> WeaponStats { get; set; }

    public List<Stat> GetStats() {
        return WeaponStats;
    }


    public string GetAnimation() {
        return Animation.ToString();
    }
}

public class ArmorBehavior : ItemBehavior, IStat {
    [field: SerializeField, FoldoutGroup("Settings"), ListDrawerSettings(ListElementLabelName = "Details")]
    public List<Stat> ArmorStats { get; set; }

    public List<Stat> GetStats() {
        return ArmorStats;
    }
}

public class ConsumableBehavior : ItemBehavior, IStat {
    public List<Stat> GetStats() {
        throw new NotImplementedException();
    }
}