using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSweepMovement : MonoBehaviour {

	private Vector3 initial_height;
	private float range = 1.5f;
	private float time_should_take;
	private bool should_go_right;
	private float parameter = 0;

	void Awake() {
		initial_height = Vector3.up * transform.localPosition.y;
		time_should_take = Random.Range(1f, 4f);
	}

	void Start () {
		
	}
	
	void Update () {
		bool disabled = ItemManager.self.actives[Item.DISABLER];	
		if(should_go_right) {
			if(parameter < 1)
				parameter += disabled ? 0 : Time.deltaTime / time_should_take;
			else
				should_go_right = false;
		} else {
			if(parameter > 0)
				parameter -= disabled ? 0 : Time.deltaTime / time_should_take;
			else
				should_go_right = true;
		}
		transform.localPosition = Vector3.Lerp(Vector3.right * range, Vector3.left * range, parameter) + initial_height;
	}
}
