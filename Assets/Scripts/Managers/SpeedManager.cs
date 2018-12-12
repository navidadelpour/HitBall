using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour {

	public static SpeedManager self;

	public float game_speed;
	public float max_speed;
	public float normal_speed;
	public float min_speed;
	private float bound_increase_amount;
	private float speed_diffrence_amount;

	public float player_speed;
	public float player_max_speed;
	public float player_normal_speed;
	public float player_min_speed;
	public float player_speed_difference_amount;

	void Awake() {
		self = this;
	}

	void Init() {
		max_speed = 10f;
		normal_speed = 7f;
		min_speed = 4f;
		bound_increase_amount = .001f;
		speed_diffrence_amount = .2f;
		game_speed = normal_speed;

		player_max_speed = 12f;
		player_normal_speed = 10f;
		player_min_speed = 8f;
		player_speed_difference_amount = .2f;
		player_speed = player_normal_speed;
	}

	void Start () {
		Init ();
	} 

	void Update1 () {
		max_speed += bound_increase_amount;
		normal_speed += bound_increase_amount;
		min_speed += bound_increase_amount;
	}

	public void NormalizeSpeed() {
		if (game_speed > normal_speed)
			game_speed -= speed_diffrence_amount;
		else 
			game_speed += speed_diffrence_amount;

		if (player_speed > player_normal_speed)
			player_speed -= player_speed_difference_amount;
		else
			player_speed += player_speed_difference_amount;
	}

	public void IncreaseSpeed() {
		if (game_speed < max_speed)
			game_speed += speed_diffrence_amount;
		if (player_speed > player_min_speed)
			player_speed -= player_speed_difference_amount;
	}

	public void DecreaseSpeed() {
		if (game_speed > min_speed)
			game_speed -= speed_diffrence_amount;
		if (player_speed < player_max_speed)
			player_speed += player_speed_difference_amount;
		
	}

}
