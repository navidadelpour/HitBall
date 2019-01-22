using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager self;

    public Dictionary<System.Enum, bool> unlocks;
    public int item_slots_unlocks;
    public System.Enum[] levels;
    public int current_level = 1;

    void Awake() {
        self = this;

        levels = new System.Enum[] {
            Item.NOTHING,
            SpecialAbility.ENEMY_EARNER,
            Guns.RIFLE,
            SpecialAbility.BOUNCY,
            Item.NOTHING,
            SpecialAbility.GUNNER,
            Guns.SHOTGUN,
            SpecialAbility.RANDOMER,
        };

        unlocks = new Dictionary<System.Enum, bool>();
        ArrayList types = new ArrayList();
        types.AddRange((Guns[]) Enum.GetValues(typeof(Guns)));
        types.AddRange((SpecialAbility[]) Enum.GetValues(typeof(SpecialAbility)));
        foreach(System.Enum type in types) {
            if(type.ToString() != Item.NOTHING.ToString())
                unlocks.Add(type, PlayerPrefs.GetInt(type.ToString() + "_unlocks") == 1);
        }
        
        // level 1 things
        unlocks[SpecialAbility.LUCKY] = true;
        unlocks[Guns.PISTOL] = true;
        item_slots_unlocks = 1;
    }

    void Start() {

    }

    void Update() {

    }

    public void CheckForLevelUp(int exp) {
        if(exp > Mathf.Pow(5, current_level)) {
            current_level ++;
            if(levels[current_level - 1].ToString() == Item.NOTHING.ToString()) {
                item_slots_unlocks ++;
                UiManager.self.CheckForLevelUp();
            } else {
                unlocks[levels[current_level - 1]] = true;
            }
            CheckForLevelUp(exp);
        }
    }

}