using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager self;

    public AudioSource main_music;
    private AudioSource coil;
    private AudioSource coin;
    private AudioSource item_get;
    private AudioSource player_die;
    private AudioSource player_jump;
    private AudioSource gun_shot;
    private AudioSource reloading;
    private AudioSource button;
    private Dictionary<string, AudioSource> audio_sources;
	
	void Awake() {
		self = this;

        main_music = GameObject.Find("MainMusic").GetComponent<AudioSource>();
        
        audio_sources = new Dictionary<string, AudioSource>() {
            {"coin", coin = GameObject.Find("Coin").GetComponent<AudioSource>()},
            {"player_die", player_die = GameObject.Find("PlayerDie").GetComponent<AudioSource>()},
            {"gun_shot", gun_shot = GameObject.Find("GunShot").GetComponent<AudioSource>()},
        };
	}

	void Start () {
		
	}
	
	void Update () {

	}

    public void Play(string name) {
        if(!SettingManager.self.has_sfx)
            return;
        try {
            AudioSource to_play = audio_sources[name];
            to_play.Play();
        } catch(System.Exception e) {
            
        }
    }

}
