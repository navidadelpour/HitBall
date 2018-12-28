using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour {

	public static PlayerCollisionHandler self;
	private bool is_collided;
	public int collided = 0;
	void Awake() {
		self = this;
	}

	void Init() {

	}

	void Start () {
		Init ();
		InvokeRepeating("CheckForCollision", .1f, .1f);
	}
	
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D other) {
		if(!is_collided)
			if (other.gameObject.tag == "Ground") {
				HeightManager.self.SetHeight ();
				PlayerMovement.self.Jump ();
				if(GameManager.self.item_activated && GameManager.self.item == Item.HIGH_JUMP) {
					HeightManager.self.has_coil = true;
					HeightManager.self.should_remove_coil = false;
					GameManager.self.RemoveItem();
				}
				collided += 1;
				is_collided = true;
			}
	}

	void CheckForCollision() {
		is_collided = false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		switch (other.gameObject.tag) {
			case "Block":
			case "Obstacle":
				// GameManager.self.game_over = !GameManager.self.has_shield;
				break;

			case "Coin":
				Destroy (other.gameObject);
				GameManager.self.IncreamentCoins();
				break;

			case "Hole":
				GameManager.self.game_over = true;
				break;

			case "Coil":
				HeightManager.self.SetHeight ();
				PlayerMovement.self.Jump ();
				HeightManager.self.has_coil = true;
				HeightManager.self.should_remove_coil = false;
				break;
			case "Item":
				Destroy (other.gameObject);
				Item item = (Item)System.Enum.Parse(typeof(Item), other.name.ToUpper());
				GameManager.self.SetItem(item);
				break;
		}
	}
}
