using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public static PlayerMovement self;
	public bool jumping;
	private float jump_time;
	private Rigidbody2D body;
	private int rotate_angle = 15;
	private float min_scale = .5f;
	private float scale_amount = 1;

	void Awake() {
		self = this;

		body = GetComponent<Rigidbody2D> ();
	}

	void Start () {

	}
	
	void FixedUpdate () {
		Rotate(rotate_angle);
		Scale();

		if (jumping)
			Jump ();
		else
			Fall ();

		if (body.velocity.y == 0) {
			Fall ();
		}

		if(ItemManager.self.actives[Item.DOUBLE_JUMP]) {
			Jump();
			ItemManager.self.actives[Item.DOUBLE_JUMP] = false;
		}

		if(ItemManager.self.actives[Item.FORCE_FALL]) {
			Fall();
			ItemManager.self.actives[Item.FORCE_FALL] = false;
		}


		if(ItemManager.self.actives[Item.TELEPORT]) {
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

	public void Fall() {
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

	private void Scale() {
		if(ItemManager.self.actives[Item.SCALER])
			scale_amount = Util.Ease(min_scale, scale_amount, .1f, -1);
		else
			scale_amount = Util.Ease(1, scale_amount, .1f);

		transform.localScale = Vector3.one * scale_amount
		+ Vector3.up *  Mathf.Abs(body.velocity.y) * Time.deltaTime * 1.5f
		+ Vector3.right * (.2f -  Mathf.Abs(body.velocity.y) * Time.deltaTime * 1.5f);

		if(transform.position.y < 2.1f)
			transform.localScale = Vector3.one * scale_amount
			+ Vector3.right * Mathf.Abs(transform.position.y - 2.1f) * Time.deltaTime * 30f * scale_amount;

	}

	private void Rotate(int angle) {
		float t = (Time.time - jump_time) * SpeedManager.self.player_speed /
			(HeightManager.self.has_coil ? HeightManager.self.player_coil_jump_height : HeightManager.self.player_jump_height);
		transform.localEulerAngles = Vector3.Lerp(
			Vector3.forward * (jumping ? 1 : -1) * angle,
			Vector3.zero,
			jumping ? t : 1 - t
		);

	}

}
