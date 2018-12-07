using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

	public static UiManager instance;

	public Text score_text;
	public Text coins_text;

	void Awake() {
		instance = this;
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
		if(GameManager.instance.paused)
			Time.timeScale = 1;
		else
			Time.timeScale = 0;
		GameManager.instance.paused = !GameManager.instance.paused;
	}

	public void SetScore() {
		score_text.text = GameManager.instance.score + "";
	}

	public void SetCoins() {
		coins_text.text = GameManager.instance.coins + "";
	}

}
