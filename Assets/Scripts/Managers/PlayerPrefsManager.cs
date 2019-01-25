using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour {
    public static PlayerPrefsManager self;
    public int locked = 0;
    public int unlock = 1;
    public int active = 2;

    public Dictionary<string, int> prefs = new Dictionary<string, int>();

    void Awake() {
        self = this;
        ResetPrefs();
    }

    void Start() {

    }

    void Update() {

    }

    public void Load() {
        ArrayList types = new ArrayList();
        types.AddRange((Guns[]) Enum.GetValues(typeof(Guns)));
        types.AddRange((SpecialAbility[]) Enum.GetValues(typeof(SpecialAbility)));
        foreach(System.Enum type in types) {
            prefs.Add(type.ToString(), PlayerPrefs.GetInt(type.ToString()));
        }


        // default values for first level of game
        prefs.Add(SpecialAbility.LUCKY.ToString(), active);
        prefs.Add(Guns.PISTOL.ToString(), active);

        prefs.Add("DefaultTheme", unlock);
        prefs.Add("RGBA(0, 0, 0, 0)", unlock);
    }

    public void Save() {

    }

    public void ResetPrefs() {
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

    }

}