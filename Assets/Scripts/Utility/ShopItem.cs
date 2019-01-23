using UnityEngine;

[System.Serializable]
public class ShopItem {
    public string description;
    public int cost;
    public int level = 1;
    public int max_level = 3;
    public bool unlock = false;

    public ShopItem(string description, int cost) {
        this.description = description;
        this.cost = cost;
    }

}