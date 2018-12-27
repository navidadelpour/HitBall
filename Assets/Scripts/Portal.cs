using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    private GameObject player;
    private float threshold = .2f;
    
	void Init() {
        player = GameObject.Find("Player");
	}

	void Start () {
		Init ();
	}
	
	void Update () {
        if(Mathf.Abs(GameManager.self.player_initial_position.x - transform.position.x) < threshold) {
            GameManager.self.has_teleport = false;
            PlayerMovement.self.enabled = true;
            player.transform.position = transform.position;
        }
	}

    private void OnDestroy() {
        SpawnManager.self.has_portal = false;
    }

}
