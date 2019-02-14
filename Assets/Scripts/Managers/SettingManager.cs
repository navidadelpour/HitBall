using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingManager : MonoBehaviour {

	public static SettingManager self;
    public GameObject setting_panel;

    private Toggle music;
    private Toggle sfx;
    private Toggle night_mode;
    
    public bool has_music;
    public bool has_sfx;
    public bool has_night_mode;

	void Awake() {
		self = this;

        setting_panel = GameObject.Find("SettingPanel");
        music = GameObject.Find("MusicToggle").GetComponent<Toggle>();
        sfx = GameObject.Find("SFXToggle").GetComponent<Toggle>();
        night_mode = GameObject.Find("NightModeToggle").GetComponent<Toggle>();

        has_music = PlayerPrefs.GetInt("has_music") == 1;
        has_sfx = PlayerPrefs.GetInt("has_sfx") == 1;
        has_night_mode = PlayerPrefs.GetInt("has_night_mode") == 1;

	}

	void Start () {
        UiManager.self.BringPanelsToCenter(new GameObject[]{
			setting_panel,
		});
        music.isOn = has_music;
        sfx.isOn = has_sfx;
        night_mode.isOn = has_night_mode;
	}
	
	void Update () {

	}

    public void Save() {
        PlayerPrefs.SetInt("has_music", has_music ? 1 : 0);
        PlayerPrefs.SetInt("has_sfx", has_sfx ? 1 : 0);
        PlayerPrefs.SetInt("has_night_mode", has_night_mode ? 1 : 0);
    }


    public void OnSettingButtonClick() {
		GameManager.self.on_player_views = false;
		Util.GoToPanel(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject, setting_panel);
    }

    public void OnLinkButtonClick(string link) {
        Application.OpenURL(link);
    }

    public void OnToggleButtonClick(string name) {
		AudioManager.self.Play("button");
        switch(name) {
            case "has_music":
                has_music = music.isOn;
                AudioManager.self.main_music.mute = !has_music;
                break;
            case "has_sfx":
                has_sfx = sfx.isOn;
                AudioManager.self.sfx_music.mute = !has_sfx;
                break;
            case "has_night_mode": 
                has_night_mode = night_mode.isOn;
                UiManager.self.SetTheme();
                break;

        }
    }
}
