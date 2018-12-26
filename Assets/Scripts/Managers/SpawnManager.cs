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
	private GameObject last_ground;
	private SpawnState last_item_spawned;
	private int gap_chance;
	private Dictionary<SpawnState, int> chances = new Dictionary<SpawnState, int>() {
		{SpawnState.COIL, 1},
		{SpawnState.COIN, 2},
		{SpawnState.BLOCK, 2},
		{SpawnState.OBSTACLE, 3},
		{SpawnState.HOLE, 3},
		{SpawnState.NOTHING, 1},
	};

	private int grounds_in_row;
	private int min_distance_between_items = 1;

	private int min_coins = 1;
	private int max_coins = 3;

	private float ground_limit_scale = 1.5f;
	private int ground_limit;
	private Vector3 on_ground_offset;
	private float ground_size_x;

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
		
		on_ground_offset = Vector3.up * ground_prefab.GetComponent<BoxCollider2D> ().size.y * ground_prefab.transform.lossyScale.y;
		ground_size_x = ground_prefab.GetComponent<BoxCollider2D> ().size.x * grounds.transform.GetChild(0).transform.lossyScale.x;
		last_ground = grounds.transform.GetChild(0).gameObject;
	}

	void Start () {
		Init ();
		SetGroundLimit();
		for(int i = 1; i < ground_limit; i++)
			CreateGround ();
	}

	private void Update() {
		SetGroundLimit();
		
		// size - initial_size(5) + defautlsize(1)
		chances[SpawnState.NOTHING] = (int) Camera.main.orthographicSize - 4;
	}

	private void SetGroundLimit() {
		ground_limit = (int) (ground_limit_scale * Mathf.Ceil(
			(Camera.main.orthographicSize * 2 * Screen.width / Screen.height) / 
			ground_size_x
		));
	}

	public void Spawn() {
		if(grounds.transform.childCount > ground_limit)
			return;

		CreateGround ();
		switch(ShouldSpawn(Util.GetKeyByChance(chances))) {
			case SpawnState.HOLE:
				CreateHole ();
				break;
			case SpawnState.OBSTACLE:
				CreateObstacle ();
				break;
			case SpawnState.BLOCK:
				CreateBlock ();
				break;
			case SpawnState.COIL:
				CreateCoil ();
				break;
			case SpawnState.COIN:
				CreateCoin ();
				break;
			case SpawnState.NOTHING:
				grounds_in_row ++;
				last_item_spawned = SpawnState.NOTHING;
				break;
		}

		if(grounds.transform.childCount < ground_limit)
			Spawn();
	}

	private SpawnState ShouldSpawn(SpawnState item_to_spawn) {
		bool b = false;
		switch(item_to_spawn) {
			case SpawnState.HOLE:
			case SpawnState.OBSTACLE:
			case SpawnState.BLOCK:
				b = last_item_spawned == SpawnState.HOLE ||
				last_item_spawned == SpawnState.OBSTACLE ||
				last_item_spawned == SpawnState.BLOCK;
				break;
			default:
				b = false;
				break;
		}
		return b ? SpawnState.NOTHING : item_to_spawn;
	}

	private void CreateGround() {
		last_ground = grounds.transform.GetChild (grounds.transform.childCount - 1).gameObject;
		GameObject item_created = Instantiate (
			ground_prefab,
			last_ground.transform.position + Vector3.right * last_ground.GetComponent<BoxCollider2D> ().size.x * last_ground.transform.lossyScale.x,
			Quaternion.identity,
			grounds.transform
		);
		last_ground = item_created;
		item_created.name = "Ground";
		item_created.tag = "Ground";
	}

	private void CreateHole() {
		last_ground.GetComponent<BoxCollider2D> ().isTrigger = true;
		last_ground.GetComponent<BoxCollider2D> ().offset = new Vector3 (0, -0.5f);
		last_ground.GetComponent<Renderer> ().enabled = false;
		last_ground.name = "Hole";
		last_ground.tag = "Hole";
		last_item_spawned = SpawnState.HOLE;
	}

	private void CreateObstacle() {
		GameObject obstacle_created = Instantiate(
			obstacles_prefabs[Random.Range(0, obstacles_prefabs.Length - 1)],
			last_ground.transform.position + on_ground_offset,
			Quaternion.identity,
			last_ground.transform
		);
		obstacle_created.tag = "Obstacle";
		last_item_spawned = SpawnState.OBSTACLE;
	}
		
	private void CreateBlock() {
		GameObject block_created = Instantiate(
			block_prefab,
			last_ground.transform.position + on_ground_offset + Vector3.up * Random.Range(1f, 6f),
			Quaternion.identity,
			last_ground.transform
		);
		block_created.GetComponent<SpriteRenderer> ().color = Random.Range (1, 3) % 2 == 0 ? Color.red : Color.blue;
		block_created.tag = "Block";
		last_item_spawned = SpawnState.BLOCK;
	}


	private void CreateCoin() {
		for (int i = 0; i < Random.Range (min_coins, max_coins + 1); i++) {
			GameObject coin_created = Instantiate (
             	coin_prefab,
				last_ground.transform.position + Vector3.up * i + on_ground_offset,
				Quaternion.identity,
				last_ground.transform
          	);
			coin_created.tag = "Coin";
		last_item_spawned = SpawnState.COIN;
		}
	}

	private void CreateCoil() {
		GameObject coil_created = Instantiate(
			coil_prefab,
			last_ground.transform.position + on_ground_offset,
			Quaternion.identity,
			last_ground.transform
		);
		coil_created.tag = "Coil";
		last_item_spawned = SpawnState.COIL;
	}

}
