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
    private int level_factor;

    void Awake() {
        self = this;

        level_factor = 2;
        levels = new System.Enum[] {
            Item.NOTHING,
            SpecialAbilities.ENEMY_EARNER,
            Guns.RIFLE,
            SpecialAbilities.BOUNCY,
            Item.NOTHING,
            SpecialAbilities.GUNNER,
            Guns.SHOTGUN,
            SpecialAbilities.RANDOMER,
        };

        current_level = PlayerPrefs.GetInt("current_level");
        item_slots_unlocks = PlayerPrefs.GetInt("item_slots_unlocks");
    }

    void Start() {

    }

    void Update() {

    }

    public void CheckForLevelUp(string indexes = "") {
        if(GameManager.self.exp > Mathf.Pow(level_factor, current_level)) {
            indexes += (current_level - 1) + "_";
            if(levels[current_level - 1].ToString() == Item.NOTHING.ToString()) {
                item_slots_unlocks ++;
                PlayerPrefs.SetInt("item_slots_unlocks", item_slots_unlocks);
                Debug.Log("item slot unlocked");
            } else {
                PlayerPrefs.SetInt(levels[current_level - 1].ToString(), 0);
                Debug.Log(levels[current_level - 1].ToString() + " unlocked");
            }
            current_level ++;
            PlayerPrefs.SetInt("current_level", current_level);
            CheckForLevelUp(indexes);
        } else {
            UiManager.self.HandleItemSlots();
            PlayerPrefs.SetString("indexes", indexes);
            Debug.Log(indexes);
        }
    }

    public int GetNextGoal() {
        return (int) Mathf.Pow(level_factor, current_level + 1);
    }

}