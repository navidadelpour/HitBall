using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public bool has_shield;
	public bool game_over;
	public int score;

	void Init() {
		instance = this;
	}

	void Start () {
		Init ();
		InvokeRepeating ("ResetGame", 2f, 2f);
	}
	
	void Update () {
		GameObject.Find ("Player").GetComponent<SpriteRenderer> ().color = new Color(255, 255, 255, game_over ? 0 : 1);
	}

	void ResetGame() {
		game_over = false;
	}

}
