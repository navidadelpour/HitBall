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
			RaycastHit2D hit = Physics2D.Raycast((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if(hit.collider != null) {
				Debug.Log (hit.collider.gameObject);
				if (hit.collider.gameObject.layer == 5)
					can_act = false;
				else
					can_act = true;
			}
		}

		if (can_act) {
			SpeedManager.instance.ShouldIncreaseSpeed (Input.GetMouseButton (0));
			GameManager.instance.has_shield = Input.GetMouseButton (1);
		}
	}
}
