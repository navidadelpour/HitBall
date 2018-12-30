using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {
    public static ItemManager self;

	public bool item_activated;
	public Item item;

	public bool has_shield;
	private float shield_adding_time;
	private float max_shield_time = 3f;

	public bool has_magnet;
	private float magnet_adding_time;
	private float max_magnet_time = 3f;

	public bool has_slow_motion;
	private float slow_motion_adding_time;
	private float max_slow_motion_time = 3f;

	public bool has_zoom;
	private float zoom_adding_time;
	private float max_zoom_time = 3f;

	public bool has_teleport;
	public bool has_high_jump;
	public bool has_double_jump;
	public bool has_force_fall;

    void Awake() {
        self = this;
    }

    void Start() {
		RemoveItem();
    }

    void Update() {
        if(item_activated){
			switch (item) {
				case Item.SHIELD:
					RemoveItem();
					shield_adding_time = Time.time;
					has_shield = true;
					break;
				case Item.MAGNET:
					RemoveItem();
					magnet_adding_time = Time.time;
					has_magnet = true;
					GameObject[] coins_in_scene = GameObject.FindGameObjectsWithTag("Coin");
					foreach (GameObject coin in coins_in_scene)
						coin.AddComponent<CoinMovement>();
					break;
				case Item.SLOW_MOTION:
					RemoveItem();
					slow_motion_adding_time = Time.time;
					has_slow_motion = true;
					break;
				case Item.ZOOM:
					RemoveItem();
					zoom_adding_time = Time.time;
					has_zoom = true;
					break;
				case Item.TELEPORT:
					RemoveItem();
					has_teleport = true;
					break;
				case Item.HIGH_JUMP:
					RemoveItem();
					has_high_jump = true;
					break;
				case Item.DOUBLE_JUMP:
					if(!PlayerMovement.self.jumping){
						RemoveItem();
						has_double_jump = true;
					} else
						item_activated = false;
					break;
				case Item.FORCE_FALL:
					if(PlayerMovement.self.jumping){
						RemoveItem();
						has_force_fall = true;
					} else
						item_activated = false;
					break;

			}
		}
		has_shield &= Time.time - shield_adding_time < max_shield_time;
		has_magnet &= Time.time - magnet_adding_time < max_magnet_time;
		has_slow_motion &= Time.time - slow_motion_adding_time < max_slow_motion_time;
		has_zoom &= Time.time - zoom_adding_time < max_zoom_time;

    }

    public void SetItem(Item item) {
		this.item = item;
		UiManager.self.SetItem();
	}

	public void RemoveItem() {
		item = Item.NOTHING;
		item_activated = false;
		UiManager.self.SetItem();
	}

}