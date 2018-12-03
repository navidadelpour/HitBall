using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour {

	public static UiManager instance;

	void Init() {
		instance = this;
	}

	void Start () {
		
	}
	
	void Update () {
		
	}

	public void PauseButtonHandler() {
		if(GameManager.instance.paused)
			Time.timeScale = 1;
		else
			Time.timeScale = 0;
		GameManager.instance.paused = !GameManager.instance.paused;
	}
}
