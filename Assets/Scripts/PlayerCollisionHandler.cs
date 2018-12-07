using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour {

	public static PlayerCollisionHandler self;

	void Awake() {
		self = this;
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
			PlayerMovement.self.Jump ();
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		switch (other.gameObject.tag) {

		case "Obstacle":
			GameManager.self.game_over = !GameManager.self.has_shield;
			break;

		case "Coin":
			Destroy (other.gameObject);
			GameManager.self.IncreamentCoins();
			break;

		case "Hole":
			GameManager.self.game_over = true;
			break;

		case "Coil":
			GameManager.self.has_coil = true;
			GameManager.self.should_remove_coil = false;
			break;
		}
	}
}
