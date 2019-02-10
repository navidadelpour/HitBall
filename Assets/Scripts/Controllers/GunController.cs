using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
    
    public static GunController self;

    public Dictionary<Guns, Gun> guns;
    public Guns active_gun;

    private float start_shoting;
    private bool reloading;
    private int current_ammo;
    private Animator gun_animator;
    
    private void Awake() {
        self = this;
        guns = new Dictionary<Guns, Gun> {
            {Guns.PISTOL, new Gun(Guns.PISTOL, 7, .5f, 2)},
            {Guns.RIFLE, new Gun(Guns.RIFLE, 30, .2f, 2)},
            {Guns.SHOTGUN, new Gun(Guns.SHOTGUN, 10, 1, 3)},
        };
    }

    private void Start() {
        gun_animator = PlayerMovement.self.transform.Find("GunAnimator").GetComponent<Animator>();
    }

    public void SetGun(Guns gun) {
        active_gun = gun;
        current_ammo = guns[active_gun].ammo;
    }

    void Update() {

    }

    public void Shot() {
        float time = guns[active_gun].shot_time * (SpecialAbilitiesManager.self.Has(SpecialAbilities.GUNNER) ? .5f : 1);
        if(Time.time - start_shoting > time && current_ammo > 0) {
            start_shoting = Time.time;

            // TODO : adding more rays to increase shot hit rate
            bool killed = false;
            RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.right * 1f, Vector3.right);
            if(hit.collider != null && !killed) {
                if(hit.collider.tag == "Block" || hit.collider.tag == "Arrow"){
                    Destroy(hit.collider.gameObject);
                    GameManager.self.HandleEnemyKill();
                    killed = true;
                }
            }

            current_ammo--;
            UiManager.self.SetGunText(current_ammo, guns[active_gun].ammo);
            gun_animator.SetTrigger("Shot");
            AudioManager.self.Play("gun_shot");
            if(current_ammo == 0 && !reloading) {
                StartCoroutine(Reload());
            }
        }
    }

    IEnumerator Reload() {
        reloading = true;
        gun_animator.SetTrigger("Reload");
        float time = guns[active_gun].reload_time * (SpecialAbilitiesManager.self.Has(SpecialAbilities.GUNNER) ? .5f : 1);
        AudioManager.self.Play("reloading");
        yield return new WaitForSeconds(time);
        current_ammo = guns[active_gun].ammo;
        reloading = false;
        UiManager.self.SetGunText(current_ammo, guns[active_gun].ammo);
    }

}