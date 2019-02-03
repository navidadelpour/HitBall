using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UiManager : MonoBehaviour {

	public static UiManager self;

	public Text score_text;
	public Text high_score_text;
	public Text coins_text;
	public Text combo_text;
	public Text gun_text;
	public Text next_goal_text;

	public Button[] item_buttons;
	public Button special_ability_button;
	public Button gift_button;

	public Image gun_image;

	public GameObject menu_panel;
	public GameObject game_panel;
	public GameObject game_over_panel;

	public GameObject texture;
	public GameObject fixed_background;


	void Awake() {
		self = this;

		score_text = GameObject.Find ("ScoreText").GetComponent<Text>();
		high_score_text = GameObject.Find("HighScoreText").GetComponent<Text>();
		coins_text = GameObject.Find ("CoinsText").GetComponent<Text>();
		combo_text = GameObject.Find ("ComboText").GetComponent<Text>();
		gun_text = GameObject.Find ("GunText").GetComponent<Text>();
		next_goal_text = GameObject.Find ("NextGoalText").GetComponent<Text>();

		item_buttons = GameObject.Find ("ItemsPanel").transform.GetComponentsInChildren<Button>();
		special_ability_button = GameObject.Find ("SpecialAbilityButton").GetComponent<Button>();
		gift_button = GameObject.Find ("GiftButton").GetComponent<Button>();

		gun_image = GameObject.Find ("GunButton").GetComponent<Image>();

		texture = GameObject.Find("Texture");
        fixed_background = GameObject.Find("FixedBackground");
	}

	void Start () {
		BringPanelsToCenter(new GameObject[]{
			menu_panel = GameObject.Find("MenuPanel"),
			game_panel = GameObject.Find("GamePanel"),
			game_over_panel = GameObject.Find("GameOverPanel"),
		});
		menu_panel.SetActive(true);

		SetScore ();
		SetHighScore();
		SetCoins ();
		SetCombo ();
		HandleItemSlots();
		DisableGift();
	}
	
	void Update () {
		
	}



	// ================================== ui changes ==================================

	public void SetHighScore() {
		high_score_text.text = GameManager.self.high_score + ""; 
	}
		
	public void SetScore() {
		score_text.text = GameManager.self.score + "";
	}

	public void SetCoins() {
		coins_text.text = GameManager.self.coins + "";
	}

	public void SetCombo() {
		combo_text.text = "x" + GameManager.self.combo; 
	}

	public void SetNextGoal(int value) {
		next_goal_text.text = "NEXT GOAL: " + value;
	}

	public void SetItem(int i, Item item) {
		item_buttons[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("textures/Items/" + item.ToString().ToLower());
	}

	public void SetGun() {
		gun_image.sprite = Resources.Load<Sprite>("textures/Guns/" + GunController.self.active_gun.ToString().ToLower());
	}

	public void SetGunText(int current_ammo, int ammo) {
		gun_text.text = current_ammo + " / " + ammo;
	}

	public void SetSpecialAbility() {
		special_ability_button.gameObject.GetComponent<Image>().sprite =
			Resources.Load<Sprite>("textures/SpecialAbilities/" + SpecialAbilityManager.self.current_ability.ToString().ToLower());
	}

	public void EnableSpecialAbility() {
		special_ability_button.interactable = true;
	}

	public void DisableSpecialAbility() {
		special_ability_button.interactable = false;
	}

	public void EnableGift() {
		gift_button.interactable = true;
	}

	public void DisableGift() {
		gift_button.interactable = false;
	}

	public void SetSpecialAbility(string name) {
		SpecialAbility special_ability = (SpecialAbility) System.Enum.Parse(typeof(SpecialAbility), name.ToUpper());
		SpecialAbilityManager.self.current_ability = special_ability;
		SetSpecialAbility();
	}

	public void SetGun(string name) {
		Guns gun = (Guns) System.Enum.Parse(typeof(Guns), name.ToUpper());
		GunController.self.SetGun(gun);
		SetGunText(GunController.self.guns[gun].ammo, GunController.self.guns[gun].ammo);
		SetGun();
	}

	public void SetTheme() {
		string theme_name = ShopManager.self.actives["Themes"].name.Split(new String[] {"_"}, StringSplitOptions.None)[0];
		string night_mode_state = SettingManager.self.has_night_mode ? "dark" : "light";
		string name = theme_name + "_" + night_mode_state + "_";
		texture.GetComponent<Renderer>().material.SetTexture("_MainTex", Resources.Load<Texture>("Textures/Backgrounds/" + name + "texture"));
		fixed_background.GetComponent<Renderer>().material.SetTexture("_MainTex", Resources.Load<Texture>("Textures/Backgrounds/" + name + "fixed_background"));
	}

	public void SetColor(int index) {
		SpriteRenderer player_renderer = GameObject.Find("Player").GetComponent<SpriteRenderer>();
		player_renderer.color = PlayerPrefsManager.self.colors[index];
	}

	public void SetFace(string key, string name) {
		SpriteRenderer key_on_player = GameObject.Find("Player").transform.Find(key).GetComponent<SpriteRenderer>();
		if(name == null)
			key_on_player.sprite = null;
		else
			key_on_player.sprite = Resources.Load<Sprite>("Textures/Faces/" + key + "/" + name);
	}


	// ================================== utility functions ==================================

	public void GameOver() {
		Util.GoToPanel(game_panel, game_over_panel);
	}

	public void BringPanelsToCenter(GameObject[] panels) {
		foreach(GameObject panel in panels) {
			RectTransform rect_transform = panel.GetComponent<RectTransform>();
			rect_transform.offsetMax = new Vector2(0, 0);
			rect_transform.offsetMin = new Vector2(0, 0);
			panel.SetActive(false);
		}
	}


	public void HandleItemSlots() {
		bool[] bools = new bool[] {true, true, true}; 
		for(int i = 0; i < 3 - LevelManager.self.item_slots_unlocks; i++) {
			bools[i] = false;
		}

		for(int i = 0; i < 3; i++) {
			item_buttons[2 - i].gameObject.SetActive(bools[i]);
		}
	}



	// ================================== listeners ==================================

	public void OnPlayButtonClick() {
		Util.GoToPanel(menu_panel, game_panel);
		AudioManager.self.Play("button");
	}

	public void OnBackToMenuButtonClick() {
		Util.GoToPanel(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject, menu_panel);
		AudioManager.self.Play("button");
	}

	public void OnBackToShopButtonClick() {
		Util.GoToPanel(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject, ShopManager.self.shop_panel);
		AudioManager.self.Play("button");
	}

}
