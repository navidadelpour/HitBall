using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {

    private AsyncOperation scene;

	void Awake() {
	
    }

    void Start () {
        scene = SceneManager.LoadSceneAsync("GameScene");
	}
	
}
