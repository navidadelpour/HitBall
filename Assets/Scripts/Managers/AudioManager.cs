using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager self;

    public AudioSource main_music;
    public AudioSource sfx_music;
    private Dictionary<string, AudioClip> audio_clips;
	
	void Awake() {
		self = this;

        main_music = GameObject.Find("MainMusic").GetComponent<AudioSource>();
        sfx_music = GameObject.Find("SFXMusic").GetComponent<AudioSource>();

        audio_clips = new Dictionary<string, AudioClip>();
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio/SFX");
        foreach (AudioClip clip in clips) {
            audio_clips.Add(clip.name, clip);
        }
	}

	void Start () {
		
	}
	
	void Update () {

	}

    public void Play(string name) {
        if(!audio_clips.ContainsKey(name))
            return;
        
        sfx_music.pitch = Random.Range(.9f, 1.1f);
        sfx_music.PlayOneShot(audio_clips[name]);
    }

}
