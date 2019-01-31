﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	public static SpawnManager self;

	private GameObject coin_prefab;
	private GameObject coil_prefab;
	private GameObject ground_prefab;
	private GameObject block_prefab;
	private GameObject portal_prefab;
	private GameObject arrow_prefab;
	private GameObject item_prefab;
	private Sprite[] item_textures;
	private GameObject[] obstacles_prefabs;

	private GameObject grounds;
	private GameObject last_ground;
	private float ground_limit_scale = 1.5f;
	private int ground_limit;
	private float ground_size_y;
	private float ground_size_x;

	private Things last_item_spawned;
	
	private Dictionary<System.Enum, int> chances = new Dictionary<System.Enum, int>() {
		{Things.COIL, 1},
		{Things.COIN, 2},
		{Things.BLOCK, 1},
		{Things.OBSTACLE, 3},
		{Things.HOLE, 3},
		{Things.NOTHING, 1},
	};
	private int item_chance = 10;
	private int arrow_chance = 5;
	private int[] coins_range = {1, 3};
	private int[] obstacles_range = {1, 3};
	public bool has_portal;

	void Awake() {
		self = this;

		ground_prefab = Resources.Load <GameObject>("prefabs/Ground");
		coin_prefab = Resources.Load <GameObject>("prefabs/Coin");
		coil_prefab = Resources.Load <GameObject>("prefabs/Coil");
		block_prefab = Resources.Load <GameObject>("prefabs/Block");
		portal_prefab = Resources.Load<GameObject>("prefabs/Portal");
		item_prefab =  Resources.Load<GameObject>("prefabs/Item");
		arrow_prefab =  Resources.Load<GameObject>("prefabs/Arrow");
		obstacles_prefabs = Resources.LoadAll <GameObject>("prefabs/Obstacles");
		item_textures = Resources.LoadAll <Sprite>("textures/Items");

		grounds = GameObject.Find ("Grounds");
		
		ground_size_y = ground_prefab.GetComponent<BoxCollider2D> ().size.y * ground_prefab.transform.lossyScale.y;
		ground_size_x = ground_prefab.GetComponent<BoxCollider2D> ().size.x * ground_prefab.transform.lossyScale.x;
		last_ground = grounds.transform.GetChild(0).gameObject;
	}

	void Start () {
		SetGroundLimit();
		for(int i = 1; i < ground_limit; i++)
			CreateGround ();
	}

	private void Update() {
		SetGroundLimit();
		
		// size - initial_size(5) + defautlsize(1)
		chances[Things.NOTHING] = (int) Camera.main.orthographicSize - 5 + 1;
		chances[Things.HOLE] = GameManager.self.safe_mode ? 0 : 3;
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
		switch(
			(ItemManager.self.actives[Item.TELEPORT] || ItemManager.self.actives[Item.GROUND_DIGGER]) && !has_portal ?
			Things.PORTAL :
			ShouldSpawn((Things)Util.GetKeyByChance(chances))
		) {
			case Things.HOLE:
				CreateHole ();
				break;
			case Things.OBSTACLE:
				CreateObstacle ();
				break;
			case Things.BLOCK:
				CreateBlock ();
				break;
			case Things.COIL:
				CreateCoil ();
				break;
			case Things.COIN:
				CreateCoin ();
				break;
			case Things.PORTAL:
				CreatePortal ();
				break;
			case Things.NOTHING:
				CreateNothing();
				break;
		}
		if(Util.HasChance(item_chance * (SpecialAbilityManager.self.Has(SpecialAbility.LUCKY) ? 2 : 1)))
			CreateItem();
		// if(Util.HasChance(arrow_chance))
		// 	CreateArrow();

		if(grounds.transform.childCount < ground_limit)
			Spawn();
	}

	private Things ShouldSpawn(Things item_to_spawn) {
		bool b = false;
		switch(item_to_spawn) {
			case Things.HOLE:
			case Things.OBSTACLE:
			case Things.BLOCK:
				b = last_item_spawned == Things.HOLE ||
				last_item_spawned == Things.OBSTACLE ||
				last_item_spawned == Things.BLOCK;
				break;
			case Things.COIL:
				b = last_item_spawned == Things.COIL;
				break;
			default:
				b = false;
				break;
		}
		return b ? Things.NOTHING : item_to_spawn;
	}

	public void CreateNothing() {
		last_item_spawned = Things.NOTHING;
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
	}

	private void CreateHole() {
		last_ground.GetComponent<BoxCollider2D> ().isTrigger = true;
		last_ground.GetComponent<BoxCollider2D> ().offset = new Vector3 (0, -0.5f);
		last_ground.GetComponent<Renderer> ().enabled = false;
		last_ground.name = "Hole";
		last_ground.tag = "Hole";
		last_item_spawned = Things.HOLE;
	}

	private void CreateObstacle() {
		GameObject obstacles_prefab = obstacles_prefabs[Random.Range(0, obstacles_prefabs.Length)];
		int max_i = 1;
		float starting_point = 0;
		float offset_x = 0;
		float offset_y = 1;

		if(obstacles_prefab.name == "Obstacle") {
			offset_x = obstacles_prefab.GetComponent<BoxCollider2D> ().size.x * obstacles_prefab.transform.lossyScale.x;
			offset_y = obstacles_prefab.GetComponent<BoxCollider2D> ().size.y * obstacles_prefab.transform.lossyScale.y;
			max_i = Random.Range (obstacles_range[0], obstacles_range[1] + 1);
			starting_point = (1 - max_i) * .5f;
		}
		for (int i = 0; i < max_i; i++) {
			Instantiate(
				obstacles_prefab,
				last_ground.transform.position + Vector3.right * starting_point + Vector3.up * (ground_size_y / 2 + offset_y / 2) + Vector3.right * offset_x * i,
				Quaternion.identity,
				last_ground.transform
			);
			last_item_spawned = Things.OBSTACLE;
		}
	}
		
	private void CreateBlock() {
		Instantiate(
			block_prefab,
			last_ground.transform.position + Vector3.up * ground_size_y / 2 + Vector3.up * Random.Range(2f, 5f),
			Quaternion.identity
		);
		last_item_spawned = Things.BLOCK;
	}


	private void CreateCoin() {
		for (int i = 0; i < Random.Range (coins_range[0], coins_range[1] + 1); i++) {
			GameObject coin_created = Instantiate (
             	coin_prefab,
				last_ground.transform.position + Vector3.up * i + Vector3.up *( ground_size_y / 2 + 1),
				Quaternion.identity,
				last_ground.transform
          	);
			if(ItemManager.self.actives[Item.MAGNET])
				coin_created.AddComponent<CoinMovement>();
			last_item_spawned = Things.COIN;
		}
	}

	private void CreateCoil() {
		float offset_y = coil_prefab.GetComponent<BoxCollider2D> ().size.y * coil_prefab.transform.lossyScale.y;
		Instantiate(
			coil_prefab,
			last_ground.transform.position + Vector3.up * (ground_size_y / 2 + offset_y / 2),
			Quaternion.identity,
			last_ground.transform
		);
		last_item_spawned = Things.COIL;
	}

	private void CreatePortal() {
		GameObject portal_created = Instantiate(
			portal_prefab,
			last_ground.transform.position + Vector3.up * ground_size_y / 2,
			Quaternion.identity,
			last_ground.transform
		);
		if(ItemManager.self.actives[Item.GROUND_DIGGER])
			portal_created.GetComponent<Renderer> ().enabled = false;
		last_item_spawned = Things.PORTAL;
		has_portal = true;
	}

	private void CreateArrow() {
		Instantiate(
			arrow_prefab,
			last_ground.transform.position + Vector3.up * ground_size_y / 2,
			Quaternion.identity,
			last_ground.transform
		);
	}

	private void CreateItem() {
		GameObject item_created = Instantiate(
			item_prefab,
			last_ground.transform.position + Vector3.up * ground_size_y / 2 + Vector3.up * Random.Range(3f, 5f),
			Quaternion.identity,
			last_ground.transform
		);
		Sprite item_texture;
		do {
			item_texture = item_textures[Random.Range(0, item_textures.Length)];
		} while (item_texture.name == "nothing");
		item_created.GetComponent<SpriteRenderer>().sprite = item_texture;
		item_created.name = item_texture.name;
	}
}
