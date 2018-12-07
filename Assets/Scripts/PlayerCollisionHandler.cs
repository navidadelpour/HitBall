using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour {

	public static PlayerCollisionHandler instance;

	void Awake() {
		instance = this;
	}

	void Init() {

	}

	void Start () {
		Init ();
	}
	
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Ground") {
			PlayerMovement.instance.Jump ();
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		switch (other.gameObject.tag) {

		case "Obstacle":
			GameManager.instance.game_over = !GameManager.instance.has_shield;
			break;

		case "Coin":
			Destroy (other.gameObject);
			GameManager.instance.IncreamentCoins();
			break;

		case "Hole":
			GameManager.instance.game_over = true;
			break;

		case "Coil":
			GameManager.instance.has_coil = true;
			GameManager.instance.should_remove_coil = false;
			break;
		}
	}
}
