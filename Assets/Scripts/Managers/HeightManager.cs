using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightManager : MonoBehaviour {

	public static HeightManager self;

	public float player_jump_height;
	private float jump_increase_amount = 1f;
	public float player_coil_jump_height;
	private float player_max_jump_height;
	private float player_normal_jump_height;
	private float player_min_jump_height;

	public bool has_coil;
	public bool should_remove_coil;

	void Awake() {
		self = this;

		player_normal_jump_height = (int) Camera.main.orthographicSize - 5 + 8;
		player_jump_height = player_normal_jump_height;
	}

	void Start () {

	}

	void Update() {
		if(GameManager.self.started)
			MakeHard();
		if (has_coil && !should_remove_coil)
			Invoke ("SetShouldRemoveCoil", .2f);

		if(ItemManager.self.actives[Items.WEB]) {
			player_jump_height = player_min_jump_height;
		}
	}

	void SetShouldRemoveCoil() {
		should_remove_coil = true;
	}


	void MakeHard() {
		player_coil_jump_height = (int) (Camera.main.orthographicSize - 5) * 2 + 12;
		player_max_jump_height = (int) (Camera.main.orthographicSize - 5) * 2 + 12;
		player_normal_jump_height = (int) (Camera.main.orthographicSize - 5) * 2 + 8;
		player_min_jump_height = (int) (Camera.main.orthographicSize - 5) * 2 + 4;
	}

	public void SetHeight() {
		if(GameManager.self.gameover) {
			player_jump_height = 1;
			return;
		}
		switch (SpeedManager.self.state) {
			case SpeedStates.INCREASE:
				if(ItemManager.self.actives[Items.JUMP_POWER])
					player_jump_height = player_max_jump_height;
				else {
					if(player_jump_height < player_normal_jump_height)
						player_jump_height = player_normal_jump_height;
					else if(player_jump_height < player_max_jump_height)
						player_jump_height += jump_increase_amount;
				}
				break;
			case SpeedStates.NORMALIZE:
					player_jump_height = player_normal_jump_height;
				break;
			case SpeedStates.DECREASE:
				if(ItemManager.self.actives[Items.JUMP_POWER])
					player_jump_height = player_min_jump_height;
				else {
					if(player_jump_height > player_normal_jump_height)
						player_jump_height = player_normal_jump_height;
					else if(player_jump_height > player_min_jump_height)
						player_jump_height -= jump_increase_amount;
				}
				break;
		}
	}

}
