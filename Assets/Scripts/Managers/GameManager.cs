using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public bool has_shield;
	public bool has_coil;
	public bool should_remove_coil;
	public bool game_over;
	public bool paused;
	public int score;
	public int coins;

	void Awake() {
		instance = this;
	}

	void Init() {

	}

	void Start () {
		Init ();
		InvokeRepeating ("ResetGame", 3f, 3f);
	}
	
	void Update () {
		GameObject.Find ("Player").GetComponent<SpriteRenderer> ().color = new Color(255, 255, 255, game_over && (int) (Time.time * 10) % 2 == 0 ? 0 : 1);
		if (has_coil && !should_remove_coil)
			Invoke ("SetShouldRemoveCoil", 1f);
	}

	void ResetGame() {
		if (game_over) {
			game_over = false;
			GameObject.Find ("Player").transform.position = Vector3.right * -5f;
		}
	}

	void SetShouldRemoveCoil() {
		should_remove_coil = true;
	}

	public void IncreamentScore() {
		score++;
		UiManager.instance.SetScore ();
	}

	public void IncreamentCoins() {
		coins++;
		UiManager.instance.SetCoins ();
	}


}
