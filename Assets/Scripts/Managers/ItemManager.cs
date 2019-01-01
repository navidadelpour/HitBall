using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {
    public static ItemManager self;

	public bool item_activated;
    public int item_activated_index;

    public AvailableItem[] available_items;
    public int available_items_size = 3;
	private Item[] items;
	private Dictionary<Item, float> started_times = new Dictionary<Item, float>();
	public Dictionary<Item, bool> actives = new Dictionary<Item, bool>();

    void Awake() {
        self = this;
        available_items = new AvailableItem[available_items_size];
        for(int i = 0; i < available_items_size; i++)
            available_items[i] = new AvailableItem(Item.NOTHING);

		items = (Item[]) Enum.GetValues(typeof(Item));
		foreach (Item item in items){
			started_times.Add(item, 0f);
			actives.Add(item, false);
		}
    }

	public 

    void Start() {
		RemoveItem();
    }

    void Update() {
        if(item_activated){
			Item item = available_items[item_activated_index].item;
			started_times[item] = Time.time;
			actives[item] = true;

			if(actives[Item.MAGNET]) {
				GameObject[] coins_in_scene = GameObject.FindGameObjectsWithTag("Coin");
				foreach (GameObject coin in coins_in_scene)
					coin.AddComponent<CoinMovement>();
			}
			
			if(actives[Item.DOUBLE_JUMP]) {
				if(PlayerMovement.self.jumping){
					item_activated = false;
					actives[Item.DOUBLE_JUMP] = false;
				}
			}

			if(actives[Item.FORCE_FALL]) {
				if(!PlayerMovement.self.jumping){
					item_activated = false;
					actives[Item.FORCE_FALL] = false;
				}
			}

			if(item_activated)
				RemoveItem();	
		}
		foreach (Item item in items)
			actives[item] &= Time.time - started_times[item] < 3;
    }

    public void AddItem(Item item) {
        // checking if the item exists already
        for(int i = 0; i < available_items_size; i++) {
            if(available_items[i].item == item){
                available_items[i] = new AvailableItem(item);
                return;
            }
        }

        // adding to null slot
        for(int i = 0; i < available_items_size; i++) {
            if(available_items[i].item == Item.NOTHING) {
                available_items[i] = new AvailableItem(item);
		        UiManager.self.SetItem(i, item);
                return;
            }
        }

        // adding to the oldest slot
        AvailableItem old_item = null;
        for(int i = 0; i < available_items_size; i++) {
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