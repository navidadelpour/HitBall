using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	public static SpawnManager instance;

	public GameObject ground;


	void Init() {
		instance = this;
	}

	// Use this for initialization
	void Start () {
		Init ();
		CreateGround ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void CreateGround() {
		GameObject ground = GameObject.Find ("Ground");
		Vector3 position_to_create = ground.transform.position + Vector3.right * ground.GetComponent<BoxCollider2D>().size.x * ground.transform.localScale.x;
		GameObject ground_created = Instantiate(ground, position_to_create, Quaternion.identity, GameObject.Find("Grounds").transform);
		ground_created.name = "Ground";
		ground_created.tag = "Ground";
	}
}
