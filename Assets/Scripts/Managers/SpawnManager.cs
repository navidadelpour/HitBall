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

	private GameObject last_item;
	private Vector3 on_ground_position;

	private int hole_chance = 30;
	private int obstacle_chance = 30;
	private int coin_chance = 30;


	private float obstacle_delay_time = 2f;
	private int ground_limit = 10;
	private bool is_safe = true;
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
		on_ground_position = Vector3.up * last_item.GetComponent<BoxCollider2D> ().size.y + Vector3.right * last_item.GetComponent<BoxCollider2D> ().size.x * last_item.transform.localScale.x;
		is_safe = false;
	}
	
	public void CreateGround() {
		last_item = grounds.transform.GetChild (grounds.transform.childCount - 1).gameObject;
		GameObject item_created = Instantiate (
	        ground_prefab,
	        last_item.transform.position + Vector3.right * last_item.GetComponent<BoxCollider2D> ().size.x * last_item.transform.localScale.x,
	        Quaternion.identity,
			grounds.transform
		);
		item_created.name = "Ground";
		item_created.tag = "Ground";


		if (!is_safe){
			if (HasChance (hole_chance) && holes_in_row < max_holes_in_row) {
				item_created.GetComponent<BoxCollider2D> ().isTrigger = true;
				item_created.GetComponent<BoxCollider2D> ().offset = new Vector3(0, -0.5f);
				item_created.GetComponent<Renderer> ().enabled = false;
				item_created.name = "Hole";
				item_created.tag = "Hole";
				holes_in_row++;
			} else {
				holes_in_row = 0;
				if (HasChance (obstacle_chance))
					CreateObstacle ();
				else if (HasChance (coin_chance))
					CreateCoin ();
			}
		}
	}

	public void CreateObstacle() {
		GameObject obstacle_created = Instantiate(
			obstacles_prefabs[Random.Range(0, obstacles_prefabs.Length - 1)],
			last_item.transform.position + on_ground_position,
			Quaternion.identity,
			obstacles.transform
		);
		obstacle_created.tag = "Obstacle";
	}

	public void CreateCoin() {
		GameObject coins_group = new GameObject ();
		coins_group.name = "CoinGroup";
		coins_group.transform.parent = coins.transform;
		for (int i = 0; i < Random.Range (3, 5); i++) {
			GameObject coin_created = Instantiate (
             	coin_prefab,
				last_item.transform.position + Vector3.up * i + on_ground_position,
				Quaternion.identity,
				coins.transform
          	);
			coin_created.tag = "Coin";
			coin_created.name = "Coin";
			coin_created.transform.parent = coins_group.transform;
		}
	}

	public bool HasChance(int chance) {
		return Random.Range (0, 100) < hole_chance;
	}
}
