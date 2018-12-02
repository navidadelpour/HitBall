using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositeMovement : MonoBehaviour {

	private float speed;
	private Vector2 opposite_velocity;
	private Rigidbody2D body;

	void Init() {
		speed = 5f;
		opposite_velocity = Vector3.left * speed;
		body = GetComponent<Rigidbody2D> ();
	}

	void Start () {
		Init ();
	}
	
	void FixedUpdate () {
		body.velocity = opposite_velocity;
	}
}
