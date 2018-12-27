using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public static PlayerMovement self;
	private float jump_time;
	private Rigidbody2D body;
	private bool jumping;

	void Awake() {
		self = this;
	}

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
			Fall ();
		}

		if(GameManager.self.item_activated && GameManager.self.item == Item.DOUBLE_JUMP) {
			if(!jumping){
				GameManager.self.RemoveItem();
				Jump();
			} else
				GameManager.self.item_activated = false;
		}

		if(GameManager.self.item_activated && GameManager.self.item == Item.FORCE_FALL) {
			if(jumping){
				GameManager.self.RemoveItem();
				Fall();
			} else
				GameManager.self.item_activated = false;
		}
	}

	public void Jump() {
		if (!jumping) {
			jump_time = Time.time;
			jumping = true;
			if (GameManager.self.should_remove_coil) {
				GameManager.self.should_remove_coil = false;
				GameManager.self.has_coil = false;
			}
		}
		body.velocity = Vector2.Lerp (
			Vector2.up * SpeedManager.self.player_speed,
			Vector2.zero,
			(Time.time - jump_time) * SpeedManager.self.player_speed /
			(GameManager.self.has_coil ? HeightManager.self.player_coil_jump_height : HeightManager.self.player_jump_height)
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
			(GameManager.self.has_coil ? HeightManager.self.player_coil_jump_height : HeightManager.self.player_jump_height)
		);
	}

}
