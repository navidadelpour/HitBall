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
    public int temp_current_level;

    void Awake() {
        self = this;

        levels = new System.Enum[] {
            Items.NOTHING,                      // level 2
            SpecialAbilities.ENEMY_EARNER,
            Guns.RIFLE,
            SpecialAbilities.BOUNCY,
            Items.NOTHING,
            SpecialAbilities.GUNNER,
            Guns.SHOTGUN,
            SpecialAbilities.RANDOMER,          // level 9
        };

        current_level = PlayerPrefs.GetInt("current_level");
        item_slots_unlocks = PlayerPrefs.GetInt("item_slots_unlocks");
        temp_current_level = current_level;
    }

    void Start() {
        UiManager.self.SetLevel(temp_current_level);
        SetNextGoal(GameManager.self.exp);
    }

    void Update() {

    }

    public void CheckForLevelUp(string indexes = "") {
        if(current_level < levels.Length + 1) {
            if(temp_current_level > current_level) {
                indexes += (current_level - 1) + "_";
                if(levels[current_level - 1].ToString() == Items.NOTHING.ToString()) {
                    item_slots_unlocks ++;
                    PlayerPrefs.SetInt("item_slots_unlocks", item_slots_unlocks);
                } else {
                    PlayerPrefs.SetInt(levels[current_level - 1].ToString(), 0);
                }
                current_level ++;
                CheckForLevelUp(indexes);
            } else {
                PlayerPrefs.SetInt("current_level", current_level);
                PlayerPrefs.SetString("indexes", indexes);
            }
        }
    }

    public void SetNextGoal(int exp) {
        int goal = 0;
        if(temp_current_level < levels.Length + 1) {
            goal = LevelFunction(temp_current_level);
            int left = goal - exp;
            if(left < 0) {
                temp_current_level++;
                UiManager.self.SetLevel(temp_current_level);
                GameManager.self.exp = 0;
            } else {
                UiManager.self.SetNextGoal(left);
            }
        } else {
            UiManager.self.SetNextGoal(0, true);
        }
    }

    public int LevelFunction(int level) {
        return (int) Mathf.Pow(3, level + 1) + 1000 * (level + 1);
    }

}