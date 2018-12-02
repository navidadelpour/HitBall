using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffCameraDestroyer : MonoBehaviour {

	private Renderer renderer;
	private float delay;

	void Init() {
		delay = .5f;
		renderer = GetComponent<Renderer> ();
	}

	// Use this for initialization
	void Start () {
		Init ();
		StartCoroutine (check());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator check() {
		yield return new WaitForSeconds (delay);
		if (!renderer.isVisible) {
			Debug.Log (this.name);
			switch (this.tag) {
			case "Ground":
				SpawnManager.instance.should_create_ground = true;
				break;
			}
		}
		StartCoroutine (check ());
	}

}
