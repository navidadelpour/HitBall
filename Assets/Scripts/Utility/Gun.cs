using UnityEngine;

[System.Serializable]
public class Gun {

    public Guns type;
    public int ammo;
    public float shot_time;
    public float reload_time;

    public Gun(Guns type, int ammo, float shot_time, float reload_time) {
        this.type = type;
        this.ammo = ammo;
        this.shot_time = shot_time;
        this.reload_time = reload_time;
    }

    override
    public string ToString() {
        return string.Format("ammo: {0}\nshot time: {1}s\nreload_time: {2}s", ammo, shot_time, reload_time);
    }
}