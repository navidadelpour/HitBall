using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

	public static InputManager self;

	private GameObject canvas;
	GraphicRaycaster graphic_raycaster;
	PointerEventData pointer_event_data;
	EventSystem event_systems;

	private bool should_act;

	void Awake() {
		self = this;
	}

	void Init() {
		canvas = GameObject.Find ("Canvas");
		graphic_raycaster = canvas.GetComponent<GraphicRaycaster>();
		event_systems = canvas.GetComponent<EventSystem>();

	}

	void Start () {
		Init ();
	}

	void Update () {
		if (Input.anyKey) {
			pointer_event_data = new PointerEventData (event_systems);
			pointer_event_data.position = Input.mousePosition;

			List<RaycastResult> results = new List<RaycastResult> ();

			graphic_raycaster.Raycast (pointer_event_data, results);

			should_act = results.Count == 0 || results [0].gameObject.name != "PauseButton";
		} else
			should_act = false;
		
		if (should_act) {
			if (Input.GetMouseButton (0) && Input.mousePosition.x > Screen.width / 2) {
				if (Input.mousePosition.y > Screen.height / 2)
					SpeedManager.self.state = SpeedStates.INCREASE;
				else
					SpeedManager.self.state = SpeedStates.DECREASE;
			}

			GameManager.self.has_shield = Input.GetMouseButton (1);
		} else {
			SpeedManager.self.state = SpeedStates.NORMALIZE;
		}
	}

}
