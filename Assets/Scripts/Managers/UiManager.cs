using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

	public static UiManager self;

	public Text score_text;
	public Text coins_text;

	void Awake() {
		self = this;
	}

	void Init() {
		score_text = GameObject.Find ("ScoreText").GetComponent<Text>();
		coins_text = GameObject.Find ("CoinsText").GetComponent<Text>();
	}

	void Start () {
		Init ();
		SetScore ();
		SetCoins ();
	}
	
	void Update () {
		
	}

	public void PauseButtonHandler() {
		if(GameManager.self.paused)
			Time.timeScale = 1;
		else
			Time.timeScale = 0;
		GameManager.self.paused = !GameManager.self.paused;
	}

	public void SetScore() {
		score_text.text = GameManager.self.score + "";
	}

	public void SetCoins() {
		coins_text.text = GameManager.self.coins + "";
	}

}
