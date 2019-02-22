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
		if(!is_collided)
			if (other.gameObject.tag == "Ground") {
				ItemManager.self.actives[Items.WINGS] = false;
				ItemManager.self.actives[Items.WEB] = false;
				HeightManager.self.SetHeight ();
				PlayerMovement.self.Jump ();
				if(!GameManager.self.gameover){
					ParticleManager.self.Spawn("Dust", this.transform.position);
					if(GameManager.self.on_player_views)
						AudioManager.self.Play("ground");
					ScreenShake.self.Shake(.125f);
				}
				if(ItemManager.self.actives[Items.HIGH_JUMP]) {
					HeightManager.self.has_coil = true;
					HeightManager.self.should_remove_coil = false;
					ItemManager.self.actives[Items.HIGH_JUMP] = false;
				}
				if(ItemManager.self.actives[Items.GROUND_DIGGER]) {
					transform.position += Vector3.down * 10f;
					PlayerMovement.self.enabled = false;
				}
				is_collided = true;
			} else if(other.gameObject.tag == "Block") {
				if(SpecialAbilitiesManager.self.Has(SpecialAbilities.ENEMY_EARNER)) {
					Destroy(other.gameObject);
					GameManager.self.EnemyEarn(other.transform.position);
				}
			}
	}

	void CheckForCollision() {
		is_collided = false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(GameManager.self.gameover)
			return;
		switch (other.gameObject.tag) {
			// case "Arrow":
			case "Obstacle":
				if(!GameManager.self.gameover) {
					AudioManager.self.Play("obsacle");
					ScreenShake.self.Shake(.2f);
				}
				if(!GameManager.self.safe_mode) {
					if(!ItemManager.self.actives[Items.SHIELD]) {
						GameManager.self.gameover = true;
						GameManager.self.SetHighScore();
						UiManager.self.GameOver();
					}
				}
				break;

			case "Coin":
				Destroy (other.gameObject);
				GameManager.self.IncreamentCoins();
				AudioManager.self.Play("coin");
				break;

			case "Hole":
				AudioManager.self.Play("hole");
				GameManager.self.gameover = true;
				GameManager.self.SetHighScore();
				UiManager.self.GameOver();
				PlayerMovement.self.should_fall = true;
				PlayerMovement.self.gameObject.GetComponent<Collider2D>().enabled = false;
				break;

			case "Coil":
				other.gameObject.GetComponent<Animator>().SetTrigger("Wiggle");
				AudioManager.self.Play("coil");
				StartCoroutine(JumpDelaied());
				break;
			case "Item":
				Destroy (other.gameObject);
				Items item = (Items)System.Enum.Parse(typeof(Items), other.name.ToUpper());
				ItemManager.self.AddItem(item);
        		AudioManager.self.Play("item_get");
				break;
			case "Ammo":
				Destroy(other.gameObject);
				GunController.self.AddCartridge();
				break;

		}
	}

	IEnumerator JumpDelaied() {
		yield return new WaitForSeconds(.05f);
		HeightManager.self.SetHeight ();
		PlayerMovement.self.Jump ();
		HeightManager.self.has_coil = true;
		HeightManager.self.should_remove_coil = false;
	}
}
