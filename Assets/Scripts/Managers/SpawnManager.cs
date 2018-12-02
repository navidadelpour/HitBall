using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	public static SpawnManager instance;

	public GameObject ground;

	public bool should_create_ground;

	void Init() {
		should_create_ground = true;
		instance = this;
	}

	// Use this for initialization
	void Start () {
		Init ();
	}
	
	// Update is called once per frame
	void Update () {
		if (should_create_ground)
			CreateGround ();
	}

	void CreateGround() {
		GameObject ground = GameObject.Find ("Ground");
		Vector3 position_to_create = ground.transform.position + Vector3.right * ground.GetComponent<BoxCollider2D>().size.x * ground.transform.localScale.x;
		GameObject ground_created = Instantiate(ground, position_to_create, Quaternion.identity, GameObject.Find("Grounds").transform);
		ground_created.name = "Ground";
		ground_created.tag = "Ground";
		should_create_ground = false;
	}
}
