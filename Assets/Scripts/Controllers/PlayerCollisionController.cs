using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionController : MonoBehaviour {

	public static PlayerCollisionController self;
	private bool is_collided;

	void Awake() {
		self = this;
	}

	void Start () {
		InvokeRepeating("CheckForCollision", .1f, .1f);
	}
	
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D other) {
		if (!is_collided && other.gameObject.tag == "Ground") {
			ItemManager.self.actives[Item.WINGS] = false;
			ItemManager.self.actives[Item.WEB] = false;
			HeightManager.self.SetHeight ();
			PlayerMovement.self.Jump ();
			ParticleManager.self.Spawn("dust", this.transform.position);
			AudioManager.self.Play("player_jump");
			ScreenShake.self.Shake(.15f);
			if(ItemManager.self.actives[Item.HIGH_JUMP]) {
				HeightManager.self.has_coil = true;
				HeightManager.self.should_remove_coil = false;
				ItemManager.self.actives[Item.HIGH_JUMP] = false;
			}
			if(ItemManager.self.actives[Item.GROUND_DIGGER]) {
				transform.position += Vector3.down * 10f;
				PlayerMovement.self.enabled = false;
			}
			is_collided = true;
		}
	}

	void CheckForCollision() {
		is_collided = false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		switch (other.gameObject.tag) {
			case "Block":
				if(SpecialAbilitiesManager.self.Has(SpecialAbilities.ENEMY_EARNER)) {
					Destroy(other.gameObject);
					GameManager.self.EnemyEarn();
				}
				break;
			// case "Arrow":
			case "Obstacle":
				if(!GameManager.self.safe_mode) {
					if(ItemManager.self.actives[Item.SHIELD])
						ItemManager.self.actives[Item.SHIELD] = false;
					else
						GameManager.self.GameOver();
				}
				AudioManager.self.Play("player_die");
				ScreenShake.self.Shake(.3f);
				break;

			case "Coin":
				Destroy (other.gameObject);
				GameManager.self.IncreamentCoins();
				AudioManager.self.Play("coin");
				break;

			case "Hole":
				GameManager.self.GameOver();
				break;

			case "Coil":
				HeightManager.self.SetHeight ();
				PlayerMovement.self.Jump ();
				HeightManager.self.has_coil = true;
				HeightManager.self.should_remove_coil = false;
				AudioManager.self.Play("coil");
				break;
			case "Item":
				Destroy (other.gameObject);
				Item item = (Item)System.Enum.Parse(typeof(Item), other.name.ToUpper());
				ItemManager.self.AddItem(item);
        		AudioManager.self.Play("item_get");
				break;
		}
	}
}
