using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager self;

    private AudioSource main_music;
    private AudioSource coil;
    private AudioSource coin;
    private AudioSource item_get;
    private AudioSource player_die;
    private AudioSource player_jump;
    private AudioSource gun_shot;
    private AudioSource reloading;
    private AudioSource button;
	
	void Awake() {
		self = this;

        main_music = GameObject.Find("MainMusic").GetComponent<AudioSource>();

        coin = GameObject.Find("Coin").GetComponent<AudioSource>();
        player_die = GameObject.Find("PlayerDie").GetComponent<AudioSource>();
        gun_shot = GameObject.Find("GunShot").GetComponent<AudioSource>();
	}

	void Start () {
		
	}
	
	void Update () {
        main_music.mute = !SettingManager.self.has_music;
	}

    public void Play(string name) {
        if(!SettingManager.self.has_sfx)
            return;
        AudioSource to_play = null;
        switch(name) {
            case "coin":
                to_play = coin;
                break;
            case "player_die":
                to_play = player_die;
                break;
            case "gun_shot":
                to_play = gun_shot;
                break;
            case "item_get":
                to_play = item_get;
                break;
            case "player_jump":
                to_play = player_jump;
                break;
            case "reloading":
                to_play = reloading;
                break;
            case "button":
                to_play = button;
                break;
        }
        if(to_play == null)
            return;
        to_play.Play();
    }

}
