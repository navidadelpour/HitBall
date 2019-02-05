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
	public Color32[] colors = new Color32[] {Color.white, Color.red, Color.blue, Color.green, Color.yellow, Color.magenta};

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

            PlayerPrefs.SetInt(SpecialAbilities.LUCKY.ToString(), active);
            PlayerPrefs.SetInt(Guns.PISTOL.ToString(), active);

            PlayerPrefs.SetInt("default", active);
            PlayerPrefs.SetInt("0.Color", active);

            PlayerPrefs.SetInt("Initialized", 1);

			PlayerPrefs.SetFloat("gift_time", 0);
			PlayerPrefs.SetFloat("max_gift_time", 0);
            GameManager.self.SetGift();
        }
    }

    public void ResetPrefs() {
        PlayerPrefs.SetInt("Initialized", 0);

        ArrayList types = new ArrayList();
        types.AddRange((Guns[]) Enum.GetValues(typeof(Guns)));
        types.AddRange((SpecialAbilities[]) Enum.GetValues(typeof(SpecialAbilities)));
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

        for(int i = 0; i < colors.Length; i++) {
            PlayerPrefs.SetInt(i + ".Color", unlock);
        }

        PlayerPrefs.SetInt("current_level", 1);
        PlayerPrefs.SetInt("item_slots_unlocks", 1);
        PlayerPrefs.SetInt("coins", 0);
		PlayerPrefs.SetInt("exp", 0);
        PlayerPrefs.SetInt("high_score", 0);

        PlayerPrefs.SetInt("has_music", 1);
        PlayerPrefs.SetInt("has_sfx", 1);
        PlayerPrefs.SetInt("night_mode", 0);
    }

}