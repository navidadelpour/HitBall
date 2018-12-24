using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleManager : MonoBehaviour {
    
    public static ScaleManager self;
    private int camera_max_size = 10;
    private float size_increase_amount = .001f;
    private Camera main_camera;
    public float height;
    public float width;
    public GameObject background;
    private GameObject grounds;
	private GameObject ground_prefab;
    private Vector3 ground_offset;

	void Awake() {
		self = this;
	}

	void Init() {
		ground_prefab = Resources.Load <GameObject>("prefabs/Ground");
        background = GameObject.Find("Background");
        grounds = GameObject.Find ("Grounds");
        main_camera = Camera.main;
        ground_offset = ground_prefab.GetComponent<BoxCollider2D> ().size * (Vector2) ground_prefab.transform.lossyScale / 2;
	}

	void Start () {
		Init ();
	} 

    void Update() {
        // setting background height and width to match the area that camera showes
        height = main_camera.orthographicSize;
        width = height * Screen.width / Screen.height;
        background.transform.localScale = new Vector3(width * 2.0f / 10, 1, height * 2.0f / 10);

        // setting the position
        Vector3 offset = -ground_offset + grounds.transform.position + new Vector3 (width, height, 0);
        background.transform.position = offset;
        main_camera.transform.position = offset + Vector3.forward * main_camera.transform.position.z;

        if(main_camera.orthographicSize < camera_max_size) 
            main_camera.orthographicSize += size_increase_amount;
    }
}