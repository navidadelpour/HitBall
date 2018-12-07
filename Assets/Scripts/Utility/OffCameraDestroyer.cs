using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffCameraDestroyer : MonoBehaviour {

	private int destroy_offset = -12;

	void Init() {

	}

	void Start () {
		Init ();
	}
	
	void Update () {
		if (transform.position.x < destroy_offset) {
			this.name = "DestroyedObject";
			Destroy (this.gameObject);
			switch (this.tag) {
			case "Ground":
			case "Hole":
				GameManager.self.IncreamentScore ();
				SpawnManager.self.CreateGround();
				break;
			}
		}
	}

}
