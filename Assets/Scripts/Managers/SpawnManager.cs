using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	public static SpawnManager self;

	private GameObject coin_prefab;
	private GameObject coil_prefab;
	private GameObject ground_prefab;
	private GameObject block_prefab;
	private GameObject[] obstacles_prefabs;
	private GameObject grounds;
	private GameObject last_item;

	private int coin_chance = 10;
	private int coil_chance = 10;
	private int hole_chance = 30;
	private int block_chance = 10;
	private int obstacle_chance = 30;

	private int coins_in_order;
	private int coils_in_order;
	private int holes_in_order;
	private int blocks_in_order;
	private int obstacles_in_order;
	private int grounds_in_order;

	private int max_coins_in_order = 1;
	private int max_coils_in_order = 2;
	private int max_holes_in_order = 2;
	private int max_blocks_in_order = 2;
	private int max_obstacles_in_order = 2;
	private int max_grounds_in_order = 3;

	private int min_coins = 1;
	private int max_coins = 3;

	private float ground_limit_scale = 1.5f;
	private int ground_limit;
	private Vector3 on_ground_offset;

	void Awake() {
		self = this;
	}

	void Init() {
		ground_prefab = Resources.Load <GameObject>("prefabs/Ground");
		coin_prefab = Resources.Load <GameObject>("prefabs/Coin");
		coil_prefab = Resources.Load <GameObject>("prefabs/Coil");
		block_prefab = Resources.Load <GameObject>("prefabs/Block");
		obstacles_prefabs = Resources.LoadAll <GameObject>("prefabs/Obstacles");

		grounds = GameObject.Find ("Grounds");
		last_item = GameObject.Find("Ground");
	}

	void Start () {
		Init ();
		SetGroundLimit();
		for(int i = 1; i < ground_limit; i++)
			CreateGround ();
		on_ground_offset = Vector3.up * last_item.GetComponent<BoxCollider2D> ().size.y;
		InvokeRepeating("x", 1f, .1f);
	}

	void Update() {
		SetGroundLimit();
	}

	void x() {
		Debug.Log(ground_limit);
		if(grounds.transform.childCount < ground_limit)
			CreateGround();
		else if(grounds.transform.childCount > ground_limit)
			Destroy(grounds.transform.GetChild(grounds.transform.childCount - 1).gameObject);
	}

	public bool HasChance(int chance) {
		return Random.Range (0, 100) < chance;
	}

	private void SetGroundLimit() {
		ground_limit = (int) (ground_limit_scale * Mathf.Ceil(
			(Screen.width / (Camera.main.orthographicSize * 10)) /
			(grounds.transform.GetChild(0).GetComponent<BoxCollider2D> ().size.x * grounds.transform.GetChild(0).transform.lossyScale.x)
		));
	}

	public void Spawn() {
		CreateGround ();
		if (Conditions("Hole")) {
			CreateHole ();
		} else if (Conditions("Obstacle")) {
			CreateObstacle ();
		} else if (Conditions("Block")) {
			CreateBlock ();
		} else if (Conditions("Coil")) {
			CreateCoil ();
		} else if (Conditions("Coin")) {
			CreateCoin ();
		} else {
			ZeroAllExcept (ref obstacles_in_order);
		}
	}

	public bool HasChance(int chance) {
		return Random.Range (0, 100) < chance;
	}

	private void SetGroundLimit() {
		ground_limit = (int) Mathf.Ceil(
			(Screen.width / (Camera.main.orthographicSize * 10)) /
			(grounds.transform.GetChild(0).GetComponent<BoxCollider2D> ().size.x * grounds.transform.GetChild(0).transform.lossyScale.x)
		);
	}

	public void ZeroAllExcept(ref int var_name) {
		int previous_value = var_name;
		coils_in_order = 0;
		obstacles_in_order = 0;
		coins_in_order = 0;
		blocks_in_order = 0;
		grounds_in_order = 0;
		holes_in_order = 0;
		var_name = previous_value + 1;
	}

	public bool Conditions(string name) {
		bool return_value = false;
		switch (name) {
		case "Hole":
			return_value = HasChance (hole_chance)
			&& holes_in_order < max_holes_in_order
			&& obstacles_in_order == 0;
			break;
		case "Obstacle":
			return_value = (HasChance (obstacle_chance) || grounds_in_order > max_grounds_in_order)
			&& obstacles_in_order < max_obstacles_in_order
			&& holes_in_order == 0;
			break;
		case "Block":
			return_value = HasChance (block_chance)
			&& blocks_in_order < max_blocks_in_order
			&& holes_in_order == 0;
			break;
		case "Coil":
			return_value = HasChance (coil_chance)
			&& coils_in_order < max_coils_in_order;
			break;
		case "Coin":
			return_value = HasChance (coin_chance)
			&& coins_in_order < max_coins_in_order;
			break;
		}
		return return_value;
	}

	public void CreateGround() {
		last_item = grounds.transform.GetChild (grounds.transform.childCount - 1).gameObject;
		GameObject item_created = Instantiate (
			ground_prefab,
			last_item.transform.position + Vector3.right * last_item.GetComponent<BoxCollider2D> ().size.x * last_item.transform.lossyScale.x,
			Quaternion.identity,
			grounds.transform
		);
		item_created.name = "Ground";
		item_created.tag = "Ground";
		last_item = item_created;
	}

	public void CreateHole() {
		last_item.GetComponent<BoxCollider2D> ().isTrigger = true;
		last_item.GetComponent<BoxCollider2D> ().offset = new Vector3 (0, -0.5f);
		last_item.GetComponent<Renderer> ().enabled = false;
		last_item.name = "Hole";
		last_item.tag = "Hole";
		ZeroAllExcept (ref holes_in_order);
	}

	public void CreateObstacle() {
		GameObject obstacle_created = Instantiate(
			obstacles_prefabs[Random.Range(0, obstacles_prefabs.Length - 1)],
			last_item.transform.position + on_ground_offset,
			Quaternion.identity,
			last_item.transform
		);
		obstacle_created.tag = "Obstacle";
		ZeroAllExcept (ref obstacles_in_order);
	}
		
	public void CreateBlock() {
		GameObject block_created = Instantiate(
			block_prefab,
			last_item.transform.position + on_ground_offset + Vector3.up * Random.Range(1f, 6f),
			Quaternion.identity,
			last_item.transform
		);
		block_created.GetComponent<SpriteRenderer> ().color = Random.Range (1, 3) % 2 == 0 ? Color.red : Color.blue;
		block_created.tag = "Block";
		ZeroAllExcept (ref blocks_in_order);
	}


	public void CreateCoin() {
		for (int i = 0; i < Random.Range (min_coins, max_coins + 1); i++) {
			GameObject coin_created = Instantiate (
             	coin_prefab,
				last_item.transform.position + Vector3.up * i + on_ground_offset,
				Quaternion.identity,
				last_item.transform
          	);
			coin_created.tag = "Coin";
		}
		ZeroAllExcept (ref coins_in_order);
	}

	public void CreateCoil() {
		GameObject coil_created = Instantiate(
			coil_prefab,
			last_item.transform.position + on_ground_offset,
			Quaternion.identity,
			last_item.transform
		);
		coil_created.tag = "Coil";
		ZeroAllExcept (ref coils_in_order);
	}

}
