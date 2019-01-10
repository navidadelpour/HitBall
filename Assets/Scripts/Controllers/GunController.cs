using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
    
    public static GunController self;

    public Dictionary<Guns, Gun> guns;
    private Guns active_gun;

    private float start_shoting;
    private bool reloading;
    private int current_ammo;

    private void Awake() {
        self = this;
        guns = new Dictionary<Guns, Gun> {
            {Guns.PISTOL, new Gun(Guns.PISTOL, 7, .5f, 2)},
            {Guns.RIFLE, new Gun(Guns.RIFLE, 30, .2f, 2)},
            {Guns.SHOTGUN, new Gun(Guns.SHOTGUN, 10, 1, 3)},
        };
        active_gun = Guns.RIFLE;
        current_ammo = guns[active_gun].ammo;
    }

    private void Start() {
        UiManager.self.SetGunText(current_ammo, guns[active_gun].ammo);
        UiManager.self.SetGun(active_gun);
    }

    private void Update() {
        if(current_ammo == 0 && !reloading) {
            StartCoroutine(Reload());
        }
    }

    public void Shot() {
        float time = guns[active_gun].shot_time * (SpecialAbilityManager.self.Has(SpecialAbility.GUNNER) ? .5f : 1);
        if(Time.time - start_shoting > time && current_ammo > 0) {
            start_shoting = Time.time;

            // Debug.Log("shot!");
            bool killed = false;
            RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.right * 1f, Vector3.right);
            if(hit.collider != null && !killed) {
                if(hit.collider.tag == "Block" || hit.collider.tag == "Arrow"){
                    Destroy(hit.collider.gameObject);
                    GameManager.self.enemies_killed_in_combo ++;
                    killed = true;
                }
            }

            current_ammo--;
            UiManager.self.SetGunText(current_ammo, guns[active_gun].ammo);
        }
    }

    IEnumerator Reload() {
        reloading = true;
        float time = guns[active_gun].reload_time * (SpecialAbilityManager.self.Has(SpecialAbility.GUNNER) ? .5f : 1);
        yield return new WaitForSeconds(time);
        current_ammo = guns[active_gun].ammo;
        reloading = false;
        UiManager.self.SetGunText(current_ammo, guns[active_gun].ammo);
    }

}