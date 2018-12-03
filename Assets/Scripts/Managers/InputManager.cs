using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	public static InputManager instance;

	public bool can_act;

	void Init() {
		instance = this;
	}

	void Start () {
		Init ();
	}
	
	void Update () {
		
		if (Input.anyKey) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)) {
				Debug.Log (hit.collider.name);
				if (hit.collider.gameObject.layer == 5) {
					can_act = false;
				} else
					can_act = true;
			}
		}

		if (can_act) {
			SpeedManager.instance.ShouldIncreaseSpeed (Input.GetMouseButton (0));
			GameManager.instance.has_shield = Input.GetMouseButton (1);
		}
	}
}
