using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffCameraDestroyer : MonoBehaviour {

	private float delay;

	void Init() {
		delay = .5f;
	}

	void Start () {
		Init ();
		StartCoroutine (check());
	}
	
	void Update () {
	
	}

	IEnumerator check() {
		yield return new WaitForSeconds (delay);
		if (transform.position.x < -10) {
			this.name = "DestroyedObject";
			Destroy (this.gameObject);
			switch (this.tag) {
				case "Ground":
				case "Hole":
					SpawnManager.instance.CreateGround();
					break;
			}
		}
		StartCoroutine (check ());
	}

}
