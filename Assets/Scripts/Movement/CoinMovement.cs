using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMovement : MonoBehaviour {

    private GameObject player;
    private float radius = 5f;
    private bool should_follow;

	void Awake() {
        player = GameObject.Find("Player");
	}

	void Start () {

	}
	
	void FixedUpdate () {
        if(Mathf.Abs((transform.position - player.transform.position).magnitude) < radius)
            should_follow = true;

        if(should_follow)
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, .2f);
    }
}