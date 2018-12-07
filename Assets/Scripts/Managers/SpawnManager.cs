using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	public static SpawnManager self;

	private GameObject coin_prefab;
	private GameObject coil_prefab;
	private GameObject ground_prefab;
	private GameObject[] obstacles_prefabs;
	private GameObject grounds;
	private GameObject last_item;

	private int coin_chance = 30;
	private int coil_chance = 30;
	private int hole_chance = 30;
	private int obstacle_chance = 30;

	private int coins_in_order;
	private int coils_in_order;
	private int holes_in_order;
	private int obstacles_in_order;

	private int max_coins_in_order = 1;
	private int max_coils_in_order = 2;
	private int max_holes_in_order = 2;
	private int max_obstacles_in_order = 2;

	private int ground_limit = 6;
	private Vector3 on_ground_offset;
	private bool is_safe = true;

	void Awake() {
		self = this;
	}

	void Init() {
		ground_prefab = Resources.Load <GameObject>("prefabs/Ground");
		coin_prefab = Resources.Load <GameObject>("prefabs/Coin");
		coil_prefab = Resources.Load <GameObject>("prefabs/Coil");
		obstacles_prefabs = Resources.LoadAll <GameObject>("prefabs/Obstacles");

		grounds = GameObject.Find ("Grounds");

	}

	void Start () {
		Init ();
		for(int i = 0; i < ground_limit; i++)
			CreateGround ();
		is_safe = false;
		on_ground_offset = Vector3.up * last_item.GetComponent<BoxCollider2D> ().size.y;
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
		last_item = item_created;

		if (!is_safe){
			if (HasChance (hole_chance) && holes_in_order < max_holes_in_order) {
				item_created.GetComponent<BoxCollider2D> ().isTrigger = true;
				item_created.GetComponent<BoxCollider2D> ().offset = new Vector3(0, -0.5f);
				item_created.GetComponent<Renderer> ().enabled = false;
				item_created.name = "Hole";
				item_created.tag = "Hole";
				holes_in_order++;
			} else {
				if (HasChance (obstacle_chance) && obstacles_in_order < max_obstacles_in_order && holes_in_order == 0)
					CreateObstacle ();
				else if (HasChance (coin_chance) && coins_in_order < max_coins_in_order) {
					CreateCoin ();
					obstacles_in_order = 0;
				} else if (HasChance (coil_chance) && coils_in_order < max_coils_in_order) {
					CreateCoil ();
					coins_in_order = 0;
				} else
					coils_in_order = 0;
				holes_in_order = 0;
			}
		}
	}

	public void CreateObstacle() {
		GameObject obstacle_created = Instantiate(
			obstacles_prefabs[Random.Range(0, obstacles_prefabs.Length - 1)],
			last_item.transform.position + on_ground_offset,
			Quaternion.identity,
			last_item.transform
		);
		obstacle_created.tag = "Obstacle";
		obstacles_in_order++;
	}

	public void CreateCoin() {
		for (int i = 0; i < Random.Range (1, 4); i++) {
			GameObject coin_created = Instantiate (
             	coin_prefab,
				last_item.transform.position + Vector3.up * i + on_ground_offset,
				Quaternion.identity,
				last_item.transform
          	);
			coin_created.tag = "Coin";
		}
		coins_in_order++;
	}

	public void CreateCoil() {
		GameObject coil_created = Instantiate(
			coil_prefab,
			last_item.transform.position + on_ground_offset,
			Quaternion.identity,
			last_item.transform
		);
		coil_created.tag = "Coil";
		coils_in_order++;
	}

}
