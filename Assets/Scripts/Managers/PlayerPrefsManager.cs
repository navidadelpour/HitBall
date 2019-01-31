using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour {
    public static PlayerPrefsManager self;
    public int locked = 0;
    public int unlock = 1;
    public int active = 2;

    public bool should_reset;
    public bool reseted;

    void Awake() {
        self = this;
        Initialize();
    }

    void Start() {

    }

    void Update() {
        if(should_reset) {
            ResetPrefs();
            should_reset = false;
            reseted = true;
        }
    }

    public void Initialize() {
        if(PlayerPrefs.GetInt("Initialized") == 0) {
            ResetPrefs();

            PlayerPrefs.SetInt(SpecialAbility.LUCKY.ToString(), active);
            PlayerPrefs.SetInt(Guns.PISTOL.ToString(), active);

            PlayerPrefs.SetInt("DefaultTheme", active);
            PlayerPrefs.SetInt("RGBA(0, 0, 0, 0)", active);

            PlayerPrefs.SetInt("Initialized", 1);
        }
    }

    public void ResetPrefs() {
        PlayerPrefs.SetInt("Initialized", 0);

        ArrayList types = new ArrayList();
        types.AddRange((Guns[]) Enum.GetValues(typeof(Guns)));
        types.AddRange((SpecialAbility[]) Enum.GetValues(typeof(SpecialAbility)));
        foreach(System.Enum type in types) {
            PlayerPrefs.SetInt(type.ToString(), unlock);
        }


        string[] pathes = {"ThemesIcons", "Faces"};
        foreach(string path in pathes) {
            Sprite[] sprite_array = Resources.LoadAll<Sprite>("Textures/");
            string name;
            foreach(Sprite sprite in sprite_array) {
                name = sprite.name.Split(new String[] {"_"}, StringSplitOptions.None)[0];
                PlayerPrefs.SetInt(name, unlock);
            }
        }

        Color32[] color_array = new Color32[] {Color.clear, Color.red, Color.blue, Color.green, Color.yellow, Color.magenta};
        foreach(Color32 color in color_array) {
            PlayerPrefs.SetInt(color.ToString(), unlock);
        }

        PlayerPrefs.SetInt("current_level", 1);
        PlayerPrefs.SetInt("item_slots_unlocks", 1);
        PlayerPrefs.SetInt("coins", 0);
		PlayerPrefs.SetInt("exp", 0);
        PlayerPrefs.SetInt("high_score", 0);

    }

}