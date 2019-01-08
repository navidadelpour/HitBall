using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
    
    public static GunController self;

    private GameObject bullet_prefab;
    private Dictionary<Guns, Gun> guns;
    private Guns active_gun;

    private float start_shoting;

    private void Awake() {
        self = this;
        bullet_prefab = Resources.Load <GameObject>("prefabs/Bullet");
        guns = new Dictionary<Guns, Gun> {
            {Guns.PISTOL, new Gun(Guns.PISTOL, 7, .5f, 1)},
            {Guns.RIFLE, new Gun(Guns.RIFLE, 30, .2f, 2)},
            {Guns.SHOTGUN, new Gun(Guns.SHOTGUN, 10, 1, 3)},
        };
        active_gun = Guns.PISTOL;
    }

    private void Start() {

    }

    private void Update() {

    }

    public void Shot() {
        if(Time.time - start_shoting > guns[active_gun].shot_time) {
            start_shoting = Time.time;
            
            Debug.Log("shot!");
            bool killed = false;
            RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.right * 1f, Vector3.right);
            if(hit != null && hit.collider != null && !killed) {
                if(hit.collider.tag == "Block" || hit.collider.tag == "Arrow"){
                    Destroy(hit.collider.gameObject);
                    killed = true;
                }
            }
        }
    }

}