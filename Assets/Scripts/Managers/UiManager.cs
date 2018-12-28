using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

	public static UiManager self;

	public Text score_text;
	public Text coins_text;

	public Button item_button;

	void Awake() {
		self = this;
	}

	void Init() {
		score_text = GameObject.Find ("ScoreText").GetComponent<Text>();
		coins_text = GameObject.Find ("CoinsText").GetComponent<Text>();

		item_button = GameObject.Find ("ItemButton").GetComponent<Button>();
	}

	void Start () {
		Init ();
		SetScore ();
		SetCoins ();
		SetItem();
	}
	
	void Update () {
		
	}
		
	public void SetScore() {
		score_text.text = GameManager.self.score + "";
	}

	public void SetCoins() {
		coins_text.text = GameManager.self.coins + "";
	}

	public void SetItem() {
		Debug.Log("textures/Items/" + GameManager.self.item.ToString().ToLower() + ".png");
		item_button.GetComponent<Image>().sprite = Resources.Load<Sprite>("textures/Items/" + GameManager.self.item.ToString().ToLower());;
	}

}
