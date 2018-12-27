using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager self;
	public bool game_over;
	public bool paused;
	public int score;
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

	private Vector3 player_initial_position;

	void Awake() {
		self = this;
	}

	void Init() {

	}

	void Start () {
		Init ();
		RemoveItem();
		InvokeRepeating ("ResetGame", 3f, 3f);
		player_initial_position = GameObject.Find ("Player").transform.position;
	}
	
	void Update () {
		// GameObject.Find ("Player").GetComponent<SpriteRenderer> ().color = new Color(255, 255, 255, game_over && (int) (Time.time * 10) % 2 == 0 ? 0 : 1);

		if(item_activated && item == Item.SHIELD) {
			RemoveItem();
			shield_adding_time = Time.time;
			has_shield = true;
		}
		has_shield &= Time.time - shield_adding_time < max_shield_time;

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

		if(item_activated && item == Item.SLOW_MOTION) {
			RemoveItem();
			slow_motion_adding_time = Time.time;
			has_slow_motion = true;
		}
		has_slow_motion &= Time.time - slow_motion_adding_time < max_slow_motion_time;

	}

	void ResetGame() {
		if (game_over) {
			game_over = false;
			GameObject.Find ("Player").transform.position = player_initial_position;
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

	public void RemoveItem() {
		item = Item.NOTHING;
		item_activated = false;
	}


}
