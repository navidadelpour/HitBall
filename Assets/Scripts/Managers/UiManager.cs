using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

	public static UiManager self;

	public Text score_text;
	public Text high_score_text;
	public Text coins_text;
	public Text combo_text;
	public Text gun_text;

	public Button[] item_buttons;

	public Image gun_image;
	public Image special_ability_image;

	public GameObject menu_panel;
	public GameObject game_panel;
	public GameObject shop_panel;
	public GameObject guns_panel;
	public GameObject special_abilities_panel;
	public GameObject game_over_panel;

	void Awake() {
		self = this;

		score_text = GameObject.Find ("ScoreText").GetComponent<Text>();
		high_score_text = GameObject.Find("HighScoreText").GetComponent<Text>();
		coins_text = GameObject.Find ("CoinsText").GetComponent<Text>();
		combo_text = GameObject.Find ("ComboText").GetComponent<Text>();
		gun_text = GameObject.Find ("GunText").GetComponent<Text>();

		item_buttons = GameObject.Find ("ItemsPanel").transform.GetComponentsInChildren<Button>();

		gun_image = GameObject.Find ("GunButton").GetComponent<Image>();
		special_ability_image = GameObject.Find ("SpecialAbilityButton").GetComponent<Image>();

		BringPanelsToCenter(new GameObject[]{
			menu_panel = GameObject.Find("MenuPanel"),
			game_panel = GameObject.Find("GamePanel"),
			shop_panel = GameObject.Find("ShopPanel"),
			guns_panel = GameObject.Find("GunsPanel"),
			special_abilities_panel = GameObject.Find("SpecialAbilitiesPanel"),
			game_over_panel = GameObject.Find("GameOverPanel"),
		});
		menu_panel.SetActive(true);
	}

	void Start () {
		SetScore ();
		SetHighScore();
		SetCoins ();
		SetCombo ();
	}
	
	void Update () {
		
	}

	// ================================== ui changes ==================================

	public void SetHighScore() {
		high_score_text.text = "HIGH SCORE: " + GameManager.self.high_score; 
	}
		
	public void SetScore() {
		score_text.text = GameManager.self.score + "";
	}

	public void SetCoins() {
		coins_text.text = GameManager.self.coins + "";
	}

	public void SetCombo() {
		combo_text.text = "COMBO: " + GameManager.self.combo; 
	}

	public void SetItem(int i, Item item) {
		item_buttons[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("textures/Items/" + item.ToString().ToLower());
	}

	public void SetGun(Guns gun) {
		gun_image.sprite = Resources.Load<Sprite>("textures/Guns/" + gun.ToString().ToLower());
	}

	public void SetGunText(int current_ammo, int ammo) {
		gun_text.text = current_ammo + " / " + ammo;
	}

	public void SetSpecialAbility(SpecialAbility special_ability) {
		special_ability_image.sprite = Resources.Load<Sprite>("textures/SpecialAbilities/" + special_ability.ToString().ToLower());
	}

	public void EnableSpecialAbility() {
		special_ability_image.color = new Color32(255, 255, 255, 255);
	}

	public void DisableSpecialAbility() {
		special_ability_image.color = new Color32(255, 255, 255, 70);
	}

	// ================================== utility functions ==================================

	public void GameOver() {
		game_panel.SetActive(false);
		game_over_panel.SetActive(true);
	}

	public void BringPanelsToCenter(GameObject[] panels) {
		foreach(GameObject panel in panels) {
			RectTransform rect_transform = panel.GetComponent<RectTransform>();
			rect_transform.offsetMax = new Vector2(0, 0);
			rect_transform.offsetMin = new Vector2(0, 0);
			panel.SetActive(false);
		}
	}

	private void GoToPanel(GameObject from, GameObject to) {
		from.SetActive(false);
		to.SetActive(true);
	}

	// ================================== listeners ==================================

	public void OnPlayButtonClick() {
		GoToPanel(menu_panel, game_panel);
	}

	public void OnShopButtonClick() {
		GoToPanel(menu_panel, shop_panel);
	}

}
