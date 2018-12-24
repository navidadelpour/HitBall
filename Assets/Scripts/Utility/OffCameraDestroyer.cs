using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffCameraDestroyer : MonoBehaviour {

	private int destroy_offset = -7;
	private BoxCollider2D box_collider;

	void Init() {
		box_collider = transform.GetComponent<BoxCollider2D> ();
	}

	void Start () {
		Init ();
	}
	
	void Update () {
		if (transform.position.x < -box_collider.size.x * transform.lossyScale.x + destroy_offset) {
			this.name = "DestroyedObject";
			Destroy (this.gameObject);
			switch (this.tag) {
			case "Ground":
			case "Hole":
				GameManager.self.IncreamentScore ();
				SpawnManager.self.Spawn();
				break;
			}
		}
	}

}
