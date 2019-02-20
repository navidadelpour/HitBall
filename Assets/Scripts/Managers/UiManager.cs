using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UiManager : MonoBehaviour {

	public static UiManager self;

	public Text score_text;
	public Text coins_text;
	public Text combo_text;
	public Text gun_text;
	public Text next_goal_text;
	public Text level_text;
	public Text gift_text;

	public Button[] item_buttons;
	public Button special_ability_button;
	public Button gift_button;

	public Image gun_image;
	public Image combo_slider;
	public Image special_ability_slider;
	public Image high_jump_slider;
	public Image low_jump_slider;

	public GameObject menu_panel;
	public GameObject game_panel;
	public GameObject game_over_panel;
	public GameObject level_panel;
	public GameObject player_overview_panel;
	public GameObject pause_panel;
	public GameObject transition_panel;
	public GameObject tutorial_panel;
	public GameObject in_game_tutorial_panel;
	public GameObject next_goal;

	public GameObject texture;
	public GameObject fixed_background;

	public SpriteRenderer obstacle_prefab;
	public SpriteRenderer obstacle_sweep_prefab;
	public SpriteRenderer ground_prefab;


	void Awake() {
		self = this;

		score_text = GameObject.Find ("ScoreText").GetComponent<Text>();
		coins_text = GameObject.Find ("CoinsText").GetComponent<Text>();
		combo_text = GameObject.Find ("ComboText").GetComponent<Text>();
		gun_text = GameObject.Find ("GunText").GetComponent<Text>();
		next_goal_text = GameObject.Find ("NextGoalText").GetComponent<Text>();
		level_text = GameObject.Find ("LevelText").GetComponent<Text>();
		gift_text = GameObject.Find ("GiftText").GetComponent<Text>();

		item_buttons = GameObject.Find ("ItemsPanel").transform.GetComponentsInChildren<Button>();
		special_ability_button = GameObject.Find ("SpecialAbilityButton").GetComponent<Button>();
		gift_button = GameObject.Find ("GiftButton").GetComponent<Button>();

		gun_image = GameObject.Find ("GunIcon").GetComponent<Image>();
		special_ability_slider = GameObject.Find ("SpecialAbilitySlider").GetComponent<Image>();
		combo_slider = GameObject.Find ("ComboSlider").GetComponent<Image>();
		high_jump_slider = GameObject.Find ("HighJumpSlider").GetComponent<Image>();
		low_jump_slider = GameObject.Find ("LowJumpSlider").GetComponent<Image>();

		texture = GameObject.Find("Texture");
        fixed_background = GameObject.Find("FixedBackground");
        player_overview_panel = GameObject.Find("PlayerOverviewPanel");
        transition_panel = GameObject.Find("TransitionPanel");
        next_goal = GameObject.Find("NextGoal");

        obstacle_prefab = Resources.Load<GameObject>("Prefabs/Obstacles/Obstacle").GetComponent<SpriteRenderer>();
        obstacle_sweep_prefab = Resources.Load<GameObject>("Prefabs/Obstacles/ObstacleSweep").GetComponent<SpriteRenderer>();
        ground_prefab = Resources.Load<GameObject>("Prefabs/Ground").GetComponent<SpriteRenderer>();
	}

	void Start () {
		BringPanelsToCenter(new GameObject[]{
			menu_panel = GameObject.Find("MenuPanel"),
			game_panel = GameObject.Find("GamePanel"),
			game_over_panel = GameObject.Find("GameOverPanel"),
			level_panel = GameObject.Find("LevelPanel"),
			pause_panel = GameObject.Find("PausePanel"),
			tutorial_panel = GameObject.Find("TutorialPanel"),
			in_game_tutorial_panel = GameObject.Find("InGameTutorialPanel"),
		});
		menu_panel.SetActive(true);
		transition_panel.GetComponent<Animator>().SetTrigger("Out");
		SetScore ();
		SetHighScore();
		SetCoins ();
		SetCombo ();
		HandleItemSlots();

		UiManager.self.HandleItemSlots();
		StartCoroutine(Unlock());
	}
	
	void Update () {
		
	}



	// ================================== ui changes ==================================

	public void SetHighScore() {
		score_text.text = GameManager.self.high_score + ""; 
	}
		
	public void SetScore() {
		score_text.text = GameManager.self.score + "";
	}

	public void SetCoins() {
		coins_text.text = GameManager.self.coins + "";
	}

	public void SetCombo() {
		combo_text.text = "x" + GameManager.self.combo; 
		combo_text.GetComponent<Animator>().SetTrigger("Wiggle");
	}

	public void SetComboSlider(float value) {
		combo_slider.fillAmount = value;
	}

	public void SetNextGoal(int value, bool b = false) {
		if(b) {
			next_goal.SetActive(false);
			return;
		}
		string goal = value + "";
		if(value <= 0) {
			next_goal_text.GetComponent<Animator>().SetTrigger("Wiggle");
			AudioManager.self.Play("goal_reached");
			goal = "REACHED!";
			SpawnManager.self.should_create_goal = true;
		}
		next_goal_text.text = goal;
	}

	public void SetLevel(int value) {
		level_text.text = "LEVEL " + value;
	}

	public void ShowInGameTutorialPanel(Items item) {
		Time.timeScale = 0;
		GameManager.self.paused = true;
		in_game_tutorial_panel.SetActive(true);

		Sprite sprite = Resources.Load<Sprite>("textures/Items/" + item.ToString().ToLower());
		Sprite background_sprite = Resources.Load<Sprite>("textures/ItemBackgrounds/" + SpawnManager.self.item_backgrounds[item.ToString().ToLower()]);;
		
		in_game_tutorial_panel.transform.Find("Image").GetComponent<Image>().sprite = background_sprite;
		in_game_tutorial_panel.transform.Find("Image").GetChild(0).GetComponent<Image>().sprite = sprite;
		in_game_tutorial_panel.transform.Find("Description").GetComponent<Text>().text = ItemManager.self.metas[item];
	}

	public void HideInGameTutorialPanel() {
		in_game_tutorial_panel.SetActive(false);
		GameManager.self.paused = false;
		Time.timeScale = 1;
	}

	public void SetItem(int i, Items item) {
		Sprite sprite = Resources.Load<Sprite>("textures/Items/" + item.ToString().ToLower());
		Sprite background_sprite;
		Color color = Color.white;
		if(item == Items.NOTHING) {
			background_sprite = null;
			color = Color.clear;
		} else {
			sprite = Resources.Load<Sprite>("textures/Items/" + item.ToString().ToLower());
			background_sprite = Resources.Load<Sprite>("textures/ItemBackgrounds/" + SpawnManager.self.item_backgrounds[item.ToString().ToLower()]);
			color = Color.white;
		}
		item_buttons[i].GetComponent<Image>().sprite = background_sprite;
		item_buttons[i].GetComponent<Image>().color = color;
		item_buttons[i].transform.Find("Image").GetComponent<Image>().sprite = sprite;
	}

	public void SetItemSlider(int i, float value) {
		item_buttons[0].transform.parent.GetChild(i).GetComponent<Image>().fillAmount = value;
	}

	public void SetJumpButtonSlidersValue(float high_time, float low_time) {
		high_jump_slider.fillAmount = high_time;
		low_jump_slider.fillAmount = low_time;
	}

	public void SetGunText(int current_ammo, int ammo) {
		gun_text.text = current_ammo + " / " + ammo;
		gun_text.GetComponent<Animator>().SetTrigger("Wiggle");
		gun_image.GetComponent<Animator>().SetTrigger("Pressed");
	}

	public void SetSpecialAbility() {
		special_ability_button.gameObject.GetComponent<Image>().sprite =
			Resources.Load<Sprite>("textures/SpecialAbilities/" + SpecialAbilitiesManager.self.current_ability.ToString().ToLower());
	}

	public void SetSpecialAbilitySlider(float value, bool give) {
		special_ability_slider.color = give ? Color.green : Color.red;
		special_ability_slider.fillAmount = value;
	}

	public void EnableSpecialAbility() {
		special_ability_button.interactable = true;
		special_ability_button.GetComponent<Image>().color = new Color(1, 1, 1, 1);
	}

	public void DisableSpecialAbility() {
		special_ability_button.interactable = false;
		special_ability_button.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
	}

	public void EnableGift() {
		gift_button.interactable = true;
		gift_button.GetComponent<Animator>().SetTrigger("In");
	}

	public void DisableGift(int gift_coin) {
		gift_button.interactable = false;
		gift_text.text = "+" + gift_coin;
		gift_button.GetComponent<Animator>().SetTrigger("Out");
		gift_text.GetComponent<Animator>().SetTrigger("In");
		AudioManager.self.Play("coin_cool");
	}

	public void SetSpecialAbility(string name) {
		SpecialAbilities special_ability = (SpecialAbilities) System.Enum.Parse(typeof(SpecialAbilities), name.ToUpper());
		SpecialAbilitiesManager.self.current_ability = special_ability;
		SetSpecialAbility();
	}

	public void SetGun(string name) {
		Dictionary<Enum, Vector3[]> transforms = new Dictionary<Enum, Vector3[]>() {
			{Guns.PISTOL, new Vector3[] {new Vector3(3.5f, -.5f, 0), new Vector3(0, 0, -20f), new Vector3(1f, 1f, 1f)}},
			{Guns.RIFLE, new Vector3[] {new Vector3(3.5f, 0, 0), new Vector3(0, 0, -45f), new Vector3(1.6f, 1.6f, 1.6f)}},
			{Guns.SHOTGUN, new Vector3[] {new Vector3(3.5f, 0, 0), new Vector3(0, 0, -45f), new Vector3(1.6f, 1.6f, 1.6f)}},
		};

		Guns gun = (Guns) System.Enum.Parse(typeof(Guns), name.ToUpper());
		GunController.self.SetGun(gun);

		Sprite sprite = Resources.Load<Sprite>("textures/Guns/" + gun.ToString().ToLower());
		gun_image.sprite = sprite;
		PlayerMovement.self.transform.Find("GunAnimator").GetChild(0).GetComponent<SpriteRenderer>().sprite = sprite;

		SetGunText(GunController.self.guns[gun].ammo, GunController.self.guns[gun].ammo);

		Transform gun_transform = PlayerMovement.self.transform.Find("GunAnimator").GetChild(0).transform;
		gun_transform.localPosition = transforms[gun][0];
		gun_transform.localEulerAngles = transforms[gun][1];
		gun_transform.localScale = transforms[gun][2];

	}

	public void SetTheme() {
		string theme_name = ShopManager.self.actives["Themes"].name.Split(new String[] {"_"}, StringSplitOptions.None)[0];
		string night_mode_state = SettingManager.self.has_night_mode ? "dark" : "light";
		string name = theme_name + "_" + night_mode_state + "_";

		texture.GetComponent<Renderer>().material.SetTexture("_MainTex", Resources.Load<Texture>("Textures/Backgrounds/" + name + "texture"));
		fixed_background.GetComponent<Renderer>().material.SetTexture("_MainTex", Resources.Load<Texture>("Textures/Backgrounds/" + name + "fixed_background"));

		Sprite obstacle_sprite = Resources.Load<Sprite>("Textures/Objects/Obstacles/" + theme_name + "_" + "obstacle");
		Sprite ground_sprite = Resources.Load<Sprite>("Textures/Objects/Grounds/" + theme_name + "_" + "ground");
		obstacle_prefab.sprite = obstacle_sprite;
		obstacle_sweep_prefab.sprite = obstacle_sprite;
		ground_prefab.sprite = ground_sprite;
		Transform Grounds_on_scene = GameObject.Find("Grounds").transform;
		for(int i = 0; i < Grounds_on_scene.childCount; i++) {
			Grounds_on_scene.GetChild(i).GetComponent<SpriteRenderer>().sprite = ground_sprite;
		}
	}

	public void SetColor(int index) {
		SpriteRenderer player_renderer = GameObject.Find("Player").GetComponent<SpriteRenderer>();
		Sprite sprite = Resources.Load<Sprite>("Textures/Faces/Backgrounds/background_" + index);
		player_renderer.sprite = sprite;
		player_overview_panel.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
	}

	public void SetFace(string key, string name) {
		SpriteRenderer key_on_player = GameObject.Find("Player").transform.Find(key).GetComponent<SpriteRenderer>();
		Sprite sprite = name == null ? null : Resources.Load<Sprite>("Textures/Faces/" + key + "/" + name);
		key_on_player.sprite = sprite;
		player_overview_panel.transform.GetChild(0).Find(key).GetComponent<Image>().sprite = sprite;
		player_overview_panel.transform.GetChild(0).Find(key).GetComponent<Image>().color = name == null ? Color.clear : Color.white;
		player_overview_panel.transform.GetChild(0).Find(key).GetComponent<Animator>().SetTrigger("Wiggle");
	}

	IEnumerator Unlock() {
		yield return new WaitForSeconds(1f);
		string[] indexes = PlayerPrefs.GetString("indexes").Split(new String[] {"_"}, StringSplitOptions.None);

		if(!indexes[0].Equals("")) {
			GameManager.self.on_player_views = false;
			level_panel.SetActive(true);
			Image image = level_panel.transform.GetChild(0).Find("Image").GetComponent<Image>();
			Text name_text = level_panel.transform.GetChild(0).Find("Name").GetComponent<Text>();

			for(int j = 0; j < indexes.Length - 1; j++) {
				AudioManager.self.Play("unlock");
				level_panel.transform.GetChild(0).GetComponent<Animator>().SetTrigger("In");
				int i = int.Parse(indexes[j]);
				Enum item = LevelManager.self.levels[i];

				string path = "Textures";
				string header = item.ToString().Replace('_', ' ');
				if(item.ToString() == Items.NOTHING.ToString())
					header = "ITEM SLOT";

				name_text.text = header;
				image.sprite = Resources.Load<Sprite>(path + "/" + item.GetType().ToString() + "/" + item.ToString().ToLower());
				yield return new WaitForSeconds(2f);
			}
			level_panel.SetActive(false);
		}
	}

	// ================================== utility functions ==================================

	public void GameOver() {
		Util.GoToPanel(game_panel, game_over_panel);
		game_over_panel.transform.Find("NewHighScoreLabel").gameObject.SetActive(GameManager.self.score == GameManager.self.high_score);
		game_over_panel.transform.Find("GameOverScoreText").GetComponent<Text>().text = "AFTER: " + GameManager.self.score;
		game_over_panel.transform.Find("GameOverBestText").GetComponent<Text>().text = "BEST: " + GameManager.self.high_score;

		game_over_panel.GetComponent<Animator>().SetTrigger("In");
		game_over_panel.transform.GetChild(0).GetComponent<Animator>().SetTrigger("In");

		AudioManager.self.Play("gameover");
	}

	public void BringPanelsToCenter(GameObject[] panels, GameObject parent_panel = null) {
		foreach(GameObject panel in panels) {
			RectTransform rect_transform = panel.GetComponent<RectTransform>();
			rect_transform.offsetMax = new Vector2(0, 0);
			rect_transform.offsetMin = new Vector2(0, 0);
			if(parent_panel != null && panel != parent_panel)
				panel.transform.SetSiblingIndex(parent_panel.transform.GetSiblingIndex() + 1);
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
		GameObject.Find("WorldSpaceCanvas").SetActive(false);
	}

	public void OnBackToMenuButtonClick() {
		Util.GoToPanel(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject, menu_panel);
		ShopManager.self.player_overview_panel.SetActive(false);
		GameManager.self.on_player_views = true;
	}

	public void OnBackToShopButtonClick() {
		StartCoroutine(ShopManager.self.ShopPanelOut());
		GameManager.self.on_player_views = false;
	}

}
