using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

	public static InputManager self;

	private GameObject canvas;
	private GraphicRaycaster graphic_raycaster;
	private PointerEventData pointer_event_data;
	private EventSystem event_systems;

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
				switch (results [0].gameObject.name) {
				case "JumpMaxButton":
					SpeedManager.self.state = SpeedStates.INCREASE;
					break;
				case "JumpMinButton":
					SpeedManager.self.state = SpeedStates.DECREASE;
					break;
				case "ItemButton":
					if(ItemManager.self.item != Item.NOTHING)
						ItemManager.self.item_activated = true;
						ItemManager.self.item_activated_index = int.Parse(results [0].gameObject.name);
					break;
				}
				
			}
		} else {
			SpeedManager.self.state = SpeedStates.NORMALIZE;
		}
	}
}
