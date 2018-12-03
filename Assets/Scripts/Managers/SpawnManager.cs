using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	public static SpawnManager instance;

	public GameObject ground;
	public GameObject[] obstacles;

	private float obstacle_delay_time;

	void Init() {
		instance = this;
		obstacle_delay_time = 1f;
	}

	void Start () {
		Init ();
		CreateGround ();
		InvokeRepeating ("CreateObstacle", 2f, 1f);
	}
	
	void Update () {
		
	}

	public void CreateGround() {
		GameObject ground = GameObject.Find ("Ground");
		GameObject ground_created = Instantiate(
			ground,
			ground.transform.position + Vector3.right * ground.GetComponent<BoxCollider2D>().size.x * ground.transform.localScale.x,
			Quaternion.identity,
			GameObject.Find("Grounds").transform
		);
		ground_created.name = "Ground";
		ground_created.tag = "Ground";
	}

	public void CreateObstacle() {
		GameObject obstacle_created = Instantiate(
			obstacles[Random.Range(0, obstacles.Length)],
			Vector2.right * 10f + Vector2.up * Random.Range(-4, 4),
			Quaternion.identity,
			GameObject.Find("Obstacles").transform
		);
		obstacle_created.tag = "Obstacle";
	}

}
