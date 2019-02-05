using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {
    public static ItemManager self;

    public AvailableItem[] available_items;
    public int available_items_size;
	
	public Dictionary<Item, bool> actives = new Dictionary<Item, bool>();
	private float max_time = 5f;

    void Awake() {
        self = this;
    }

    void Start() {
        available_items_size = LevelManager.self.item_slots_unlocks;
        available_items = new AvailableItem[available_items_size];
        for(int i = 0; i < available_items_size; i++)
            available_items[i] = new AvailableItem(Item.NOTHING);

		foreach (Item item in (Item[]) Enum.GetValues(typeof(Item)))
			actives.Add(item, false);

    }

    void Update() {
        if(SpecialAbilitiesManager.self.Has(SpecialAbilities.RANDOMER)) {
            Array items = Enum.GetValues(typeof(Item));
            foreach(AvailableItem available_item in available_items) {
                if(available_item.item == Item.NOTHING) {
                    AddItem((Item) items.GetValue(UnityEngine.Random.Range(0, items.Length - 1)));
                }
            }
        }
    }

	public void ActiveItem(int index) {
		StartCoroutine(SetActive(available_items[index].item));

		if(actives[Item.MAGNET]) {
			GameObject[] coins_in_scene = GameObject.FindGameObjectsWithTag("Coin");
			foreach (GameObject coin in coins_in_scene)
				coin.AddComponent<CoinMovement>();
		}
		
		if(actives[Item.DOUBLE_JUMP]) {
			if(PlayerMovement.self.jumping){
				actives[Item.DOUBLE_JUMP] = false;
				return;
			}
		}

		if(actives[Item.FORCE_FALL]) {
			if(!PlayerMovement.self.jumping){
				actives[Item.FORCE_FALL] = false;
				return;
			}
		}

		RemoveItem(index);

        if(SpecialAbilitiesManager.self.Has(SpecialAbilities.RANDOMER)) {
            Array items = Enum.GetValues(typeof(Item));
            AddItem((Item) items.GetValue(UnityEngine.Random.Range(0, items.Length - 1)));
        }
	}

	IEnumerator SetActive(Item item) {
		actives[item] = true;
		yield return new WaitForSeconds(max_time);
		actives[item] = false;
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

	public void RemoveItem(int index) {
        available_items[index] = new AvailableItem(Item.NOTHING);
		UiManager.self.SetItem(index, Item.NOTHING);
	}

}