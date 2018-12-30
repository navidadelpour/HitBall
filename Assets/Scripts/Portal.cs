using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    private GameObject player;
    private float threshold = .2f;
    
	void Awake() {
        player = GameObject.Find("Player");
	}

	void Start () {

	}
	
	void Update () {
        if(Mathf.Abs(GameManager.self.player_initial_position.x - transform.position.x) < threshold) {
            ItemManager.self.has_teleport = false;
            PlayerMovement.self.enabled = true;
            player.transform.position = transform.position;
        }
	}

    private void OnDestroy() {
        SpawnManager.self.has_portal = false;
    }

}
