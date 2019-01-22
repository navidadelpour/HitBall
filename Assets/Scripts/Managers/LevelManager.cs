using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager self;

    public Dictionary<System.Enum, bool> unlocks;

    void Awake() {
        self = this;

        unlocks = new Dictionary<System.Enum, bool>();
        ArrayList types = new ArrayList();
        types.AddRange((Guns[]) Enum.GetValues(typeof(Guns)));
        types.AddRange((Item[]) Enum.GetValues(typeof(Item)));
        types.AddRange((SpecialAbility[]) Enum.GetValues(typeof(SpecialAbility)));
        foreach(System.Enum type in types) {
            if(type.ToString() != Item.NOTHING.ToString())
                unlocks.Add(type, PlayerPrefs.GetInt(type.ToString() + "_unlocks") == 1);
        }
    }

    void Start() {

    }

    void Update() {

    }

}