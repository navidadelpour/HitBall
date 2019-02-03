using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

    public static ScreenShake self;

	public float decay = .5f;
	public Vector3 amount;
	private float range;

    void Awake() {
        self = this;
    }

	void Update () {
		if(range > 0) {
			amount = new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0f);
			range -= Time.deltaTime * decay;
		} else {
			amount = Vector3.zero;
		}

	}

	public void Shake(float toShake) {
		range = toShake;
	}
}