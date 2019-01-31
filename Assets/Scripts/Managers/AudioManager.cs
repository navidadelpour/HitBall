using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager self;
    private AudioSource coin;
    private AudioSource player_die;
    private AudioSource gun_shot;
	
	void Awake() {
		self = this;

        coin = GameObject.Find("Coin").GetComponent<AudioSource>();
        player_die = GameObject.Find("PlayerDie").GetComponent<AudioSource>();
        gun_shot = GameObject.Find("GunShot").GetComponent<AudioSource>();
	}

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void Play(string name) {
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
