using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour {

	void Update () {
		if(!GameManager.self.paused)
			GetComponent<Renderer> ().material.mainTextureOffset +=
				Vector2.left * ((int) SpeedManager.self.game_speed) / 1000;
	}
}
