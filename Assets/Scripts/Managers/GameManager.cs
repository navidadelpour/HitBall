using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public bool has_shield;
	public bool game_over;
	public bool paused;
	public int score;
	public int coins;

	void Init() {
		instance = this;
	}

	void Start () {
		Init ();
		InvokeRepeating ("ResetGame", 3f, 3f);
	}
	
	void Update () {
		GameObject.Find ("Player").GetComponent<SpriteRenderer> ().color = new Color(255, 255, 255, game_over && (int) (Time.time * 10) % 2 == 0 ? 0 : 1);
	}

	void ResetGame() {
		if (game_over) {
			game_over = false;
			GameObject.Find ("Player").transform.position = Vector3.right * -5f;
		}
	}

}
