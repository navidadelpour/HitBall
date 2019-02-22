using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager self;

	public bool safe_mode = true;
	public bool gameover;
	public bool paused;
	public bool started;
	public bool on_player_views = true;
	public int combo = 1;
	public int score;
	public int exp;
	public int high_score;
	public int coins;

	public float gift_time;
	public float max_gift_time;
	public float combo_time = 3f;
	private float combo_timer;
	public bool has_gift;
	private bool has_combo;

	void Awake() {
		self = this;

		high_score = PlayerPrefs.GetInt("high_score");
		coins = PlayerPrefs.GetInt("coins");
		exp = PlayerPrefs.GetInt("exp");

		gift_time = PlayerPrefs.GetFloat("gift_time");
		max_gift_time = PlayerPrefs.GetFloat("max_gift_time");
	}

	void Start () {
		Time.timeScale = 1;
	}
	
	void Update () {
		CheckGift();
	}

	public void GameOver() {
		gameover = true;

		if(!PlayerPrefsManager.self.reseted) {
			PlayerPrefs.SetInt("coins", coins);
			PlayerPrefs.SetInt("exp", exp);
			PlayerPrefs.SetInt("high_score", high_score);

			PlayerPrefs.SetFloat("gift_time", gift_time);

			LevelManager.self.CheckForLevelUp();
			SettingManager.self.Save();
		}
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
		max_gift_time = Random.Range(.5f, 1) * 1500;
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
		if(!started || gameover)
			return;
		score += combo;
		exp += combo;
		UiManager.self.SetScore ();
		LevelManager.self.SetNextGoal(exp);
	}

	public void EnemyEarn(Vector3 position) {
		IncreamentCombo();
		AudioManager.self.Play("block_destroy");
		ParticleManager.self.Spawn("Block", position);
	}

	public void IncreamentCoins() {
		coins++;
		UiManager.self.SetCoins ();
	}

	public void ResetCombo() {
		combo = 1;
		UiManager.self.SetCombo();
	}

	public void IncreamentCombo() {
		combo += 1;
		UiManager.self.SetCombo();
		combo_timer = combo_time;
		if(!has_combo)
			StartCoroutine(ComboTimer());
	}

	IEnumerator ComboTimer() {
		has_combo = true;
        while(combo_timer > 0) {
            UiManager.self.SetComboSlider(combo_timer / combo_time);
            combo_timer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        UiManager.self.SetComboSlider(combo_timer / combo_time);

		has_combo = false;
		ResetCombo();
	}

	private void OnApplicationQuit() {
		GameOver();
	}

	public void OnExitButtonClick() {
		Application.Quit();
	}

	public void OnGiftButtonClick() {
		int gift_coin = Random.Range(1, 21) * 5;
		coins += gift_coin;
		SetGift();
		UiManager.self.DisableGift(gift_coin);
		UiManager.self.SetCoins();
	}

}
