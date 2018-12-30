using UnityEngine;

 [System.Serializable]
public class AvailableItem {
    public Item item;
    public float time_added;

    public AvailableItem(Item item) {
        this.item = item;
        this.time_added = Time.time;
    }
}