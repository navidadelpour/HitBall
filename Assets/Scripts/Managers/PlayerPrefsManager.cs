using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour {
    public static PlayerPrefsManager self;

    public bool should_reset;
    public bool reseted;
	public Color32[] colors;

    void Awake() {
        self = this;
        colors = new Color32[] {Color.red, Color.blue, Color.green, Color.yellow, Color.magenta};
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

            PlayerPrefs.SetInt(SpecialAbilities.LUCKY.ToString(), 2);
            PlayerPrefs.SetInt(Guns.PISTOL.ToString(), 2);

            PlayerPrefs.SetInt("default", 2);
            PlayerPrefs.SetInt("0.Color", 2);
            PlayerPrefs.SetInt("mustache01", 2);
            PlayerPrefs.SetInt("beard01", 2);
            PlayerPrefs.SetInt("hat01", 2);

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
            PlayerPrefs.SetInt(type.ToString(), 1);
        }

        Items[] items = (Items[]) Enum.GetValues(typeof(Items));
		foreach (Items item in items) {
            PlayerPrefs.SetInt(item.ToString().ToLower() + "_tutorial_shown", 0);
        }

        string[] pathes = {"ThemesIcons", "Faces/Beards", "Faces/Hats", "Faces/Mustaches"};
        foreach(string path in pathes) {
            Sprite[] sprite_array = Resources.LoadAll<Sprite>("Textures/" + path);
            string name;
            foreach(Sprite sprite in sprite_array) {
                name = sprite.name.Split(new String[] {"_"}, StringSplitOptions.None)[0];
                PlayerPrefs.SetInt(name, 1);
            }
        }

        for(int i = 0; i < colors.Length; i++) {
            PlayerPrefs.SetInt(i + ".Color", 1);
        }

		PlayerPrefs.SetString("indexes", "");
        PlayerPrefs.SetInt("current_level", 1);
        PlayerPrefs.SetInt("item_slots_unlocks", 1);
        PlayerPrefs.SetInt("coins", 0);
		PlayerPrefs.SetInt("exp", 0);
        PlayerPrefs.SetInt("high_score", 0);

        PlayerPrefs.SetInt("has_music", 1);
        PlayerPrefs.SetInt("has_sfx", 1);
        PlayerPrefs.SetInt("has_night_mode", 0);
        PlayerPrefs.SetInt("tutorial_shown", 0);
    }

}