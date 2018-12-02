using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private float speed_x;
	private float speed_y;
	private Vector2 jump_velocity;
	private Vector2 move_velocity;
	private float jump_time;
	private Rigidbody2D body;

	void Init() {
		speed_x = 10f;
		speed_y = 10f;
		jump_time = 1f;
		move_velocity = Vector2.right * speed_x;
		jump_velocity = Vector2.up * speed_y;
		body = GetComponent<Rigidbody2D> ();
	}

	// Use this for initialization
	void Start () {
		Init ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Ground") {
			Jump ();
		}
	}

	void Jump() {
		//while (body.velocity != jump_velocity) {
			body.velocity = Vector2.Lerp (body.velocity, jump_velocity, jump_time);
		//}
	}
}
