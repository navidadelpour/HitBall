using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour {

	public static PlayerCollisionHandler instance;

	void Init() {
		instance = this;
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
		if (other.gameObject.tag == "Obstacle") {
			if (GameManager.instance.has_shield)
				return;
			else
				GameManager.instance.game_over = true;
		} else if (other.gameObject.tag == "Coin") {
			Destroy (other.gameObject);
			GameManager.instance.coins++;
		}
	}
}
