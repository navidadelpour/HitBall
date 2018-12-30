using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public static PlayerMovement self;
	public bool jumping;
	private float jump_time;
	private Rigidbody2D body;

	void Awake() {
		self = this;

		body = GetComponent<Rigidbody2D> ();
	}

	void Start () {

	}
	
	void FixedUpdate () {
		if (jumping)
			Jump ();
		else
			Fall ();

		if (body.velocity.y == 0) {
			Fall ();
		}

		if(GameManager.self.has_double_jump){
			Jump();
			GameManager.self.has_double_jump = false;
		}

		if(GameManager.self.has_force_fall){
			Fall();
			GameManager.self.has_force_fall = false;
		}

		if(GameManager.self.has_teleport) {
			transform.position += Vector3.down * 10f;
			enabled = false;
		}
	}

	public void Jump() {
		if (!jumping) {
			jump_time = Time.time;
			jumping = true;
			if (HeightManager.self.should_remove_coil) {
				HeightManager.self.should_remove_coil = false;
				HeightManager.self.has_coil = false;
			}
		}
		body.velocity = Vector2.Lerp (
			Vector2.up * SpeedManager.self.player_speed,
			Vector2.zero,
			(Time.time - jump_time) * SpeedManager.self.player_speed /
			(HeightManager.self.has_coil ? HeightManager.self.player_coil_jump_height : HeightManager.self.player_jump_height)
		);
	}

	void Fall() {
		if (jumping) {
			jump_time = Time.time;
			jumping = false;
		}
		body.velocity = Vector2.Lerp (
			Vector2.zero,
			Vector2.down * SpeedManager.self.player_speed,
			(Time.time - jump_time) * SpeedManager.self.player_speed /
			(HeightManager.self.has_coil ? HeightManager.self.player_coil_jump_height : HeightManager.self.player_jump_height)
		);
	}

}
