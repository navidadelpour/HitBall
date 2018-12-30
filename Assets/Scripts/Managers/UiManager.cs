using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

	public static UiManager self;

	public Text score_text;
	public Text high_score_text;
	public Text coins_text;

	public Button item_button;
	public Button reset_button;

	void Awake() {
		self = this;

		score_text = GameObject.Find ("ScoreText").GetComponent<Text>();
		high_score_text = GameObject.Find("HighScoreText").GetComponent<Text>();
		coins_text = GameObject.Find ("CoinsText").GetComponent<Text>();

		item_button = GameObject.Find ("ItemButton").GetComponent<Button>();
		reset_button = GameObject.Find ("ResetButton").GetComponent<Button>();
	}

	void Start () {
		reset_button.gameObject.SetActive(false);
		SetScore ();
		SetHighScore();
		SetCoins ();
	}
	
	void Update () {
		
	}

	public void GameOver() {
		reset_button.gameObject.SetActive(true);
	}

	public void SetHighScore() {
		high_score_text.text = "HIGH SCORE: " + GameManager.self.high_score; 
	}
		
	public void SetScore() {
		score_text.text = GameManager.self.score + "";
	}

	public void SetCoins() {
		coins_text.text = GameManager.self.coins + "";
	}

	public void SetItem() {
		item_button.GetComponent<Image>().sprite = Resources.Load<Sprite>("textures/Items/" + ItemManager.self.item.ToString().ToLower());
	}

}
