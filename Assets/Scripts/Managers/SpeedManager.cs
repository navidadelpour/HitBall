using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour {

	public static SpeedManager instance;

	public float game_speed;
	public float max_speed;
	public float min_speed;
	private float bound_increase_amount;
	private float speed_increase_amount;
	private float speed_decrease_amount;

	public float player_speed;

	void Init() {
		instance = this;
		min_speed = 5f;
		max_speed = 15f;
		bound_increase_amount = .001f;
		speed_increase_amount = .4f;
		speed_decrease_amount = .2f;
		game_speed = min_speed;

		player_speed = 10f;
	}

	void Start () {
		Init ();
	}

	void Update () {
		// TODO: increase speed algorithm
		min_speed += bound_increase_amount;
		max_speed += bound_increase_amount;
	}

	public void ShouldIncreaseSpeed(bool should_increase) {
		if (should_increase) {
			if (game_speed < max_speed)
				game_speed += speed_increase_amount;
		} else {
			if (game_speed > min_speed)
				game_speed -= speed_decrease_amount;
			else
				game_speed += speed_increase_amount;
		}
	}


}
