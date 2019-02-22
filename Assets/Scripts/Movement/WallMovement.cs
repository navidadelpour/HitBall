using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour {

	Material material;

	void Awake() {
		material = GetComponent<Renderer> ().material;
	}

	void Update () {
		if(!GameManager.self.paused)
			material.mainTextureOffset += Vector2.left * ((int) SpeedManager.self.game_speed) / 3000;
	}
}
