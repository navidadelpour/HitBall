using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager self;

    public Dictionary<System.Enum, bool> unlocks;
    public int item_slots_unlocks;
    public System.Enum[] levels;
    public int current_level;
    private int[] item_slot_levels = new int[] {2, };

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

        current_level = PlayerPrefs.GetInt("current_level");
        item_slots_unlocks = PlayerPrefs.GetInt("item_slots_unlocks");
    }

    void Start() {

    }

    void Update() {

    }

    public void CheckForLevelUp() {
        if(GameManager.self.exp > Mathf.Pow(5, current_level)) {
            if(levels[current_level - 1].ToString() == Item.NOTHING.ToString()) {
                item_slots_unlocks ++;
                PlayerPrefs.SetInt("item_slots_unlocks", item_slots_unlocks);
            } else {
                PlayerPrefs.SetInt(levels[current_level - 1].ToString(), 0);
            }
            current_level ++;
            PlayerPrefs.SetInt("current_level", current_level);
            CheckForLevelUp();
        } else {
            UiManager.self.HandleItemSlots();
        }
    }

    public int GetNextGoal() {
        return (int) Mathf.Pow(5, current_level + 1);
    }

}