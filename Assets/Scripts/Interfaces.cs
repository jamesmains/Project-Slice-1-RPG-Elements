using System.Collections.Generic;
using UnityEngine;

public interface IStat {
    // List<StatDetails> StartingStats { get; set; }
    // List<Stat> CurrentStats { get; set; }
    public List<Stat> GetStats();
}

public interface IAnimate {
    AnimationStates Animation { get; set; }
    public string GetAnimation();
}

public interface IWeapon {
    GameObject AttackEffect { get; set; }
}