using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour {

	private Vector3 direction;
	private float speed_scale;
	private float min_speed = 3;

	void Awake() {
		bool b = Random.Range(0, 2) == 0;
		direction = b ? Vector3.left : Vector3.down;
		transform.position += b ? Vector3.up * Random.Range(1f, 5f) : Vector3.up * 14 + Vector3.right * Random.Range(1f, 5f);
		speed_scale = Random.Range(1f, 2f);
	}

	void Start () {

	}
	
	void Update () {
		transform.Translate(direction * Time.deltaTime * speed_scale * min_speed);
	}
}
