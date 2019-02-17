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
    public int cartridge;
    private Animator gun_animator;
    private Animator face_animator;
    private GameObject gun_fire;
    
    private LineRenderer laser_renderer;
    private Vector3 target_pos;
    private float length = 20f;
    public GameObject target;
    private RaycastHit2D hit;

    private void Awake() {
        self = this;
        guns = new Dictionary<Guns, Gun> {
            {Guns.PISTOL, new Gun(Guns.PISTOL, 7, .5f, 3)},
            {Guns.RIFLE, new Gun(Guns.RIFLE, 30, .15f, 4)},
            {Guns.SHOTGUN, new Gun(Guns.SHOTGUN, 10, .5f, 1)},
        };
        target_pos = Vector3.right * length;
        laser_renderer = GetComponent<LineRenderer>();
    }

    private void Start() {
        gun_animator = PlayerMovement.self.transform.Find("GunAnimator").GetComponent<Animator>();
        face_animator = PlayerMovement.self.transform.Find("Face").GetComponent<Animator>();
        gun_fire = PlayerMovement.self.transform.Find("GunAnimator").Find("GunFire").gameObject;
        gun_fire.SetActive(false);
    }

    public void SetGun(Guns gun) {
        active_gun = gun;
        current_ammo = guns[active_gun].ammo;
        cartridge = 1;
    }

    void Update() {
        target_pos = transform.position + Vector3.right * length;
        hit = Physics2D.Raycast(transform.position + Vector3.right * 1f, Vector3.right);
        if(hit.collider != null) {
            if(hit.collider.tag == "Block" || hit.collider.tag == "Item") {
                target = hit.collider.gameObject;
            }
            target_pos = (Vector3) hit.point;
        } else {
            target = null;
        }
        if(laser_renderer.enabled) {
            laser_renderer.SetPosition(0, this.transform.position);
            laser_renderer.SetPosition(1, target_pos);
        }
    }

    public void Shot() {
        float time = guns[active_gun].shot_time * (SpecialAbilitiesManager.self.Has(SpecialAbilities.GUNNER) ? .5f : 1);
        if(Time.time - start_shoting > time) {
            start_shoting = Time.time;
            if(current_ammo > 0) {
                bool killed = false;
                if(target != null && !killed) {
                    Destroy(target.gameObject);
                    ParticleManager.self.Spawn("Block", target.transform.position);
                    if(target.tag == "Block") {
                        GameManager.self.IncreamentCombo();
                        killed = true;
                        AudioManager.self.Play("block_destroy");
                    }
                }

                current_ammo--;
                UiManager.self.SetGunText(current_ammo, guns[active_gun].ammo * cartridge);
                gun_animator.SetTrigger("Shot");
                face_animator.SetTrigger("Shot");
                AudioManager.self.Play(active_gun.ToString().ToLower() + "_shot");
                StartCoroutine(ShotFire());
                if(current_ammo == 0 && !reloading) {
                    if(cartridge != 0)
                        StartCoroutine(Reload());
                }
            } else if(cartridge == 0) {

                AudioManager.self.Play("no_ammo");

            }
        }
    }

    IEnumerator ShotFire() {
        gun_fire.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gun_fire.SetActive(false);
    }

    IEnumerator Reload() {
        laser_renderer.enabled = false;
        reloading = true;
        gun_animator.SetTrigger("Reload");
        float time = guns[active_gun].reload_time * (SpecialAbilitiesManager.self.Has(SpecialAbilities.GUNNER) ? .5f : 1);
        AudioManager.self.Play(active_gun.ToString().ToLower() + "_reload");
        yield return new WaitForSeconds(time);
        current_ammo = guns[active_gun].ammo;
        cartridge--;
        reloading = false;
        UiManager.self.SetGunText(current_ammo, guns[active_gun].ammo * cartridge);
        laser_renderer.enabled = true;
    }

    public void AddCartridge() {
        cartridge++;
        UiManager.self.SetGunText(current_ammo, guns[active_gun].ammo * cartridge);
        AudioManager.self.Play("ammo_pickup");
        if(current_ammo == 0 && !reloading)
            StartCoroutine(Reload());
    }

}