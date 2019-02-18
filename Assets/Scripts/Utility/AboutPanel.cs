using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke("StartGame", 1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void StartGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
	}
}
