using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager self;

	public bool safe_mode = true;
	public bool paused;
	public int combo = 1;
	public int score;
	public int exp;
	public int enemies_killed_in_combo;
	public int high_score;
	public int coins;

	public Vector3 player_initial_position;

	void Awake() {
		self = this;

		player_initial_position = GameObject.Find ("Player").transform.position;
		high_score = PlayerPrefs.GetInt("high_score");
		coins = PlayerPrefs.GetInt("coins");
		exp = PlayerPrefs.GetInt("exp");
	}

	void Start () {

	}
	
	void Update () {
		if(((int) Mathf.Floor(Mathf.Log(enemies_killed_in_combo / 2 < 1 ? 1 : enemies_killed_in_combo / 2, 2))) + 1 > combo) {
			combo += 1;
			UiManager.self.SetCombo();
		}
	}

	public void GameOver() {
		SetHighScore();
		IncreamentExp();

		PlayerPrefs.SetInt("coins", coins);
		PlayerPrefs.SetInt("exp", exp);

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
		score += combo;
		UiManager.self.SetScore ();
	}

	public void EnemyEarn() {
		score += 10;
	}

	public void IncreamentCoins() {
		coins++;
		UiManager.self.SetCoins ();
	}

	private void IncreamentExp() {
		exp += score;
	}

	public void ResetCombo() {
		enemies_killed_in_combo = 0;
		combo = 1;
	}

}
