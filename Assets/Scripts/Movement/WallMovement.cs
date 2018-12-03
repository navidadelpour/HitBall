using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour {

	private Vector2 texture_offset;

	void Start () {
		
	}
	
	void Update () {
		GetComponent<Renderer> ().material.mainTextureOffset += Vector2.left * ((int) SpeedManager.instance.game_speed) / 1000;
	}
}
