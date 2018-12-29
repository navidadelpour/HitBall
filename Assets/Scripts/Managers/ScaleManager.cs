using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleManager : MonoBehaviour {
    
    public static ScaleManager self;

    private float camera_max_size = 10f;
    private float camera_normal_size;
    private float camera_range_size = 1f;
    private float size_increase_amount = .0001f;
    private float zoom_scale = 2;

    private Camera main_camera;
    private float height;
    private float width;
    private GameObject background;
    private GameObject grounds;
	private GameObject ground_prefab;
    private Vector3 ground_offset;

	void Awake() {
		self = this;

		ground_prefab = Resources.Load <GameObject>("prefabs/Ground");
        background = GameObject.Find("Background");
        grounds = GameObject.Find ("Grounds");
        main_camera = Camera.main;
        ground_offset = ground_prefab.GetComponent<BoxCollider2D> ().size * (Vector2) ground_prefab.transform.lossyScale / 2;
        camera_normal_size = main_camera.orthographicSize;
	}

	void Start () {

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

        if(camera_normal_size < camera_max_size) 
            camera_normal_size += size_increase_amount;

        switch (SpeedManager.self.state) {
            case SpeedStates.INCREASE:
                if(main_camera.orthographicSize < (camera_normal_size + camera_range_size) * (GameManager.self.has_zoom ? zoom_scale : 1f)) 
                    main_camera.orthographicSize = Util.Ease(
                        (camera_normal_size + camera_range_size) * (GameManager.self.has_zoom ? zoom_scale : 1f),
                        main_camera.orthographicSize,
                        size_increase_amount * 100
                    );
                break;
            case SpeedStates.NORMALIZE:
                if(main_camera.orthographicSize < camera_normal_size * (GameManager.self.has_zoom ? zoom_scale : 1f)) 
                    main_camera.orthographicSize = Util.Ease(
                        camera_normal_size * (GameManager.self.has_zoom ? zoom_scale : 1f),
                        main_camera.orthographicSize,
                        size_increase_amount * 100
                    );
                else if (main_camera.orthographicSize > camera_normal_size * (GameManager.self.has_zoom ? zoom_scale : 1f))
                    main_camera.orthographicSize = Util.Ease(
                        camera_normal_size * (GameManager.self.has_zoom ? zoom_scale : 1f),
                        main_camera.orthographicSize,
                        size_increase_amount * 100, -1
                    );
                break;
            case SpeedStates.DECREASE:
                if(main_camera.orthographicSize > (camera_normal_size - camera_range_size) * (GameManager.self.has_zoom ? zoom_scale : 1f)) 
                    main_camera.orthographicSize = Util.Ease(
                        (camera_normal_size - camera_range_size) * (GameManager.self.has_zoom ? zoom_scale : 1f),
                        main_camera.orthographicSize,
                        size_increase_amount * 100, -1
                    );
                break;
        }
    }

}