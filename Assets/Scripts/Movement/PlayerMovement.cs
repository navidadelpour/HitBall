using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	private float jump_time;
	private Rigidbody2D body;
	private bool jumping;

	void Init() {
		body = GetComponent<Rigidbody2D> ();
	}

	void Start () {
		Init ();
	}
	
	void FixedUpdate () {
		if (jumping)
			Jump ();
		else
			Fall ();

		if (body.velocity.y == 0) {
			jump_time = Time.time;
			jumping = false;
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Ground") {
			jump_time = Time.time;
			jumping = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Obstacle") {
			if (GameManager.instance.has_shield)
				return;
			else
				GameManager.instance.game_over = true;
		}
	}

	void Jump() {
		body.velocity = Vector2.Lerp (
			Vector2.up * SpeedManager.instance.player_speed,
			Vector2.zero,
			(Time.time - jump_time) * SpeedManager.instance.player_speed / 8f);
	}

	void Fall() {
		body.velocity = Vector2.Lerp (
			Vector2.zero,
			Vector2.down * SpeedManager.instance.player_speed,
			(Time.time - jump_time) * SpeedManager.instance.player_speed / 8f);
	}

}
