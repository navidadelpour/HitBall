using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private float speed;
	private Vector2 jump_velocity;
	private float jump_time;
	private Rigidbody2D body;

	void Init() {
		speed = 10f;
		jump_time = 1f;
		jump_velocity = Vector2.up * speed;
		body = GetComponent<Rigidbody2D> ();
	}

	void Start () {
		Init ();
	}
	
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Ground") {
			Jump ();
		}
	}

	void Jump() {
		body.velocity += Vector2.Lerp (body.velocity, jump_velocity, jump_time);
	}
}
