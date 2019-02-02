using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager self;

	public bool safe_mode = true;
	public bool paused;
	public bool started;
	public int combo = 1;
	public int score;
	public int exp;
	public int enemies_killed_in_combo;
	public int high_score;
	public int coins;

	public float gift_time;
	public float max_gift_time;
	public bool has_gift;

	void Awake() {
		self = this;

		high_score = PlayerPrefs.GetInt("high_score");
		coins = PlayerPrefs.GetInt("coins");
		exp = PlayerPrefs.GetInt("exp");

		gift_time = PlayerPrefs.GetFloat("gift_time");
		max_gift_time = PlayerPrefs.GetFloat("max_gift_time");
	}

	void Start () {

	}
	
	void Update () {
		CheckGift();
	}

	public void GameOver() {
		SetHighScore();
		IncreamentExp();

		if(!PlayerPrefsManager.self.reseted) {
			PlayerPrefs.SetInt("coins", coins);
			PlayerPrefs.SetInt("exp", exp);
			PlayerPrefs.SetInt("high_score", high_score);

			PlayerPrefs.SetFloat("gift_time", gift_time);

			LevelManager.self.CheckForLevelUp();
			SettingManager.self.Save();
		}
		InputManager.self.OnPauseButtonClick();
		UiManager.self.GameOver();
	}

	public void SetHighScore() {
		if(score > high_score) {
			high_score = score;
			UiManager.self.SetHighScore();
		}
	}

	public void SetGift() {
		has_gift = false;
		gift_time = 0;
		max_gift_time = Random.Range(.1f, 1) * 60;
		PlayerPrefs.SetFloat("max_gift_time", max_gift_time);
	}

	public void CheckGift() {
		if(!has_gift) {
			if(gift_time < max_gift_time) {
				gift_time += Time.deltaTime;
			} else {
				has_gift = true;
				UiManager.self.EnableGift();
			}
		}
	}


	public void IncreamentScore() {
		if(!started)
			return;
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
		UiManager.self.SetCombo();
	}

	public void HandleEnemyKill() {
		enemies_killed_in_combo++;
		if(((int) Mathf.Floor(Mathf.Log(enemies_killed_in_combo / 2 < 1 ? 1 : enemies_killed_in_combo / 2, 2))) + 1 > combo) {
			combo += 1;
			UiManager.self.SetCombo();
		}
	}

	private void OnApplicationQuit() {
		GameOver();
	}

	public void OnGiftButtonClick() {
		int gift_coin = Random.Range(1, 21) * 5;
		coins += gift_coin;
		SetGift();
		UiManager.self.DisableGift();
	}

}
