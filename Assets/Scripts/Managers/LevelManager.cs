using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager self;

    public Dictionary<System.Enum, bool> unlocks;
    public int item_slots_unlocks = 1;
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

    }

    void Start() {
        CheckForLevelUp();
    }

    void Update() {

    }

    public void CheckForLevelUp() {
        if(GameManager.self.exp > Mathf.Pow(5, current_level)) {
            current_level ++;
            if(levels[current_level - 1].ToString() == Item.NOTHING.ToString()) {
                item_slots_unlocks ++;
                UiManager.self.CheckForLevelUp();
            } else {
                PlayerPrefs.SetInt(levels[current_level - 1].ToString(), 0);
            }
            CheckForLevelUp();
        }
    }

}