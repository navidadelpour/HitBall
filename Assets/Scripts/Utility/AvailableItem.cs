using UnityEngine;

 [System.Serializable]
public class AvailableItem {
    public Items item;
    public float time_added;

    public AvailableItem(Items item) {
        this.item = item;
        this.time_added = Time.time;
    }
}