using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositeMovement : MonoBehaviour {

	private float speed;
	private Rigidbody2D body;

	void Init() {
		body = GetComponent<Rigidbody2D> ();
	}

	void Start () {
		Init ();
	}
	
	void FixedUpdate () {
		body.velocity = Vector2.left * SpeedManager.self.game_speed;
	}

}
