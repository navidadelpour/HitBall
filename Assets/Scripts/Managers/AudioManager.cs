using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager self;

    private AudioSource main_music;
    private AudioSource coin;
    private AudioSource player_die;
    private AudioSource gun_shot;
	
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
        }
       to_play.Play();
    }

}
