using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

	public static InputManager instance;

	private GameObject canvas;
	GraphicRaycaster graphic_raycaster;
	PointerEventData pointer_event_data;
	EventSystem event_systems;

	private bool should_act;

	void Init() {
		instance = this;
		canvas = GameObject.Find ("Canvas");
		graphic_raycaster = canvas.GetComponent<GraphicRaycaster>();
		event_systems = canvas.GetComponent<EventSystem>();

	}

	void Start () {
		Init ();
	}

	void Update () {
		if (Input.anyKey) {
			pointer_event_data = new PointerEventData(event_systems);
			pointer_event_data.position = Input.mousePosition;

			List<RaycastResult> results = new List<RaycastResult>();

			graphic_raycaster.Raycast(pointer_event_data, results);

			should_act = results.Count == 0 || results[0].gameObject.name != "PauseButton";
		}

		if (should_act) {
			game_act ();
		}
	}

	void game_act() {
		SpeedManager.instance.ShouldIncreaseSpeed (Input.GetMouseButton (0));
		GameManager.instance.has_shield = Input.GetMouseButton (1);
	}

}
