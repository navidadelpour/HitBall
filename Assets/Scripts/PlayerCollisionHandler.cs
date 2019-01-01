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

	void Start () {
		InvokeRepeating("CheckForCollision", .1f, .1f);
	}
	
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D other) {
		if(!is_collided)
			if (other.gameObject.tag == "Ground") {
				HeightManager.self.SetHeight ();
				PlayerMovement.self.Jump ();
				if(ItemManager.self.actives[Item.HIGH_JUMP]) {
					HeightManager.self.has_coil = true;
					HeightManager.self.should_remove_coil = false;
					ItemManager.self.actives[Item.HIGH_JUMP] = false;
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
			// case "Block":
			case "Obstacle":
				if(!GameManager.self.safe_mode) {
					if(ItemManager.self.actives[Item.SHIELD])
						ItemManager.self.actives[Item.SHIELD] = false;
					else
						GameManager.self.GameOver();
				}
				break;

			case "Coin":
				Destroy (other.gameObject);
				GameManager.self.IncreamentCoins();
				break;

			case "Hole":
				GameManager.self.GameOver();
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
				ItemManager.self.AddItem(item);
				break;
		}
	}
}
