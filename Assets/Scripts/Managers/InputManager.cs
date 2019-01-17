using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour {

	public static InputManager self;

	private GameObject canvas;
	private GraphicRaycaster graphic_raycaster;
	private PointerEventData pointer_event_data;
	private EventSystem event_systems;

	private bool gun_button_pressed;

	void Awake() {
		self = this;
		
		canvas = GameObject.Find ("Canvas");
		graphic_raycaster = canvas.GetComponent<GraphicRaycaster>();
		event_systems = canvas.GetComponent<EventSystem>();
	}

	void Start () {

	}

	void Update () {
		if (Input.anyKey) {
			pointer_event_data = new PointerEventData (event_systems);
			pointer_event_data.position = Input.mousePosition;

			List<RaycastResult> results = new List<RaycastResult> ();

			graphic_raycaster.Raycast (pointer_event_data, results);

			if (results.Count != 0 && results [0].gameObject.name != "PauseButton") {
				try {
					int index = int.Parse(results [0].gameObject.name);
					if(ItemManager.self.available_items[index].item != Item.NOTHING) {
						ItemManager.self.ActiveItem(index);
					}
				} catch (System.Exception) {
					switch (results [0].gameObject.name) {
					case "JumpMaxButton":
						SpeedManager.self.state = SpeedStates.INCREASE;
						break;
					case "JumpMinButton":
						SpeedManager.self.state = SpeedStates.DECREASE;
						break;
					case "GunButton":
						// GunController.self.Shot();
						break;
					case "SpecialAbilityButton":
						SpecialAbilityManager.self.Active();
						break;
					}
				}
			}
		} else {
			SpeedManager.self.state = SpeedStates.NORMALIZE;
		}

		if(gun_button_pressed)
			GunController.self.Shot();

	}

	public void OnPlayButtonClick() {
		GameManager.self.started = true;
	}

	public void OnPauseButtonClick() {
		if(GameManager.self.paused)
			Time.timeScale = 1;
		else
			Time.timeScale = 0;
		GameManager.self.paused = !GameManager.self.paused;
	}

	public void OnResetButtonClick() {
		OnPauseButtonClick();
		SceneManager.LoadScene("Scene1");
	}

	public void OnGunbuttonPointerDown() {
		gun_button_pressed = true;
	}

	public void OnGunbuttonPointerUp() {
		gun_button_pressed = false;
	}

}
