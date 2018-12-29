using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager self;
	public bool paused;
	public int score;
	public int high_score;
	public int coins;

	public bool item_activated;
	public Item item;

	public bool has_shield;
	private float shield_adding_time;
	private float max_shield_time = 3f;

	public bool has_magnet;
	private float magnet_adding_time;
	private float max_magnet_time = 3f;

	public bool has_slow_motion;
	private float slow_motion_adding_time;
	private float max_slow_motion_time = 3f;

	public bool has_zoom;
	private float zoom_adding_time;
	private float max_zoom_time = 3f;

	public bool has_teleport;
	public bool has_high_jump;

	public Vector3 player_initial_position;

	void Awake() {
		self = this;
	}

	void Init() {
		player_initial_position = GameObject.Find ("Player").transform.position;
		high_score = PlayerPrefs.GetInt("high_score");
	}

	void Start () {
		Init ();
		RemoveItem();
	}
	
	void Update () {
		// shield checker
		if(item_activated && item == Item.SHIELD) {
			RemoveItem();
			shield_adding_time = Time.time;
			has_shield = true;
		}
		has_shield &= Time.time - shield_adding_time < max_shield_time;

		// magnet checker
		if(item_activated && item == Item.MAGNET) {
			RemoveItem();
			magnet_adding_time = Time.time;
			has_magnet = true;
			GameObject[] coins_in_scene = GameObject.FindGameObjectsWithTag("Coin");
			foreach (GameObject coin in coins_in_scene) {
				coin.AddComponent<CoinMovement>();
			}
		}
		has_magnet &= Time.time - magnet_adding_time < max_magnet_time;

		// slow motion checker
		if(item_activated && item == Item.SLOW_MOTION) {
			RemoveItem();
			slow_motion_adding_time = Time.time;
			has_slow_motion = true;
		}
		has_slow_motion &= Time.time - slow_motion_adding_time < max_slow_motion_time;

		// zoom checker
		if(item_activated && item == Item.ZOOM) {
			RemoveItem();
			zoom_adding_time = Time.time;
			has_zoom = true;
		}
		has_zoom &= Time.time - zoom_adding_time < max_zoom_time;

		// teleport checker
		if(item_activated && item == Item.TELEPORT) {
			RemoveItem();
			has_teleport = true;
		}

		// high jump checher
		if(item_activated && item == Item.HIGH_JUMP) {
			RemoveItem();
			has_high_jump = true;
		}
	}

	public void GameOver() {
		SetHighScore();
		Pause();
		UiManager.self.GameOver();
	}

	public void ResetGame() {
		Pause();
		SceneManager.LoadScene("Scene1");
	}

	public void Pause() {
		if(GameManager.self.paused)
			Time.timeScale = 1;
		else
			Time.timeScale = 0;
		GameManager.self.paused = !GameManager.self.paused;
	}

	public void SetHighScore() {
		if(score > high_score) {
			high_score = score;
			PlayerPrefs.SetInt("high_score", high_score);
			UiManager.self.SetHighScore();
		}
	}


	public void IncreamentScore() {
		score++;
		UiManager.self.SetScore ();
	}

	public void IncreamentCoins() {
		coins++;
		UiManager.self.SetCoins ();
	}

	public void SetItem(Item item) {
		this.item = item;
		UiManager.self.SetItem();
	}

	public void RemoveItem() {
		item = Item.NOTHING;
		item_activated = false;
		UiManager.self.SetItem();
	}


}
