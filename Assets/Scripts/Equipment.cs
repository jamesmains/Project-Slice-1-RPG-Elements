using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Equipment : MonoBehaviour {
    [SerializeField, FoldoutGroup("Status"), ReadOnly]
    private List<Stat> EquipmentStats = new List<Stat>();

    [SerializeField, FoldoutGroup("Status"), ReadOnly]
    private List<Item> EquippedItems = new();

    public Action<List<Stat>> OnEquipmentChanged;

    public void SetEquippedItems(List<Item> equippedItems) {
        EquippedItems = equippedItems;
        OnEquipmentChanged.Invoke(GetEquipmentStats());
    }

    public Item GetEquippedItem(ItemType itemType) {
        return EquippedItems.FirstOrDefault(o => o.Details.Type == itemType);
    }

    public List<Item> GetEquippedItems() {
        return EquippedItems;
    }

    public List<Stat> GetEquipmentStats() {
        EquipmentStats = new List<Stat>();
        Dictionary<StatDetails, float> stats = new();
        foreach (var itemStat in EquippedItems.SelectMany(item => item.Details.GetAllStats())) {
            if (stats.ContainsKey(itemStat.Details)) {
                stats[itemStat.Details] += itemStat.Level;
            }
            else {
                stats.Add(itemStat.Details, itemStat.Level);
            }
        }

        foreach (var stat in stats) {
            var s = new Stat();
            s.Details = stat.Key;
            s.Level = (int)stat.Value;
            EquipmentStats.Add(s);
        }

        return EquipmentStats;
    }
}