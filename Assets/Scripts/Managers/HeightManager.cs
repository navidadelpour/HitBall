using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightManager : MonoBehaviour {

	public static HeightManager self;

	public float player_jump_height;
	public float player_coil_jump_height = 10f;
	private float player_max_jump_height = 8f;
	private float player_normal_jump_height = 6f;
	private float player_min_jump_height = 4f;

	void Awake() {
		self = this;
	}

	void Init() {
		player_jump_height = player_normal_jump_height;
	}

	void Start () {
		Init ();
	}

	public void SetHeight() {
		switch (SpeedManager.self.state) {
		case SpeedStates.INCREASE:
			player_jump_height = player_max_jump_height;
			break;
		case SpeedStates.NORMALIZE:
			player_jump_height = player_normal_jump_height;
			break;
		case SpeedStates.DECREASE:
			player_jump_height = player_min_jump_height;
			break;
		}
	}

}
