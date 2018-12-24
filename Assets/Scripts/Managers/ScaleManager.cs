using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleManager : MonoBehaviour {
    
    public static ScaleManager self;
    private Camera main_camera;
    private GameObject background;
    private GameObject grounds;


	void Awake() {
		self = this;
	}

	void Init() {
        main_camera = Camera.main;
        background = GameObject.Find("Background");
        grounds = GameObject.Find ("Grounds");
	}

	void Start () {
		Init ();
	} 

    void Update() {
        // setting background height and width to match the area that camera showes
        float height = main_camera.orthographicSize * 2.0f / 10;
        float width = height * Screen.width / Screen.height;
        background.transform.localScale = new Vector3(width, 1, height);

    }
}