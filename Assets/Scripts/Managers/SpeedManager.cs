using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour {

	public static SpeedManager self;

	public SpeedStates state;

	public float game_speed;
	private float game_normal_speed;
	private Dictionary<string, float> game_min_speed = new Dictionary<string, float>() {{"min", 4f}, {"max", 8f}};
	private Dictionary<string, float> game_max_speed = new Dictionary<string, float>() {{"min", 8f}, {"max", 16f}};
	private float game_speed_diffrence_amount = .2f;

	public float player_speed;
	private float player_normal_speed;
	private Dictionary<string, float> player_min_speed = new Dictionary<string, float>() {{"min", 8f}, {"max", 16f}};
	private Dictionary<string, float> player_max_speed = new Dictionary<string, float>() {{"min", 12f}, {"max", 24f}};
	private float player_speed_difference_amount = .2f;

	private float game_bound_increase_amount = 0.001f; 


	void Awake() {
		self = this;
	}

	void Init() {
		game_normal_speed = (game_min_speed["min"] + game_max_speed["min"]) / 2;
		player_normal_speed = (player_min_speed["min"] + player_max_speed["min"]) / 2;
		game_speed = game_normal_speed;
		player_speed = player_normal_speed;
	}

	void Start () {
		Init ();
	} 

	void Update () {
		MakeHard();
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
		if (game_speed < game_max_speed["min"])
			game_speed += game_speed_diffrence_amount;
		if (player_speed > player_min_speed["min"])
			player_speed -= player_speed_difference_amount;
	}

	public void DecreaseSpeed() {
		if (game_speed > game_min_speed["min"])
			game_speed -= game_speed_diffrence_amount;
		if (player_speed < player_max_speed["min"])
			player_speed += player_speed_difference_amount;
	}

	public void MakeHard() {
		game_normal_speed = (game_min_speed["min"] + game_max_speed["min"]) / 2;

		if(game_min_speed["min"] < game_min_speed["max"])
			game_min_speed["min"] += game_bound_increase_amount;

		if(game_max_speed["min"] < game_max_speed["max"])
			game_max_speed["min"] += game_bound_increase_amount;
	}

}
