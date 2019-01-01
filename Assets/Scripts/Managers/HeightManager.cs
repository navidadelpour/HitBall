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
		MakeHard();
		if (has_coil && !should_remove_coil)
			Invoke ("SetShouldRemoveCoil", .2f);

	}

	void SetShouldRemoveCoil() {
		should_remove_coil = true;
	}


	void MakeHard() {
		player_coil_jump_height = (int) Camera.main.orthographicSize - 5 + 12;
		player_max_jump_height = (int) Camera.main.orthographicSize - 5 + 12;
		player_normal_jump_height = (int) Camera.main.orthographicSize - 5 + 8;
		player_min_jump_height = (int) Camera.main.orthographicSize - 5 + 4;

	}

	public void SetHeight() {
		switch (SpeedManager.self.state) {
		case SpeedStates.INCREASE:
			if(ItemManager.self.actives[Item.JUMP_POWER])
				player_jump_height = player_max_jump_height;
			else {
				if(player_jump_height < player_normal_jump_height)
					player_jump_height = player_normal_jump_height;
				else if(player_jump_height < player_max_jump_height)
					player_jump_height += jump_increase_amount;
			}
			break;
		case SpeedStates.NORMALIZE:
			if(ItemManager.self.actives[Item.JUMP_POWER])
				player_jump_height = player_normal_jump_height;
			else {
				if(player_jump_height > player_normal_jump_height)
					player_jump_height -= jump_increase_amount;
				else if (player_jump_height < player_normal_jump_height)
					player_jump_height += jump_increase_amount;
			}
			break;
		case SpeedStates.DECREASE:
			if(ItemManager.self.actives[Item.JUMP_POWER])
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
