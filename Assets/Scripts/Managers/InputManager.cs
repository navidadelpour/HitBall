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
		SpeedManager.instance.ShouldIncreaseSpeed (Input.GetMouseButton (0));
		GameManager.instance.has_shield = Input.GetMouseButton(1);
	}
}
