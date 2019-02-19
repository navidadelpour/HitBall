using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour {

	public static SpeedManager self;

	public SpeedStates state;

	public float game_speed;
	private float game_normal_speed;
	private Dictionary<string, float> game_min_speed = new Dictionary<string, float>() {{"min", 4f}};
	private Dictionary<string, float> game_max_speed = new Dictionary<string, float>() {{"min", 10f}};
	private float game_speed_diffrence_amount = .1f;

	public float player_speed;
	private float player_normal_speed;
	private Dictionary<string, float> player_min_speed = new Dictionary<string, float>() {{"min", 8f}};
	private Dictionary<string, float> player_max_speed = new Dictionary<string, float>() {{"min", 14f}};
	private float player_speed_difference_amount = .1f;

	private float game_bound_increase_amount = 0.002f; 
	private float slow_motion_scale = 2f;


	void Awake() {
		self = this;

		game_normal_speed = (game_min_speed["min"] + game_max_speed["min"]) / 2;
		player_normal_speed = (player_min_speed["min"] + player_max_speed["min"]) / 2;
		game_min_speed["max"] = game_min_speed["min"] * 2;
		game_max_speed["max"] = game_max_speed["min"] * 2;
		player_max_speed["max"] = player_max_speed["min"] * 2;
		player_min_speed["max"] = player_min_speed["min"] * 2;
		game_speed = game_normal_speed;
		player_speed = player_normal_speed;
	}

	void Start () {

	} 

	void Update () {
		if(GameManager.self.gameover) {
			if(player_speed > 2)
				player_speed = 2;
			if(player_speed > 0)
				player_speed -= player_speed_difference_amount * Time.deltaTime;
			if(game_speed > 2)
				game_speed = 2;
			if(game_speed > 0)
				game_speed -= game_speed_diffrence_amount * Time.deltaTime;
			return;
		}
		if(GameManager.self.started)
			MakeHard();
		bool has_slow_motion = ItemManager.self.actives[Items.SLOW_MOTION];
		if(!ItemManager.self.actives[Items.WINGS]) {
			switch (state) {
			case SpeedStates.INCREASE:
				if(ItemManager.self.actives[Items.JUMP_POWER]) {
					game_speed = game_max_speed["min"];
					player_speed = player_min_speed["min"];
				} else {
					if (game_speed < game_max_speed["min"] / (has_slow_motion ? slow_motion_scale : 1f))
						game_speed += game_speed_diffrence_amount;
					if (player_speed > player_min_speed["min"])
						player_speed -= player_speed_difference_amount;
				}
				break;
			case SpeedStates.NORMALIZE:
				if(ItemManager.self.actives[Items.JUMP_POWER]) {
					game_speed = game_normal_speed;
					player_speed = player_normal_speed;
				} else {
					if (game_speed > game_normal_speed / (has_slow_motion ? slow_motion_scale : 1f))
						game_speed -= game_speed_diffrence_amount;
					else 
						game_speed += game_speed_diffrence_amount;

					if (player_speed > player_normal_speed)
						player_speed -= player_speed_difference_amount;
					else
						player_speed += player_speed_difference_amount;
				}
				break;
			case SpeedStates.DECREASE:
				if(ItemManager.self.actives[Items.JUMP_POWER]) {
					game_speed = game_min_speed["min"];
					player_speed = player_max_speed["min"];
				} else {
					if (game_speed > game_min_speed["min"] / (has_slow_motion ? slow_motion_scale : 1f))
						game_speed -= game_speed_diffrence_amount;
					if (player_speed < player_max_speed["min"])
						player_speed += player_speed_difference_amount;
				}
				break;
			}
		}
	}

	public void MakeHard() {
		game_normal_speed = (game_min_speed["min"] + game_max_speed["min"]) / 2;

		if(game_min_speed["min"] < game_min_speed["max"])
			game_min_speed["min"] += game_bound_increase_amount;

		if(game_max_speed["min"] < game_max_speed["max"])
			game_max_speed["min"] += game_bound_increase_amount;
	}

}
