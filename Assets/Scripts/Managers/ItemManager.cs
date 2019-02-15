using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemManager : MonoBehaviour {
    public static ItemManager self;

    public AvailableItem[] available_items;
    public int available_items_size;
	
	public Dictionary<Items, bool> actives = new Dictionary<Items, bool>();
	private float max_time = 31f;
    Items[] untimed_items = new Items[] {Items.DOUBLE_JUMP, Items.FORCE_FALL, Items.HIGH_JUMP, Items.TELEPORT, Items.GROUND_DIGGER, Items.WEB};

    void Awake() {
        self = this;
    }

    void Start() {
        available_items_size = LevelManager.self.item_slots_unlocks;
        available_items = new AvailableItem[available_items_size];
        for(int i = 0; i < available_items_size; i++)
            available_items[i] = new AvailableItem(Items.NOTHING);

		foreach (Items item in (Items[]) Enum.GetValues(typeof(Items)))
			actives.Add(item, false);

    }

    void Update() {
        if(SpecialAbilitiesManager.self.Has(SpecialAbilities.RANDOMER)) {
            Array items = Enum.GetValues(typeof(Items));
            foreach(AvailableItem available_item in available_items) {
                if(available_item.item == Items.NOTHING) {
                    AddItem((Items) items.GetValue(UnityEngine.Random.Range(0, items.Length - 1)));
                }
            }
        }
    }

	public void ActiveItem(int index) {
        if(actives[available_items[index].item])
            return;
		actives[available_items[index].item] = true;

		if(actives[Items.MAGNET]) {
			GameObject[] coins_in_scene = GameObject.FindGameObjectsWithTag("Coin");
			foreach (GameObject coin in coins_in_scene)
				coin.AddComponent<CoinMovement>();
		}
		
		if(actives[Items.DOUBLE_JUMP]) {
			if(PlayerMovement.self.jumping){
				actives[Items.DOUBLE_JUMP] = false;
				return;
			}
		}

		if(actives[Items.FORCE_FALL]) {
			if(!PlayerMovement.self.jumping){
				actives[Items.FORCE_FALL] = false;
				return;
			}
		}

        if(untimed_items.Contains(available_items[index].item))
		    RemoveItem(index);
        else
            StartCoroutine(SetSliderValue(index));
	}

    public void AddItem(Items item) {
        // checking if the item exists already
        for(int i = 0; i < available_items_size; i++) {
            if(available_items[i].item == item){
                available_items[i] = new AvailableItem(item);
                return;
            }
        }

        // adding to null slot
        for(int i = 0; i < available_items_size; i++) {
            if(available_items[i].item == Items.NOTHING) {
                available_items[i] = new AvailableItem(item);
		        UiManager.self.SetItem(i, item);
                return;
            }
        }

        AvailableItem[] sorted = (AvailableItem[]) available_items.Clone();
        Array.Sort(sorted, delegate(AvailableItem x, AvailableItem y) { return x.time_added.CompareTo(y.time_added); });
        for(int i = 0; i < sorted.Length; i++) {
            Debug.Log(sorted[i].item + ", " + actives[sorted[i].item]);
            if(actives[sorted[i].item] == false) {
                int real_index = Array.IndexOf(available_items, sorted[i]);
                available_items[real_index] = new AvailableItem(item);
		        UiManager.self.SetItem(real_index, item);
                break;
            }
        }
	}

	public void RemoveItem(int index) {
        available_items[index] = new AvailableItem(Items.NOTHING);
		UiManager.self.SetItem(index, Items.NOTHING);

        if(SpecialAbilitiesManager.self.Has(SpecialAbilities.RANDOMER)) {
            Array items = Enum.GetValues(typeof(Items));
            AddItem((Items) items.GetValue(UnityEngine.Random.Range(0, items.Length - 1)));
        }
	}

    IEnumerator SetSliderValue(int index) {
        float time = max_time;
        while(time > 0) {
            UiManager.self.SetItemSlider(index, time / max_time);
            time -= .05f;
            yield return new WaitForSeconds(.05f);
        }
        UiManager.self.SetItemSlider(index, time / max_time);

		actives[available_items[index].item] = false;
		RemoveItem(index);
    }

}