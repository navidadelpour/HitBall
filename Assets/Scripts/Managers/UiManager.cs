﻿using System.Collections;
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

	public Button[] item_buttons;
	public Button special_ability_button;

	public Image gun_image;

	public GameObject menu_panel;
	public GameObject game_panel;
	public GameObject shop_panel;
	public GameObject shop_general_panel;
	public GameObject shop_items_panel;
	public GameObject shop_guns_panel;
	public GameObject shop_special_abilities_panel;
	public GameObject game_over_panel;

	public GameObject shop_item;
	private Vector3 shop_item_size;
	private float shop_margin = 30f;

	void Awake() {
		self = this;

		score_text = GameObject.Find ("ScoreText").GetComponent<Text>();
		high_score_text = GameObject.Find("HighScoreText").GetComponent<Text>();
		coins_text = GameObject.Find ("CoinsText").GetComponent<Text>();
		combo_text = GameObject.Find ("ComboText").GetComponent<Text>();
		gun_text = GameObject.Find ("GunText").GetComponent<Text>();

		item_buttons = GameObject.Find ("ItemsPanel").transform.GetComponentsInChildren<Button>();
		special_ability_button = GameObject.Find ("SpecialAbilityButton").GetComponent<Button>();

		gun_image = GameObject.Find ("GunButton").GetComponent<Image>();

		shop_item = Resources.Load<GameObject>("Prefabs/Ui/ShopItem");
		shop_general_panel = GameObject.Find("ShopGeneralPanel");

		shop_panel = GameObject.Find("ShopPanel");
		shop_guns_panel = Instantiate(shop_general_panel, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
		shop_guns_panel.name = "GunsPanel";
		shop_items_panel = Instantiate(shop_general_panel, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
		shop_items_panel.name = "ItemsPanel";
		shop_special_abilities_panel = Instantiate(shop_general_panel, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
		shop_special_abilities_panel.name = "SpecialAbilitiesPanel";

		Destroy(shop_general_panel);

		BringPanelsToCenter(new GameObject[]{
			menu_panel = GameObject.Find("MenuPanel"),
			game_panel = GameObject.Find("GamePanel"),
			shop_panel,
			shop_items_panel,
			shop_guns_panel,
			shop_special_abilities_panel,
			game_over_panel = GameObject.Find("GameOverPanel"),
		});
		menu_panel.SetActive(true);

		shop_item_size = Vector3.right * shop_item.GetComponent<RectTransform>().rect.width + Vector3.up * shop_item.GetComponent<RectTransform>().rect.height;
	}

	void Start () {
		SetScore ();
		SetHighScore();
		SetCoins ();
		SetCombo ();
        SetSpecialAbility();
		SetupShopGunsPanel();
		SetupShopSpecialAbilityPanel();
		SetupShopItemsPanel();
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

	// ================================== utility functions ==================================

	public void GameOver() {
		GoToPanel(game_panel, game_over_panel);
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

	void SetupShopGunsPanel() {
		Transform content = Util.FindDeepChild(shop_guns_panel.transform, "Content").transform;
		Guns[] enum_array = (Guns[]) Enum.GetValues(typeof(Guns));
		content.GetComponent<RectTransform>().sizeDelta = Vector2.up * (enum_array.Length * (shop_margin + shop_item_size.y) + shop_margin);
		int i = 0;
		foreach (System.Enum enum_item in enum_array) {
			GameObject shop_item_created = Instantiate(
				shop_item,
				content.transform.position +
				Vector3.right * (shop_item_size.x / 2 + shop_margin) +
				Vector3.down * (shop_item_size.y / 2 + (shop_item_size.y + shop_margin) * i + shop_margin),
				Quaternion.identity,
				content.transform
			);
			shop_item_created.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Guns/" + enum_item.ToString().ToLower());
			shop_item_created.GetComponent<Button>().interactable = LevelManager.self.unlocks[enum_item];
			i++;
		}
	}

	void SetupShopSpecialAbilityPanel() {
		Transform content = Util.FindDeepChild(shop_special_abilities_panel.transform, "Content").transform;
		SpecialAbility[] enum_array = (SpecialAbility[]) Enum.GetValues(typeof(SpecialAbility));
		content.GetComponent<RectTransform>().sizeDelta = Vector2.up * (enum_array.Length * (shop_margin + shop_item_size.y) + shop_margin);
		int i = 0;
		foreach (System.Enum enum_item in enum_array) {
			GameObject shop_item_created = Instantiate(
				shop_item,
				content.transform.position +
				Vector3.right * (shop_item_size.x / 2 + shop_margin) +
				Vector3.down * (shop_item_size.y / 2 + (shop_item_size.y + shop_margin) * i + shop_margin),
				Quaternion.identity,
				content.transform
			);
			shop_item_created.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/SpecialAbilities/" + enum_item.ToString().ToLower());
			shop_item_created.GetComponent<Button>().interactable = LevelManager.self.unlocks[enum_item];
			i++;
		}
	}

	void SetupShopItemsPanel() {
		Transform content = Util.FindDeepChild(shop_items_panel.transform, "Content").transform;
		Item[] enum_array = (Item[]) Enum.GetValues(typeof(Item));
		content.GetComponent<RectTransform>().sizeDelta = Vector2.up * ((enum_array.Length - 1) * (shop_margin + shop_item_size.y) + shop_margin);
		int i = 0;
		foreach (System.Enum enum_item in enum_array) {
			if(enum_item.ToString() != Item.NOTHING.ToString()) {
				GameObject shop_item_created = Instantiate(
					shop_item,
					content.transform.position +
					Vector3.right * (shop_item_size.x / 2 + shop_margin) +
					Vector3.down * (shop_item_size.y / 2 + (shop_item_size.y + shop_margin) * i + shop_margin),
					Quaternion.identity,
					content.transform
				);
				shop_item_created.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Items/" + enum_item.ToString().ToLower());
				i++;
			}
		}
	}


	public void CheckForLevelUp() {
		bool[] bools = new bool[] {true, true, true}; 
		for(int i = 0; i < 3 - LevelManager.self.item_slots_unlocks; i++) {
			bools[i] = false;
		}

		for(int i = 0; i < 3; i++) {
			item_buttons[i].gameObject.SetActive(bools[i]);
		}
	}



	// ================================== listeners ==================================

	public void OnPlayButtonClick() {
		GoToPanel(menu_panel, game_panel);
	}

	public void OnShopButtonClick() {
		GoToPanel(menu_panel, shop_panel);
	}

	public void OnBackToMenuButtonClick() {
		GoToPanel(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject, menu_panel);
	}

	public void OnBackToShopButtonClick() {
		GoToPanel(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject, shop_panel);
	}

	public void OnGunsPanelButtonClick() {
		GoToPanel(shop_panel, shop_guns_panel);
	}

	public void OnItemsPanelButtonClick() {
		GoToPanel(shop_panel, shop_items_panel);
	}


	public void OnSpecialAbilitiesPanelButtonClick() {
		GoToPanel(shop_panel, shop_special_abilities_panel);
	}

}
