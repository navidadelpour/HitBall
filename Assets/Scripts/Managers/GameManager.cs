using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	void Init() {
		instance = this;
	}

	void Start () {
		Init ();
	}
	
	void Update () {

	}

}
