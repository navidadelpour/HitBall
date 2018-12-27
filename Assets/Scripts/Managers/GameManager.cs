using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager self;
	public bool has_shield;
	public bool has_magnet;
	public bool has_coil;
	public bool should_remove_coil;
	public bool game_over;
	public bool paused;
	public int score;
	public int coins;

	public bool item_activated;
	public Item item;
	private float shield_adding_time;
	private float magnet_adding_time;
	private float max_shield_time = 3f;
	private float max_magnet_time = 3f;
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
		if (has_coil && !should_remove_coil)
			Invoke ("SetShouldRemoveCoil", 1f);

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

	}

	void ResetGame() {
		if (game_over) {
			game_over = false;
			GameObject.Find ("Player").transform.position = player_initial_position;
		}
	}

	void SetShouldRemoveCoil() {
		should_remove_coil = true;
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
