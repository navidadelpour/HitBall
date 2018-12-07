using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	public static SpawnManager instance;

	private GameObject coin_prefab;
	private GameObject coins;
	private int coin_chance = 30;

	private GameObject coil_prefab;
	private GameObject coils;
	private int coil_chance = 30;

	private GameObject ground_prefab;
	private GameObject grounds;
	private int ground_limit = 10;

	private int hole_chance = 30;
	private int holes_in_row = 0;
	private int max_holes_in_row = 2;

	private GameObject[] obstacles_prefabs;
	private GameObject obstacles;
	private int obstacle_chance = 30;

	private GameObject last_item;
	private Vector3 on_ground_offset;

	private bool is_safe = true;

	void Awake() {
		instance = this;
	}

	void Init() {
		ground_prefab = Resources.Load <GameObject>("prefabs/Ground");
		coin_prefab = Resources.Load <GameObject>("prefabs/Coin");
		coil_prefab = Resources.Load <GameObject>("prefabs/Coil");
		obstacles_prefabs = Resources.LoadAll <GameObject>("prefabs/Obstacles");

		grounds = GameObject.Find ("Grounds");

		coins = new GameObject ();
		coins.name = "Coins";

		coils = new GameObject ();
		coils.name = "Coils";

		obstacles = new GameObject ();
		obstacles.name = "Obstacles";
	}

	void Start () {
		Init ();
		for(int i = 0; i < ground_limit; i++)
			CreateGround ();
		is_safe = false;
		on_ground_offset = Vector3.up * last_item.GetComponent<BoxCollider2D> ().size.y + Vector3.right * last_item.GetComponent<BoxCollider2D> ().size.x * last_item.transform.localScale.x;
	}

	public bool HasChance(int chance) {
		return Random.Range (0, 100) < chance;
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
				if (HasChance (obstacle_chance) && holes_in_row == 0)
					CreateObstacle ();
				else if (HasChance (coin_chance))
					CreateCoin ();
				else if (HasChance (coil_chance))
					CreateCoil ();
				holes_in_row = 0;
			}
		}
	}

	public void CreateObstacle() {
		GameObject obstacle_created = Instantiate(
			obstacles_prefabs[Random.Range(0, obstacles_prefabs.Length - 1)],
			last_item.transform.position + on_ground_offset,
			Quaternion.identity,
			obstacles.transform
		);
		obstacle_created.tag = "Obstacle";
	}

	public void CreateCoin() {
		GameObject coins_group = new GameObject ();
		coins_group.name = "CoinGroup";
		coins_group.transform.parent = coins.transform;
		for (int i = 0; i < Random.Range (1, 4); i++) {
			GameObject coin_created = Instantiate (
             	coin_prefab,
				last_item.transform.position + Vector3.up * i + on_ground_offset,
				Quaternion.identity,
				coins.transform
          	);
			coin_created.tag = "Coin";
			coin_created.transform.parent = coins_group.transform;
		}
	}

	public void CreateCoil() {
		GameObject coil_created = Instantiate(
			coil_prefab,
			last_item.transform.position + on_ground_offset,
			Quaternion.identity,
			coils.transform
		);
		coil_created.tag = "Coil";
	}

}
