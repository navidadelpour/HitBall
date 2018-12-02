using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	public static InputManager instance;

	void Init() {
		instance = this;
	}

	void Start () {
		Init ();
	}
	
	void Update () {
		if (Input.GetMouseButton (0)) {
			GameManager.instance.ShouldIncreaseSpeed (true);
		} else {
			GameManager.instance.ShouldIncreaseSpeed (false);
		}

	}
}
