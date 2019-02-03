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

	public GameObject shop_panel;
	private GameObject shop_general_panel;
	private GameObject shop_guns_panel;
	private GameObject shop_special_abilities_panel;
	private GameObject shop_faces_panel;
	private GameObject shop_themes_panel;
	private GameObject shop_colors_panel;

	private GameObject faces_header;
	private GameObject faces_panel;
	private GameObject shop_item;
	private GameObject shop_faces_item;
	private GameObject shop_themes_item;
	private Vector3 shop_item_size;
	private Vector3 shop_faces_item_size;
	private float shop_margin = 30f;
	private int colors_cost = 10;

	public Dictionary<string, GameObject> actives = new Dictionary<string, GameObject>();

    void Awake() {
        self = this;

		tick_sprite = Resources.Load<Sprite>("Textures/UI/Main/tick");
		lock_sprite = Resources.Load<Sprite>("Textures/UI/Main/lock");

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
			shop_panel,
			shop_guns_panel,
			shop_special_abilities_panel,
			shop_faces_panel,
			shop_themes_panel,
			shop_colors_panel,
		});

		SetupShopGunsPanel();
		SetupShopSpecialAbilityPanel();
		SetupShopFacesPanel();
		SetupShopThemesPanel();
		SetupShopColorsPanel();
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
					actives["Guns"] = shop_item_created;
					UiManager.self.SetGun(enum_array[i].ToString());
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
					UiManager.self.SetSpecialAbility(enum_array[i].ToString());
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
			shop_item_created.name = sprite_array[i].name;
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

			shop_item_created.transform.Find("Image").gameObject.GetComponent<Image>().sprite = sprite_array[i];
			shop_item_created.transform.Find("Status").gameObject.GetComponent<Image>().sprite = sprite_to_give;
			shop_item_created.transform.Find("Cost").gameObject.GetComponent<Text>().text = cost;
		}

	}

	void SetupShopColorsPanel() {
		Color32[] color_array = PlayerPrefsManager.self.colors;

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

				string name = index + ".Color";
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
						UiManager.self.SetColor(index);
						break;
				}
				shop_item_created.name = name + "_" + (cost == "" ? "0" : cost);
				shop_item_created.transform.Find("Image").gameObject.GetComponent<Image>().color = color_array[index];
				shop_item_created.transform.Find("Status").gameObject.GetComponent<Image>().sprite = sprite_to_give;
				shop_item_created.transform.Find("Cost").gameObject.GetComponent<Text>().text = cost;
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
							UiManager.self.SetFace(postfixes[k], sprite_array[index].name);
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
				if(shop_item_instance == actives["SpecialAbilities"])
					return;
				PlayerPrefs.SetInt(actives["SpecialAbilities"].name, 0);
				actives["SpecialAbilities"].transform.Find("Status").GetComponent<Image>().sprite = null;
				actives["SpecialAbilities"] = shop_item_instance;
				actives["SpecialAbilities"].transform.Find("Status").GetComponent<Image>().sprite = tick_sprite;
				PlayerPrefs.SetInt(actives["SpecialAbilities"].name, 2);
				UiManager.self.SetSpecialAbility(actives["SpecialAbilities"].name);
				break;
			case "GunsPanel":
				if(shop_item_instance == actives["Guns"])
					return;
				PlayerPrefs.SetInt(actives["Guns"].name, 0);
				actives["Guns"].transform.Find("Status").GetComponent<Image>().sprite = null;
				actives["Guns"] = shop_item_instance;
				actives["Guns"].transform.Find("Status").GetComponent<Image>().sprite = tick_sprite;
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
                        unlock = true;
					}
                    if(unlock) {
						PlayerPrefs.SetInt(actives["Themes"].name.Split(new String[] {"_"}, StringSplitOptions.None)[0], 0);
						actives["Themes"].transform.Find("Status").GetComponent<Image>().sprite = null;
						actives["Themes"].transform.Find("Cost").GetComponent<Text>().text = "";
						actives["Themes"] = shop_item_instance;
						actives["Themes"].transform.Find("Status").GetComponent<Image>().sprite = tick_sprite;
						actives["Themes"].transform.Find("Cost").GetComponent<Text>().text = "";
						PlayerPrefs.SetInt(actives["Themes"].name.Split(new String[] {"_"}, StringSplitOptions.None)[0], 2);
						UiManager.self.SetTheme();
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
							unlock = true;
						}
						if(unlock) {
							PlayerPrefs.SetInt(actives["Colors"].name.Split(new String[] {"_"}, StringSplitOptions.None)[0], 0);
							actives["Colors"].transform.Find("Status").GetComponent<Image>().sprite = null;
							actives["Colors"].transform.Find("Cost").GetComponent<Text>().text = "";
							actives["Colors"] = shop_item_instance;
							actives["Colors"].transform.Find("Status").GetComponent<Image>().sprite = tick_sprite;
							actives["Colors"].transform.Find("Cost").GetComponent<Text>().text = "";
							PlayerPrefs.SetInt(actives["Colors"].name.Split(new String[] {"_"}, StringSplitOptions.None)[0], 2);
							UiManager.self.SetColor(int.Parse(actives["Colors"].name.Split(new String[] {"_"}, StringSplitOptions.None)[0].Split(new String[] {"."}, StringSplitOptions.None)[0]));
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
						actives[key].transform.Find("Status").GetComponent<Image>().sprite = null;
						actives[key].transform.Find("Cost").GetComponent<Text>().text = "";
						actives[key] = null;
						UiManager.self.SetFace(key, null);
					} else {
						if(!unlock && GameManager.self.coins >= cost) {
							GameManager.self.coins -= cost;
							unlock = true;
						}
						if(unlock) {
							if(actives.ContainsKey(key) && actives[key] != null) {
								PlayerPrefs.SetInt(actives[key].name.Split(new String[] {"_"}, StringSplitOptions.None)[0], 0);
								actives[key].transform.Find("Status").GetComponent<Image>().sprite = null;
								actives[key].transform.Find("Cost").GetComponent<Text>().text = "";
							}
							actives[key] = shop_item_instance;
							actives[key].transform.Find("Status").GetComponent<Image>().sprite = tick_sprite;
							actives[key].transform.Find("Cost").GetComponent<Text>().text = "";
							PlayerPrefs.SetInt(actives[key].name.Split(new String[] {"_"}, StringSplitOptions.None)[0], 2);
							UiManager.self.SetFace(key, actives[key].name);
						}
					}
				}
				break;
		}
	}

    public void OnShopButtonClick() {
		Util.GoToPanel(UiManager.self.menu_panel, shop_panel);
		AudioManager.self.Play("button");
	}

	public void OnGunsPanelButtonClick() {
		Util.GoToPanel(shop_panel, shop_guns_panel);
		AudioManager.self.Play("button");
	}

	public void OnSpecialAbilitiesPanelButtonClick() {
		Util.GoToPanel(shop_panel, shop_special_abilities_panel);
		AudioManager.self.Play("button");
	}
	
	public void OnFacesPanelButtonClick() {
		Util.GoToPanel(shop_panel, shop_faces_panel);
		AudioManager.self.Play("button");
	}

	public void OnThemesPanelButtonClick() {
		Util.GoToPanel(shop_panel, shop_themes_panel);
		AudioManager.self.Play("button");
	}

	public void OnColorsPanelButtonClick() {
		Util.GoToPanel(shop_panel, shop_colors_panel);
		AudioManager.self.Play("button");
	}

}