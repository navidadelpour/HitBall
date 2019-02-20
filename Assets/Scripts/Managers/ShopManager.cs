using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour {
    public static ShopManager self;
	private Sprite tick_sprite;
	private Sprite lock_sprite;
	private Texture colors_image;

	public GameObject shop_panel;
	private GameObject shop_general_panel;
	private GameObject shop_guns_panel;
	private GameObject shop_special_abilities_panel;
	private GameObject shop_faces_panel;
	private GameObject shop_themes_panel;
	private GameObject shop_colors_panel;
	public GameObject player_overview_panel;
	public GameObject always_panel;

	private GameObject faces_header;
	private GameObject faces_panel;
	private GameObject shop_item;
	private GameObject shop_faces_item;
	private GameObject shop_themes_item;
	private Vector3 shop_item_size;
	private Vector3 shop_faces_item_size;
	private float shop_margin = 0f;
	private int colors_cost = 100;

	public Dictionary<string, GameObject> actives = new Dictionary<string, GameObject>();
	private Dictionary<System.Enum, string[]> metas = new Dictionary<Enum, string[]>();
	float width_scale;
	float height_scale;

    void Awake() {
        self = this;
		width_scale = (float) Screen.width / 854;
		height_scale = (float) Screen.height / 480;

		AddMeta();

		tick_sprite = Resources.Load<Sprite>("Textures/UI/Shop/tick");
		lock_sprite = Resources.Load<Sprite>("Textures/UI/Shop/lock");
		colors_image = Resources.Load<Texture>("Textures/UI/Shop/colors_image");

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

        shop_item_size = Vector3.right * shop_item.GetComponent<RectTransform>().rect.width + Vector3.up * shop_item.GetComponent<RectTransform>().rect.height;
		shop_faces_item_size = Vector3.right * shop_faces_item.GetComponent<RectTransform>().rect.width + Vector3.up * shop_faces_item.GetComponent<RectTransform>().rect.height;

    }

    void Start() {
        UiManager.self.BringPanelsToCenter(new GameObject[]{
			always_panel = GameObject.Find("Always"),
			player_overview_panel = GameObject.Find("PlayerOverviewPanel"),
			shop_panel,
			shop_guns_panel,
			shop_special_abilities_panel,
			shop_faces_panel,
			shop_themes_panel,
			shop_colors_panel,
		}, shop_panel);
		player_overview_panel.SetActive(false);
		always_panel.SetActive(true);
		SetupShopGunsPanel();
		SetupShopSpecialAbilityPanel();
		SetupShopFacesPanel();
		SetupShopThemesPanel();
		SetupShopColorsPanel();
		// player_overview_panel.transform.SetAsLastSibling();
    }

    void Update() {

    }

    void SetupShopGunsPanel() {
		Guns[] enum_array = (Guns[]) Enum.GetValues(typeof(Guns));

		Transform content = Util.FindDeepChild(shop_guns_panel.transform, "Content").transform;
		content.GetComponent<RectTransform>().sizeDelta = Vector2.up * (enum_array.Length * (shop_margin + shop_item_size.y) + shop_margin);

		for(int i = 0; i < enum_array.Length; i++) {
			GameObject shop_item_created = Instantiate(
				shop_item,
				content.transform.position +
				// Vector3.right * (shop_item_size.x / 2) * width_scale +
				Vector3.down * (shop_item_size.y / 2 + (shop_item_size.y + shop_margin) * i + shop_margin) * height_scale,
				Quaternion.identity,
				content.transform
			);
			
			switch(PlayerPrefs.GetInt(enum_array[i].ToString())) {
				case 0:

					break;
				case 1:
					Transform lock_panel = shop_item_created.transform.Find("LockPanel");
					lock_panel.gameObject.SetActive(true);
					lock_panel.Find("Cost").gameObject.GetComponent<Text>().text += 2 + Array.FindIndex(LevelManager.self.levels, x => x.ToString() == enum_array[i].ToString());
					shop_item_created.GetComponent<Button>().interactable = false;
					break;
				case 2:
					shop_item_created.transform.Find("Tick").gameObject.SetActive(true);
					actives["Guns"] = shop_item_created;
					UiManager.self.SetGun(enum_array[i].ToString());
					break;
			}

			shop_item_created.name = enum_array[i].ToString();
			shop_item_created.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Guns/" + enum_array[i].ToString().ToLower());
			shop_item_created.transform.Find("Header").GetComponent<Text>().text = metas[enum_array[i]][0];
			shop_item_created.transform.Find("Description").GetComponent<Text>().text = metas[enum_array[i]][1];

		}
	}

	void SetupShopSpecialAbilityPanel() {
		SpecialAbilities[] enum_array = (SpecialAbilities[]) Enum.GetValues(typeof(SpecialAbilities));

		Transform content = Util.FindDeepChild(shop_special_abilities_panel.transform, "Content").transform;
		content.GetComponent<RectTransform>().sizeDelta = Vector2.up * (enum_array.Length * (shop_margin + shop_item_size.y) + shop_margin);

		for(int i = 0; i < enum_array.Length; i++) {
			GameObject shop_item_created = Instantiate(
				shop_item,
				content.transform.position +
				// Vector3.right * (shop_item_size.x / 2) * width_scale +
				Vector3.down * (shop_item_size.y / 2 + (shop_item_size.y + shop_margin) * i + shop_margin) * height_scale,
				Quaternion.identity,
				content.transform
			);
			
			switch(PlayerPrefs.GetInt(enum_array[i].ToString())) {
				case 0:

					break;
				case 1:
					Transform lock_panel = shop_item_created.transform.Find("LockPanel");
					lock_panel.gameObject.SetActive(true);
					lock_panel.Find("Cost").gameObject.GetComponent<Text>().text += 2 + Array.FindIndex(LevelManager.self.levels, x => x.ToString() == enum_array[i].ToString());
					shop_item_created.GetComponent<Button>().interactable = false;
					break;
				case 2:
					shop_item_created.transform.Find("Tick").gameObject.SetActive(true);
					actives["SpecialAbilities"] = shop_item_created;
					UiManager.self.SetSpecialAbility(enum_array[i].ToString());
					break;
			}

			shop_item_created.name = enum_array[i].ToString();
			shop_item_created.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/SpecialAbilities/" + enum_array[i].ToString().ToLower());
			shop_item_created.transform.Find("Header").GetComponent<Text>().text = metas[enum_array[i]][0];
			shop_item_created.transform.Find("Description").GetComponent<Text>().text = metas[enum_array[i]][1];
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
				// Vector3.right * (shop_item_size.x / 2) * width_scale +
				Vector3.down * (shop_item_size.y / 2 + (shop_item_size.y + shop_margin) * i + shop_margin) * height_scale,
				Quaternion.identity,
				content.transform
			);

			string[] x = sprite_array[i].name.Split(new String[] {"_"}, StringSplitOptions.None);
			string name = x[0];
			string cost = x[1];
			shop_item_created.name = sprite_array[i].name;
			switch(PlayerPrefs.GetInt(name)) {
				case 0:

					break;
				case 1:
					Transform lock_panel = shop_item_created.transform.Find("LockPanel");
					lock_panel.gameObject.SetActive(true);
					lock_panel.Find("Cost").gameObject.GetComponent<Text>().text = cost;
					break;
				case 2:
					shop_item_created.transform.Find("Tick").gameObject.SetActive(true);
					actives["Themes"] = shop_item_created;
					UiManager.self.SetTheme();
					break;
			}

			shop_item_created.transform.Find("Image").gameObject.GetComponent<Image>().sprite = sprite_array[i];
		}

	}

	void SetupShopColorsPanel() {
		Color32[] color_array = PlayerPrefsManager.self.colors;

		Transform content = Util.FindDeepChild(shop_colors_panel.transform, "Content").transform;
		content.GetComponent<RectTransform>().sizeDelta = Vector2.up * (Mathf.Ceil((float) color_array.Length / 3) * (shop_margin + shop_item_size.y) + shop_margin);

		for(int i = 0; i < color_array.Length / 3 + 1; i++) {
			for(int j = 0; j < 3; j++) {
				int index = i * 3 + j;
				if(index > color_array.Length - 1)
					break;

				GameObject shop_item_created = Instantiate(
					shop_faces_item,
					content.transform.position +
					Vector3.right * (-(shop_faces_item_size.x) + (shop_faces_item_size.x) * j) * width_scale +
					Vector3.down * (shop_faces_item_size.y / 2 + (shop_faces_item_size.y + shop_margin) * i + shop_margin) * height_scale,
					Quaternion.identity,
					content.transform
				);

				string name = index + ".Color";
				string cost = colors_cost + "";
				switch(PlayerPrefs.GetInt(name)) {
					case 0:

						break;
					case 1:
						Transform lock_panel = shop_item_created.transform.Find("LockPanel");
						lock_panel.gameObject.SetActive(true);
						lock_panel.Find("Cost").gameObject.GetComponent<Text>().text = cost;
						break;
					case 2:
						shop_item_created.transform.Find("Tick").gameObject.SetActive(true);
						actives["Colors"] = shop_item_created;
						UiManager.self.SetColor(index);
						break;
				}
				shop_item_created.name = name + "_" + (cost == "" ? "0" : cost);
				shop_item_created.transform.Find("Image").gameObject.GetComponent<RawImage>().texture = colors_image;
				shop_item_created.transform.Find("Image").gameObject.GetComponent<RawImage>().color = color_array[index];
			}
		}
	}


	void SetupShopFacesPanel() {
		Dictionary<string, Vector2> uv_rects = new Dictionary<string, Vector2>() {
			{"Mustaches", new Vector2(.12f, .2f)},
			{"Beards", new Vector2(0, 0)},
			{"Hats", new Vector2(0, 0)}
		};
		Transform content = Util.FindDeepChild(shop_faces_panel.transform, "Content").transform;
		float content_size = 0f;
		string[] postfixes = {"Mustaches", "Beards", "Hats"};
		for(int k = 0; k < postfixes.Length; k++) {
			Texture2D[] sprite_array = Resources.LoadAll<Texture2D>("Textures/Faces/" + postfixes[k]);

			Vector2 faces_header_size = Vector3.right * faces_header.GetComponent<RectTransform>().rect.width + Vector3.up * faces_header.GetComponent<RectTransform>().rect.height;

			GameObject panel = Instantiate(
				faces_panel,
				content.transform.position,
				Quaternion.identity,
				content.transform
			);
			
			panel.GetComponent<RectTransform>().sizeDelta = Vector2.up * (Mathf.Ceil((float) sprite_array.Length / 3) * (shop_margin + shop_faces_item_size.y) + shop_margin + faces_header_size.y);
			panel.name = postfixes[k];

			GameObject header = Instantiate(
				faces_header,
				panel.transform.position +
				// Vector3.right * (faces_header_size.x / 2) * width_scale +
				Vector3.down * (faces_header_size.y / 2 + content_size) * height_scale,
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
						Vector3.right * (-(shop_faces_item_size.x) + (shop_faces_item_size.x) * j) * width_scale +
						Vector3.down * (shop_faces_item_size.y / 2 + (shop_faces_item_size.y + shop_margin) * i + shop_margin + faces_header_size.y + content_size) * height_scale,
						Quaternion.identity,
						panel.transform
					);

					string[] x = sprite_array[index].name.Split(new String[] {"_"}, StringSplitOptions.None);
					string name = x[0];
					string cost = x[1];
					switch(PlayerPrefs.GetInt(name)) {
						case 0:
							
							break;
						case 1:
							Transform lock_panel = shop_item_created.transform.Find("LockPanel");
							lock_panel.gameObject.SetActive(true);
							lock_panel.Find("Cost").gameObject.GetComponent<Text>().text = cost;
							break;
						case 2:
							shop_item_created.transform.Find("Tick").gameObject.SetActive(true);
							actives[postfixes[k]] = shop_item_created;
							UiManager.self.SetFace(postfixes[k], sprite_array[index].name);
							break;
					}
					
					shop_item_created.name = sprite_array[index].name;
					shop_item_created.transform.Find("Image").gameObject.GetComponent<RawImage>().uvRect = new Rect(uv_rects[postfixes[k]], Vector2.one);
					shop_item_created.transform.Find("Image").gameObject.GetComponent<RawImage>().texture = sprite_array[index];
				}
			}
			content_size += panel.GetComponent<RectTransform>().sizeDelta.y;
		}
		content.GetComponent<RectTransform>().sizeDelta = Vector3.up * content_size;
	}

	public void EnableShopItem(GameObject shop_item_instance) {
		AudioManager.self.Play("button");
		GameObject top_parent_panel = shop_item_instance.transform.parent.parent.parent.parent.gameObject;
		switch(top_parent_panel.name) {
			case "SpecialAbilitiesPanel":
				if(shop_item_instance == actives["SpecialAbilities"])
					return;
				PlayerPrefs.SetInt(actives["SpecialAbilities"].name, 0);
				actives["SpecialAbilities"].transform.Find("Tick").gameObject.SetActive(false);
				actives["SpecialAbilities"] = shop_item_instance;
				actives["SpecialAbilities"].transform.Find("Tick").gameObject.SetActive(true);
				PlayerPrefs.SetInt(actives["SpecialAbilities"].name, 2);
				UiManager.self.SetSpecialAbility(actives["SpecialAbilities"].name);
				break;
			case "GunsPanel":
				if(shop_item_instance == actives["Guns"])
					return;
				PlayerPrefs.SetInt(actives["Guns"].name, 0);
				actives["Guns"].transform.Find("Tick").gameObject.SetActive(false);
				actives["Guns"] = shop_item_instance;
				actives["Guns"].transform.Find("Tick").gameObject.SetActive(true);
				PlayerPrefs.SetInt(actives["Guns"].name, 2);
				UiManager.self.SetGun(actives["Guns"].name);
				break;
			case "ThemesPanel":
				if(shop_item_instance == actives["Themes"])
					return;
				else {
					string[] x = shop_item_instance.name.Split(new String[] {"_"}, StringSplitOptions.None);
					string name = x[0];
					int cost = int.Parse(x[1]);
					bool unlock = PlayerPrefs.GetInt(name) != 1;

					if(!unlock && GameManager.self.coins >= cost) {
						GameManager.self.coins -= cost;
						UiManager.self.SetCoins();
                        unlock = true;
						shop_item_instance.transform.Find("LockPanel").gameObject.SetActive(false);
						AudioManager.self.Play("buy");
					}
                    if(unlock) {
						PlayerPrefs.SetInt(actives["Themes"].name.Split(new String[] {"_"}, StringSplitOptions.None)[0], 0);
						actives["Themes"].transform.Find("Tick").gameObject.SetActive(false);
						actives["Themes"] = shop_item_instance;
						actives["Themes"].transform.Find("Tick").gameObject.SetActive(true);
						PlayerPrefs.SetInt(actives["Themes"].name.Split(new String[] {"_"}, StringSplitOptions.None)[0], 2);
						UiManager.self.SetTheme();
                    } else {
						AudioManager.self.Play("lock");
					}
				}
				break;
				case "ColorsPanel":
					if(shop_item_instance == actives["Colors"])
						return;
					else {
						string[] x = shop_item_instance.name.Split(new String[] {"_"}, StringSplitOptions.None);
						string name = x[0];
						int cost = int.Parse(x[1]);	
					    bool unlock = PlayerPrefs.GetInt(name) != 1;

						if(!unlock && GameManager.self.coins >= cost) {
							GameManager.self.coins -= cost;
							UiManager.self.SetCoins();
							unlock = true;
							shop_item_instance.transform.Find("LockPanel").gameObject.SetActive(false);
						}
						if(unlock) {
							PlayerPrefs.SetInt(actives["Colors"].name.Split(new String[] {"_"}, StringSplitOptions.None)[0], 0);
							actives["Colors"].transform.Find("Tick").gameObject.SetActive(false);
							actives["Colors"] = shop_item_instance;
							actives["Colors"].transform.Find("Tick").gameObject.SetActive(true);
							PlayerPrefs.SetInt(actives["Colors"].name.Split(new String[] {"_"}, StringSplitOptions.None)[0], 2);
							UiManager.self.SetColor(int.Parse(actives["Colors"].name.Split(new String[] {"_"}, StringSplitOptions.None)[0].Split(new String[] {"."}, StringSplitOptions.None)[0]));
							AudioManager.self.Play("buy");
						} else {
							AudioManager.self.Play("lock");
						}
					}
					break;
			default:
				if(top_parent_panel.transform.parent.name == "FacesPanel") {
					string key = shop_item_instance.transform.parent.name;

					string[] x = shop_item_instance.name.Split(new String[] {"_"}, StringSplitOptions.None);
					string name = x[0];
					int cost = int.Parse(x[1]);
					bool unlock = PlayerPrefs.GetInt(name) != 1;

					if(actives.ContainsKey(key) && shop_item_instance == actives[key]) {
						PlayerPrefs.SetInt(actives[key].name.Split(new String[] {"_"}, StringSplitOptions.None)[0], 0);
						actives[key].transform.Find("Tick").gameObject.SetActive(false);
						actives[key] = null;
						UiManager.self.SetFace(key, null);
					} else {
						if(!unlock && GameManager.self.coins >= cost) {
							GameManager.self.coins -= cost;
							UiManager.self.SetCoins();
							unlock = true;
							shop_item_instance.transform.Find("LockPanel").gameObject.SetActive(false);
							AudioManager.self.Play("buy");
						}
						if(unlock) {
							if(actives.ContainsKey(key) && actives[key] != null) {
								PlayerPrefs.SetInt(actives[key].name.Split(new String[] {"_"}, StringSplitOptions.None)[0], 0);
								actives[key].transform.Find("Tick").gameObject.SetActive(false);
							}
							actives[key] = shop_item_instance;
							actives[key].transform.Find("Tick").gameObject.SetActive(true);
							PlayerPrefs.SetInt(actives[key].name.Split(new String[] {"_"}, StringSplitOptions.None)[0], 2);
							UiManager.self.SetFace(key, actives[key].name);
						} else {
							AudioManager.self.Play("lock");
						}
					}
				}
				break;
		}
	}

    public void OnShopButtonClick() {
		Util.GoToPanel(UiManager.self.menu_panel, shop_panel);
		player_overview_panel.SetActive(true);
		GameManager.self.on_player_views = false;
	}

	public void OnGunsPanelButtonClick() {
		ShopPanelIn(shop_guns_panel);
	}

	public void OnSpecialAbilitiesPanelButtonClick() {
		ShopPanelIn(shop_special_abilities_panel);
	}
	
	public void OnFacesPanelButtonClick() {
		ShopPanelIn(shop_faces_panel);
	}

	public void OnThemesPanelButtonClick() {
		ShopPanelIn(shop_themes_panel);
	}

	public void OnColorsPanelButtonClick() {
		ShopPanelIn(shop_colors_panel);
	}

	public void ShopPanelIn(GameObject panel) {
		panel.SetActive(true);

		panel.GetComponent<Animator>().SetTrigger("In");
		player_overview_panel.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Left");
		panel.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	public IEnumerator ShopPanelOut() {
		GameObject panel = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
		Animator panel_animator = panel.GetComponent<Animator>();
		
		player_overview_panel.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Right");
		panel_animator.SetTrigger("Out");

		panel.GetComponent<CanvasGroup>().blocksRaycasts = false;
		yield return new WaitForSeconds(1f);
		
		panel.SetActive(false);
	}

	private void AddMeta() {
		metas[Guns.PISTOL] = new string[]{"revolver".ToUpper(), "a deadly weapon\n\n" + GunController.self.guns[Guns.PISTOL].ToString().ToUpper()};
		metas[Guns.RIFLE] = new string[]{"m4a".ToUpper(), "a dead soldier's gun\n\n" + GunController.self.guns[Guns.RIFLE].ToString().ToUpper()};
		metas[Guns.SHOTGUN] = new string[]{"shotgun".ToUpper(), "a native gun\n\n" + GunController.self.guns[Guns.SHOTGUN].ToString().ToUpper()};

		metas[SpecialAbilities.BOUNCY] = new string[]{"fast devil".ToUpper(), "bounce like a devil.".ToUpper()};
		metas[SpecialAbilities.ENEMY_EARNER] = new string[]{"giant".ToUpper(), "punch any blue enemy you seee.".ToUpper()};
		metas[SpecialAbilities.GUNNER] = new string[]{"gunner".ToUpper(), "fast shoting and reloading.".ToUpper()};
		metas[SpecialAbilities.LUCKY] = new string[]{"lucky".ToUpper(), "more chance to find items.".ToUpper()};
		metas[SpecialAbilities.RANDOMER] = new string[]{"randomer".ToUpper(), "fill your item slots randomly.".ToUpper()};
	}

}