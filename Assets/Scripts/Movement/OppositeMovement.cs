using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositeMovement : MonoBehaviour {

	private float speed;
	private Rigidbody2D body;

	void Awake() {
		body = GetComponent<Rigidbody2D> ();
	}

	void Start () {

	}
	
	void FixedUpdate () {
		body.velocity = Vector2.left * SpeedManager.self.game_speed * (this.tag == "Block" ? Random.Range(.25f, .75f) : 1);
	}

}
