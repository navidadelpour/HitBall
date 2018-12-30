using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {
    public static ItemManager self;

	public bool item_activated;
    public int item_activated_index;

    public AvailableItem[] available_items;
    public int items_size = 3;

    public bool has_shield;
	public bool has_magnet;
	public bool has_slow_motion;
	public bool has_zoom;
	public bool has_teleport;
	public bool has_high_jump;
	public bool has_double_jump;
	public bool has_force_fall;


	private float shield_adding_time;
	private float magnet_adding_time;
	private float slow_motion_adding_time;
	private float zoom_adding_time;

	private float max_shield_time = 3f;
	private float max_magnet_time = 3f;
	private float max_slow_motion_time = 3f;
	private float max_zoom_time = 3f;

    void Awake() {
        self = this;
        available_items = new AvailableItem[items_size];
        for(int i = 0; i < items_size; i++)
            available_items[i] = new AvailableItem(Item.NOTHING);
    }

    void Start() {
		RemoveItem();
    }

    void Update() {
        Debug.Log(item_activated_index);
        if(item_activated){
			switch (available_items[item_activated_index].item) {
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

    public void AddItem(Item item) {
        // checking if the item exists already
        for(int i = 0; i < items_size; i++) {
            if(available_items[i].item == item){
                available_items[i] = new AvailableItem(item);
                return;
            }
        }

        // adding to null slot
        for(int i = 0; i < items_size; i++) {
            if(available_items[i].item == Item.NOTHING) {
                available_items[i] = new AvailableItem(item);
		        UiManager.self.SetItem(i, item);
                return;
            }
        }

        // adding to the oldest slot
        AvailableItem old_item = null;
        for(int i = 0; i < items_size; i++) {
            if(i == 0 || available_items[i].time_added < old_item.time_added)
                old_item = available_items[i];
        }
        int j = Array.IndexOf(available_items, old_item);
        available_items[j] = new AvailableItem(item);

        UiManager.self.SetItem(j, item);
	}

	public void RemoveItem() {
        available_items[item_activated_index] = new AvailableItem(Item.NOTHING);
		item_activated = false;
		UiManager.self.SetItem(item_activated_index, Item.NOTHING);
	}

}