using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	public static SpawnManager instance;

	public GameObject ground;
	public GameObject coin;
	public GameObject[] obstacles_prefabs;

	private GameObject grounds;
	private GameObject coins;
	private GameObject obstacles;

	private float obstacle_delay_time;

	void Init() {
		instance = this;
		obstacle_delay_time = 2f;
		grounds = GameObject.Find ("Grounds");
		coins = new GameObject ();
		coins.name = "Coins";
		obstacles = new GameObject ();
		obstacles.name = "Obstacles";
	}

	void Start () {
		Init ();
		CreateGround ();
		Invoke ("CreateObstacle", 1f);
		Invoke ("CreateCoin", 3f);
	}
	
	void Update () {
		
	}

	public void CreateGround() {
		GameObject ground = GameObject.Find ("Ground");
		GameObject ground_created = Instantiate(
			ground,
			ground.transform.position + Vector3.right * ground.GetComponent<BoxCollider2D>().size.x * ground.transform.localScale.x,
			Quaternion.identity,
			grounds.transform
		);
		ground_created.name = "Ground";
		ground_created.tag = "Ground";
	}

	public void CreateObstacle() {
		GameObject obstacle_created = Instantiate(
			obstacles_prefabs[Random.Range(0, obstacles_prefabs.Length - 1)],
			Vector2.right * 10f + Vector2.up * Random.Range(-3f, 0f),
			Quaternion.identity,
			obstacles.transform
		);
		obstacle_created.tag = "Obstacle";
		Invoke ("CreateObstacle", Random.Range (2, 5));
	}

	public void CreateCoin() {
		for (int i = 0; i < Random.Range (3, 5); i++) {
			GameObject coin_created = Instantiate (
             	coin,
				Vector2.right * 10f + Vector2.up * i + Vector2.down * 3f,
				Quaternion.identity,
				coins.transform
          	);
			coin_created.tag = "Coin";
		}
		Invoke ("CreateCoin", Random.Range (1, 3));
	}

}
