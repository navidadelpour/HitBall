using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffCameraDestroyer : MonoBehaviour {

	private BoxCollider2D box_collider;
	private float grounds_position_x;

	void Awake() {
		box_collider = transform.GetComponent<BoxCollider2D> ();
		grounds_position_x = GameObject.Find("Grounds").transform.position.x;
	}

	void Start () {

	}
	
	void Update () {
		if (transform.position.x < -box_collider.size.x * transform.lossyScale.x + grounds_position_x) {
			this.name = "DestroyedObject";
			Destroy (this.gameObject);
			switch (this.tag) {
			case "Ground":
			case "Hole":
				GameManager.self.IncreamentScore ();
				SpawnManager.self.Spawn();
				break;
			}
			if(this.transform.childCount > 0 && this.transform.GetChild(0).tag == "Block")
				GameManager.self.ResetCombo();
		}
	}

}
