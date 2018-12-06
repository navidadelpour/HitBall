using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	public static SpawnManager instance;

	private GameObject coin_prefab;
	private GameObject ground_prefab;
	private GameObject[] obstacles_prefabs;

	private GameObject grounds;
	private GameObject coins;
	private GameObject obstacles;

	private float obstacle_delay_time = 2f;
	private int ground_limit = 7;
	private int hole_chance = 3;
	private bool can_spawn_hole = false;
	private int holes_in_row = 0;
	private int max_holes_in_row = 2;

	void Init() {
		instance = this;
		ground_prefab = Resources.Load <GameObject>("prefabs/Ground");
		coin_prefab = Resources.Load <GameObject>("prefabs/Coin");
		obstacles_prefabs = Resources.LoadAll <GameObject>("prefabs/Obstacles");

		grounds = GameObject.Find ("Grounds");

		coins = new GameObject ();
		coins.name = "Coins";

		obstacles = new GameObject ();
		obstacles.name = "Obstacles";
	}

	void Start () {
		Init ();
		for(int i = 0; i < ground_limit; i++)
			CreateGround ();
		can_spawn_hole = true;
		Invoke ("CreateObstacle", 1f);
		Invoke ("CreateCoin", 3f);
	}
	
	void Update () {
		
	}

	public void CreateGround() {
		GameObject last_item = grounds.transform.GetChild(grounds.transform.childCount - 1).gameObject;
		GameObject item_created = Instantiate(
			ground_prefab,
			last_item.transform.position + Vector3.right * last_item.GetComponent<BoxCollider2D>().size.x * last_item.transform.localScale.x,
			Quaternion.identity,
			grounds.transform
		);
		item_created.name = "Ground";
		item_created.tag = "Ground";

		if (can_spawn_hole && Random.Range (0, 10) < hole_chance && holes_in_row < max_holes_in_row) {
			item_created.GetComponent<BoxCollider2D> ().isTrigger = true;
			item_created.GetComponent<Renderer> ().enabled = false;
			item_created.name = "Hole";
			item_created.tag = "Hole";
			holes_in_row++;
		} else
			holes_in_row = 0;
	}

	public void CreateObstacle() {
		GameObject obstacle_created = Instantiate(
			obstacles_prefabs[Random.Range(0, obstacles_prefabs.Length - 1)],
			Vector2.right * 10f + Vector2.up * -3,
			Quaternion.identity,
			obstacles.transform
		);
		obstacle_created.tag = "Obstacle";
		Invoke ("CreateObstacle", Random.Range (1f, 1f));
	}

	public void CreateCoin() {
		for (int i = 0; i < Random.Range (3, 5); i++) {
			GameObject coin_created = Instantiate (
             	coin_prefab,
				Vector2.right * 10f + Vector2.up * i + Vector2.down * 3f,
				Quaternion.identity,
				coins.transform
          	);
			coin_created.tag = "Coin";
		}
		Invoke ("CreateCoin", Random.Range (1f, 3f));
	}

}
