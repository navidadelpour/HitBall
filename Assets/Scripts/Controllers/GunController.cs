using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
    
    public static GunController self;

    private GameObject bullet_prefab;

    private void Awake() {
        self = this;
        bullet_prefab = Resources.Load <GameObject>("prefabs/Bullet");
    }

    private void Start() {

    }

    private void Update() {

    }

    public void Shot() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.right * 1f, Vector3.right);
        if(hit != null && hit.collider != null) {
            if(hit.collider.tag == "Block" || hit.collider.tag == "Arrow"){
                Destroy(hit.collider.gameObject);
                return;
            }
        }
    }

}