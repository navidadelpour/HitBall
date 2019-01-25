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
	public Sprite tick_sprite;
	public Sprite lock_sprite;

	public GameObject menu_panel;
	public GameObject game_panel;
	public GameObject shop_panel;
	public GameObject shop_general_panel;
	public GameObject shop_guns_panel;
	public GameObject shop_special_abilities_panel;
	public GameObject shop_faces_panel;
	public GameObject shop_themes_panel;
	public GameObject shop_colors_panel;
	public GameObject game_over_panel;

	public GameObject faces_header;
	public GameObject faces_panel;
	public GameObject shop_item;
	public GameObject shop_faces_item;
	public GameObject shop_themes_item;
	private Vector3 shop_item_size;
	private Vector3 shop_faces_item_size;
	private float shop_margin = 30f;
	private int colors_cost = 10;

	public Dictionary<string, GameObject> actives = new Dictionary<string, GameObject>();

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

		tick_sprite = Resources.Load<Sprite>("Textures/UI/tick");
		lock_sprite = Resources.Load<Sprite>("Textures/UI/lock");

		faces_header = Resources.Load<GameObject>("Prefabs/Ui/FacesHeader");
		faces_panel = Resources.Load<GameObject>("Prefabs/Ui/Panel");
		shop_item = Resources.Load<GameObject>("Prefabs/Ui/ShopItem");
		shop_faces_item = Resources.Load<GameObject>("Prefabs/Ui/ShopFacesItem");
		shop_themes_item = Resources.Load<GameObject>("Prefabs/Ui/ShopThemesItem");

		shop_general_panel = GameObject.Find("ShopGeneralPanel");

		shop_panel = GameObject.Find("ShopPanel");
		shop_guns_panel = Instantiate(shop_general_panel, GameObject.Find("Canvas").transform);
		shop_guns_panel.name = "GunsPanel";
		shop_special_abilities_panel = Instantiate(shop_general_panel, GameObject.Find("Canvas").transform);
		shop_special_abilities_panel.name = "SpecialAbilitiesPanel";
		shop_faces_panel = Instantiate(shop_general_panel, GameObject.Find("Canvas").transform);
		shop_faces_panel.name = "FacesPanel";
		shop_themes_panel = Instantiate(shop_general_panel, GameObject.Find("Canvas").transform);
		shop_themes_panel.name = "ThemesPanel";
		shop_colors_panel = Instantiate(shop_general_panel, GameObject.Find("Canvas").transform);
		shop_colors_panel.name = "ColorsPanel";

		Destroy(shop_general_panel);

		BringPanelsToCenter(new GameObject[]{
			menu_panel = GameObject.Find("MenuPanel"),
			game_panel = GameObject.Find("GamePanel"),
			shop_panel,
			shop_guns_panel,
			shop_special_abilities_panel,
			shop_faces_panel,
			shop_themes_panel,
			shop_colors_panel,
			game_over_panel = GameObject.Find("GameOverPanel"),
		});
		menu_panel.SetActive(true);

		shop_item_size = Vector3.right * shop_item.GetComponent<RectTransform>().rect.width + Vector3.up * shop_item.GetComponent<RectTransform>().rect.height;
		shop_faces_item_size = Vector3.right * shop_faces_item.GetComponent<RectTransform>().rect.width + Vector3.up * shop_faces_item.GetComponent<RectTransform>().rect.height;

		// foreach (string s in new string[] {"SpecialAbilities", "Guns", "Beards", "Hats", "Themes", "Colors"}) {
		// 	actives.Add(s, PlayerPrefs.GetInt(s) == PlayerPrefsManager.self.active ? GameObject.Find(s) : null);
		// }

		// PlayerPrefs.SetInt("DefaultTheme", 2);
		// PlayerPrefs.SetInt("RGBA(0, 0, 0, 0)", 2);
	}

	void Start () {
        SetSpecialAbility();
		SetupShopGunsPanel();
		SetupShopSpecialAbilityPanel();
		SetupShopFacesPanel();
		SetupShopThemesPanel();
		SetupShopColorsPanel();
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
		Guns[] enum_array = (Guns[]) Enum.GetValues(typeof(Guns));

		Transform content = Util.FindDeepChild(shop_guns_panel.transform, "Content").transform;
		content.GetComponent<RectTransform>().sizeDelta = Vector2.up * (enum_array.Length * (shop_margin + shop_item_size.y) + shop_margin);

		for(int i = 0; i < enum_array.Length; i++) {
			GameObject shop_item_created = Instantiate(
				shop_item,
				content.transform.position +
				Vector3.right * (shop_item_size.x / 2 + shop_margin) +
				Vector3.down * (shop_item_size.y / 2 + (shop_item_size.y + shop_margin) * i + shop_margin),
				Quaternion.identity,
				content.transform
			);
			
			Sprite sprite_to_give = null;
			bool interactable = true;
			switch(PlayerPrefs.GetInt(enum_array[i].ToString())) {
				case 0:
					sprite_to_give = null;
					break;
				case 1:
					sprite_to_give = lock_sprite;
					interactable = false;
					break;
				case 2:
					sprite_to_give = tick_sprite;
					actives["SpecialAbilities"] = shop_item_created;
					break;
			}

			shop_item_created.name = enum_array[i].ToString();
			shop_item_created.GetComponent<Button>().interactable = interactable;
			shop_item_created.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Guns/" + enum_array[i].ToString().ToLower());
			shop_item_created.transform.Find("Status").GetComponent<Image>().sprite = sprite_to_give;
		}
	}

	void SetupShopSpecialAbilityPanel() {
		SpecialAbility[] enum_array = (SpecialAbility[]) Enum.GetValues(typeof(SpecialAbility));

		Transform content = Util.FindDeepChild(shop_special_abilities_panel.transform, "Content").transform;
		content.GetComponent<RectTransform>().sizeDelta = Vector2.up * (enum_array.Length * (shop_margin + shop_item_size.y) + shop_margin);

		for(int i = 0; i < enum_array.Length; i++) {
			GameObject shop_item_created = Instantiate(
				shop_item,
				content.transform.position +
				Vector3.right * (shop_item_size.x / 2 + shop_margin) +
				Vector3.down * (shop_item_size.y / 2 + (shop_item_size.y + shop_margin) * i + shop_margin),
				Quaternion.identity,
				content.transform
			);
			
			Sprite sprite_to_give = null;
			bool interactable = true;
			switch(PlayerPrefs.GetInt(enum_array[i].ToString())) {
				case 0:
					sprite_to_give = null;
					break;
				case 1:
					sprite_to_give = lock_sprite;
					interactable = false;
					break;
				case 2:
					sprite_to_give = tick_sprite;
					actives["SpecialAbilities"] = shop_item_created;
					break;
			}

			shop_item_created.name = enum_array[i].ToString();
			shop_item_created.GetComponent<Button>().interactable = interactable;
			shop_item_created.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/SpecialAbilities/" + enum_array[i].ToString().ToLower());
			shop_item_created.transform.Find("Status").GetComponent<Image>().sprite = sprite_to_give;
		}
	}

	public void SetupShopThemesPanel() {
		Sprite[] sprite_array = Resources.LoadAll<Sprite>("Textures/ThemesIcons");

		Transform content = Util.FindDeepChild(shop_themes_panel.transform, "Content").transform;
		content.GetComponent<RectTransform>().sizeDelta = Vector2.up * (sprite_array.Length * (shop_margin + shop_item_size.y) + shop_margin);

		for(int i = 0; i < sprite_array.Length; i++) {
			GameObject shop_item_created = Instantiate(
				shop_themes_item,
				content.transform.position +
				Vector3.right * (shop_item_size.x / 2 + shop_margin) +
				Vector3.down * (shop_item_size.y / 2 + (shop_item_size.y + shop_margin) * i + shop_margin),
				Quaternion.identity,
				content.transform
			);

			string[] x = sprite_array[i].name.Split(new String[] {"_"}, StringSplitOptions.None);
			string name = x[0];
			string cost = x[1];
			Sprite sprite_to_give = null;
			switch(PlayerPrefs.GetInt(name)) {
				case 0:
					sprite_to_give = null;
					cost = "";
					break;
				case 1:
					sprite_to_give = lock_sprite;
					break;
				case 2:
					sprite_to_give = tick_sprite;
					cost = "";
					actives["Themes"] = shop_item_created;
					break;
			}

			shop_item_created.name = sprite_array[i].name;
			shop_item_created.transform.Find("Image").gameObject.GetComponent<Image>().sprite = sprite_array[i];
			shop_item_created.transform.Find("Status").gameObject.GetComponent<Image>().sprite = sprite_to_give;
			shop_item_created.transform.Find("Cost").gameObject.GetComponent<Text>().text = cost;
		}

	}

	void SetupShopColorsPanel() {
		Color32[] color_array = new Color32[] {Color.clear, Color.red, Color.blue, Color.green, Color.yellow, Color.magenta};

		Transform content = Util.FindDeepChild(shop_colors_panel.transform, "Content").transform;
		content.GetComponent<RectTransform>().sizeDelta = Vector2.up * ((color_array.Length - 1) * (shop_margin + shop_item_size.y) + shop_margin);

		for(int i = 0; i < color_array.Length / 3 + 1; i++) {
			for(int j = 0; j < 3; j++) {
				int index = i * 3 + j;
				if(index > color_array.Length - 1)
					break;

				GameObject shop_item_created = Instantiate(
					shop_faces_item,
					content.transform.position +
					Vector3.right * ((shop_faces_item_size.x / 2) + (shop_faces_item_size.x + shop_margin) * j + shop_margin) +
					Vector3.down * (shop_faces_item_size.y / 2 + (shop_faces_item_size.y + shop_margin) * i + shop_margin),
					Quaternion.identity,
					content.transform
				);

				string name = color_array[index].ToString();
				string cost = colors_cost + "";
				Sprite sprite_to_give = null;
				switch(PlayerPrefs.GetInt(name)) {
					case 0:
						sprite_to_give = null;
						cost = "";
						break;
					case 1:
						sprite_to_give = lock_sprite;
						break;
					case 2:
						sprite_to_give = tick_sprite;
						cost = "";
						actives["Colors"] = shop_item_created;
						break;
				}
				shop_item_created.name = color_array[index].ToString() + "_" + (cost == "" ? "0" : cost);
				shop_item_created.transform.Find("Image").gameObject.GetComponent<Image>().color = color_array[index];
				shop_item_created.transform.Find("Status").gameObject.GetComponent<Image>().sprite = sprite_to_give;
				shop_item_created.transform.Find("Cost").gameObject.GetComponent<Text>().text = cost;
				// SetPlayerColor(color_array[index]);
			}
		}
	}


	void SetupShopFacesPanel() {
		Transform content = Util.FindDeepChild(shop_faces_panel.transform, "Content").transform;
		float content_size = 0f;
		string[] postfixes = {"Beards", "Hats"};
		for(int k = 0; k < postfixes.Length; k++) {
			Sprite[] sprite_array = Resources.LoadAll<Sprite>("Textures/Faces/" + postfixes[k]);

			Vector2 faces_header_size = Vector3.right * faces_header.GetComponent<RectTransform>().rect.width + Vector3.up * faces_header.GetComponent<RectTransform>().rect.height;

			GameObject panel = Instantiate(
				faces_panel,
				content.transform.position,
				Quaternion.identity,
				content.transform
			);
			panel.GetComponent<RectTransform>().sizeDelta = Vector2.up * ((sprite_array.Length / 3 + 1) * (shop_margin + shop_faces_item_size.y) + shop_margin + faces_header_size.y);
			panel.name = postfixes[k];

			GameObject header = Instantiate(
				faces_header,
				panel.transform.position +
				Vector3.right * faces_header_size.x / 2 +
				Vector3.down * (faces_header_size.y / 2 + content_size),
				Quaternion.identity,
				panel.transform
			);
			header.GetComponent<Text>().text = postfixes[k];

			for(int i = 0; i < sprite_array.Length / 3 + 1; i++) {
				for(int j = 0; j < 3; j++) {
					int index = i * 3 + j;
					if(index > sprite_array.Length - 1)
						break;

					GameObject shop_item_created = Instantiate(
						shop_faces_item,
						panel.transform.position +
						Vector3.right * ((shop_faces_item_size.x / 2) + (shop_faces_item_size.x + shop_margin) * j + shop_margin) +
						Vector3.down * (shop_faces_item_size.y / 2 + (shop_faces_item_size.y + shop_margin) * i + shop_margin + faces_header_size.y + content_size),
						Quaternion.identity,
						panel.transform
					);

					string[] x = sprite_array[index].name.Split(new String[] {"_"}, StringSplitOptions.None);
					string name = x[0];
					string cost = x[1];
					Sprite sprite_to_give = null;
					switch(PlayerPrefs.GetInt(name)) {
						case 0:
							sprite_to_give = null;
							cost = "";
							break;
						case 1:
							sprite_to_give = lock_sprite;
							break;
						case 2:
							sprite_to_give = tick_sprite;
							cost = "";
							actives[postfixes[k]] = shop_item_created;
							break;
					}
					
					shop_item_created.name = sprite_array[index].name;
					shop_item_created.transform.Find("Image").gameObject.GetComponent<Image>().sprite = sprite_array[index];
					shop_item_created.transform.Find("Status").gameObject.GetComponent<Image>().sprite = sprite_to_give;
					shop_item_created.transform.Find("Cost").gameObject.GetComponent<Text>().text = cost;
				}
			}
			content_size += panel.GetComponent<RectTransform>().sizeDelta.y;
		}
		content.GetComponent<RectTransform>().sizeDelta = Vector3.up * content_size;
	}

	public void EnableShopItem(GameObject shop_item_instance) {
		GameObject top_parent_panel = shop_item_instance.transform.parent.parent.parent.parent.gameObject;
		switch(top_parent_panel.name) {
			case "SpecialAbilitiesPanel":
				if(shop_item_instance.name == actives["SpecialAbilities"].name)
					return;
				shop_item_instance.transform.Find("Status").GetComponent<Image>().sprite = tick_sprite;
				actives["SpecialAbilities"].transform.Find("Status").GetComponent<Image>().sprite = null;
				actives["SpecialAbilities"] = shop_item_instance;
				SpecialAbilityManager.self.current_ability = (SpecialAbility) System.Enum.Parse(typeof(SpecialAbility), shop_item_instance.name.ToUpper());
				SetSpecialAbility();
				break;
			case "GunsPanel":
				if(shop_item_instance.name == actives["Guns"].name)
					return;
				shop_item_instance.transform.Find("Status").GetComponent<Image>().sprite = tick_sprite;
				actives["Guns"].transform.Find("Status").GetComponent<Image>().sprite = null;
				actives["Guns"] = shop_item_instance;
				Guns gun = (Guns) System.Enum.Parse(typeof(Guns), shop_item_instance.name.ToUpper());
				GunController.self.SetGun(gun);
				SetGun();
				SetGunText(GunController.self.guns[gun].ammo, GunController.self.guns[gun].ammo);
				break;
			case "ThemesPanel":
				if(actives["Themes"] != null && shop_item_instance == actives["Themes"])
					return;
				else {
					string[] x = shop_item_instance.name.Split(new String[] {"_"}, StringSplitOptions.None);
					string name = x[0];
					int cost = int.Parse(x[1]);
					bool unlock = shop_item_instance.transform.Find("Status").GetComponent<Image>().sprite != lock_sprite;
					bool active = shop_item_instance.transform.Find("Status").GetComponent<Image>().sprite == tick_sprite;

					if(!unlock && GameManager.self.coins >= cost) {
						GameManager.self.coins -= cost;
						unlock = true;
					}
					if(unlock) {
						actives["Themes"].transform.Find("Status").GetComponent<Image>().sprite = null;
						actives["Themes"].transform.Find("Cost").GetComponent<Text>().text = "";

						actives["Themes"] = shop_item_instance;

						actives["Themes"].transform.Find("Status").GetComponent<Image>().sprite = tick_sprite;
						actives["Themes"].transform.Find("Cost").GetComponent<Text>().text = "";
						// SetTheme(name);
					}
				}
				break;
				case "ColorsPanel":
					if(actives["Colors"] != null && shop_item_instance == actives["Colors"])
						return;
					else {
						string[] x = shop_item_instance.name.Split(new String[] {"_"}, StringSplitOptions.None);
						string name = x[0];
						int cost = int.Parse(x[1]);	
						bool unlock = shop_item_instance.transform.Find("Status").GetComponent<Image>().sprite != lock_sprite;
						bool active = shop_item_instance.transform.Find("Status").GetComponent<Image>().sprite == tick_sprite;

						if(!unlock && GameManager.self.coins >= cost) {
							GameManager.self.coins -= cost;
							unlock = true;
						}
						if(unlock) {
							actives["Colors"].transform.Find("Status").GetComponent<Image>().sprite = null;
							actives["Colors"].transform.Find("Cost").GetComponent<Text>().text = "";

							actives["Colors"] = shop_item_instance;

							actives["Colors"].transform.Find("Status").GetComponent<Image>().sprite = tick_sprite;
							actives["Colors"].transform.Find("Cost").GetComponent<Text>().text = "";
							// SetTheme(name);
						}
					}
					break;
			default:
				if(top_parent_panel.transform.parent.name == "FacesPanel") {
					string key = shop_item_instance.transform.parent.name;
					string[] x = shop_item_instance.name.Split(new String[] {"_"}, StringSplitOptions.None);
					string name = x[0];
					int cost = int.Parse(x[1]);
					bool unlock = shop_item_instance.transform.Find("Status").GetComponent<Image>().sprite != lock_sprite;
					bool active = shop_item_instance.transform.Find("Status").GetComponent<Image>().sprite == tick_sprite;
					SpriteRenderer key_on_player = GameObject.Find("Player").transform.Find(key).gameObject.GetComponent<SpriteRenderer>();
					if(actives[key] != null && shop_item_instance == actives[key]) {
						key_on_player.sprite = null;
						actives[key].transform.Find("Status").GetComponent<Image>().sprite = null;
						actives[key].transform.Find("Cost").GetComponent<Text>().text = "";
						actives[key] = null;
					} else {
						if(!unlock && GameManager.self.coins >= cost) {
							GameManager.self.coins -= cost;
							unlock = true;
						}
						if(unlock) {
							if(actives[key] != null) {
								actives[key].transform.Find("Status").GetComponent<Image>().sprite = null;
								actives[key].transform.Find("Cost").GetComponent<Text>().text = "";
							}
							actives[key] = shop_item_instance;

							actives[key].transform.Find("Status").GetComponent<Image>().sprite = tick_sprite;
							actives[key].transform.Find("Cost").GetComponent<Text>().text = "";

							key_on_player.sprite = shop_item_instance.transform.Find("Image").GetComponent<Image>().sprite;
						}
					}
				}
				break;
		}
	}

	public void CheckForLevelUp() {
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

	public void OnSpecialAbilitiesPanelButtonClick() {
		GoToPanel(shop_panel, shop_special_abilities_panel);
	}
	
	public void OnFacesPanelButtonClick() {
		GoToPanel(shop_panel, shop_faces_panel);
	}

	public void OnThemesPanelButtonClick() {
		GoToPanel(shop_panel, shop_themes_panel);
	}

	public void OnColorsPanelButtonClick() {
		GoToPanel(shop_panel, shop_colors_panel);
	}

}
