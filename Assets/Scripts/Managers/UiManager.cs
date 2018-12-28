using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

	public static UiManager self;

	public Text score_text;
	public Text coins_text;

	public Button item_button;
	public Button reset_button;

	void Awake() {
		self = this;
	}

	void Init() {
		score_text = GameObject.Find ("ScoreText").GetComponent<Text>();
		coins_text = GameObject.Find ("CoinsText").GetComponent<Text>();

		item_button = GameObject.Find ("ItemButton").GetComponent<Button>();
		reset_button = GameObject.Find ("ResetButton").GetComponent<Button>();
	}

	void Start () {
		Init ();
		reset_button.gameObject.active = false;
		SetScore ();
		SetCoins ();
		SetItem();
	}
	
	void Update () {
		
	}

	public void GameOver() {
		reset_button.gameObject.active = true;
	}
		
	public void SetScore() {
		score_text.text = GameManager.self.score + "";
	}

	public void SetCoins() {
		coins_text.text = GameManager.self.coins + "";
	}

	public void SetItem() {
		item_button.GetComponent<Image>().sprite = Resources.Load<Sprite>("textures/Items/" + GameManager.self.item.ToString().ToLower());
	}

}
