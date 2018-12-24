using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour {

	public static SpeedManager self;

	public SpeedStates state;

	public float game_speed;
	private float game_max_speed = 8f;
	private float game_normal_speed = 6f;
	private float game_min_speed = 4f;
	private float game_speed_diffrence_amount = .2f;
	private float game_bound_increase_amount = 0.0001f; 

	public float player_speed;
	private float player_max_speed = 12f;
	private float player_normal_speed = 10f;
	private float player_min_speed = 8f;
	private float player_speed_difference_amount = .2f;


	void Awake() {
		self = this;
	}

	void Init() {
		game_speed = game_normal_speed;
		player_speed = player_normal_speed;
	}

	void Start () {
		Init ();
	} 

	void Update () {
		game_max_speed += game_bound_increase_amount;
		game_normal_speed += game_bound_increase_amount;
		game_min_speed += game_bound_increase_amount;

		switch (state) {
		case SpeedStates.INCREASE:
			IncreaseSpeed ();
			break;
		case SpeedStates.NORMALIZE:
			NormalizeSpeed ();
			break;
		case SpeedStates.DECREASE:
			DecreaseSpeed ();
			break;
		}
	}

	public void NormalizeSpeed() {
		if (game_speed > game_normal_speed)
			game_speed -= game_speed_diffrence_amount;
		else 
			game_speed += game_speed_diffrence_amount;

		if (player_speed > player_normal_speed)
			player_speed -= player_speed_difference_amount;
		else
			player_speed += player_speed_difference_amount;
	}

	public void IncreaseSpeed() {
		if (game_speed < game_max_speed)
			game_speed += game_speed_diffrence_amount;
		if (player_speed > player_min_speed)
			player_speed -= player_speed_difference_amount;
	}

	public void DecreaseSpeed() {
		if (game_speed > game_min_speed)
			game_speed -= game_speed_diffrence_amount;
		if (player_speed < player_max_speed)
			player_speed += player_speed_difference_amount;
		
	}

}
