using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffCameraDestroyer : MonoBehaviour {

	private Renderer object_renderer;
	private float delay;

	void Init() {
		delay = .5f;
		object_renderer = GetComponent<Renderer> ();
	}

	void Start () {
		Init ();
		StartCoroutine (check());
	}
	
	void Update () {
	
	}

	IEnumerator check() {
		yield return new WaitForSeconds (delay);
		if (!object_renderer.isVisible) {
			this.name = "DestroyedObject";
			Destroy (this.gameObject);
			switch (this.tag) {
			case "Ground":
				SpawnManager.instance.CreateGround();
				break;
			}
		}
		StartCoroutine (check ());
	}

}
