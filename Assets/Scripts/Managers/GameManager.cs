using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager self;

	public bool safe_game = true;
	public bool paused;
	public int score;
	public int high_score;
	public int coins;

	public Vector3 player_initial_position;

	void Awake() {
		self = this;

		player_initial_position = GameObject.Find ("Player").transform.position;
		high_score = PlayerPrefs.GetInt("high_score");
	}

	void Start () {

	}
	
	void Update () {

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

}
