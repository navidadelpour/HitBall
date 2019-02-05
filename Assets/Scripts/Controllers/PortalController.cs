using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour {

    private GameObject player;
    private float threshold = .5f;
    
	void Awake() {
        player = GameObject.Find("Player");
	}

	void Start () {

	}
	
	void Update () {
        if(Mathf.Abs(PlayerMovement.self.gameObject.transform.position.x - transform.position.x) < threshold) {
            ItemManager.self.actives[Items.TELEPORT] = false;
            ItemManager.self.actives[Items.GROUND_DIGGER] = false;
            PlayerMovement.self.enabled = true;
            player.transform.position = transform.position;
        }
	}

    private void OnDestroy() {
        SpawnManager.self.has_portal = false;
    }

}
