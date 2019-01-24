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

	public bool set_player_prefs;

	public Vector3 player_initial_position;

	void Awake() {
		self = this;

		player_initial_position = GameObject.Find ("Player").transform.position;
		high_score = PlayerPrefs.GetInt("high_score");
		coins = PlayerPrefs.GetInt("coins");
		exp = PlayerPrefs.GetInt("exp");
	}

	void Start () {
		LevelManager.self.CheckForLevelUp(exp);
	}
	
	void Update () {
		if(((int) Mathf.Floor(Mathf.Log(enemies_killed_in_combo / 2 < 1 ? 1 : enemies_killed_in_combo / 2, 2))) + 1 > combo) {
			combo += 1;
			UiManager.self.SetCombo();
		}

		if(set_player_prefs) {
			ResetPlayerPrefs();
		}
	}

	public void GameOver() {
		SetHighScore();
		IncreamentExp();

		PlayerPrefs.SetInt("coins", coins);
		PlayerPrefs.SetInt("exp", exp);

		LevelManager.self.CheckForLevelUp(exp);
		InputManager.self.OnPauseButtonClick();
		UiManager.self.GameOver();
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

	public void ResetPlayerPrefs() {
		PlayerPrefs.SetInt("high_score", 0);
		PlayerPrefs.SetInt("coins", 0);
		PlayerPrefs.SetInt("exp", 0);

		ArrayList types = new ArrayList();
        types.AddRange((Guns[]) System.Enum.GetValues(typeof(Guns)));
        types.AddRange((SpecialAbility[]) System.Enum.GetValues(typeof(SpecialAbility)));
        foreach(System.Enum type in types) {
			PlayerPrefs.SetInt(type.ToString() + "_unlocks", 0);
        }


		PlayerPrefs.SetString("active_gun", Guns.PISTOL.ToString());
		PlayerPrefs.SetString("active_special_ability", Guns.PISTOL.ToString());
	}

}
