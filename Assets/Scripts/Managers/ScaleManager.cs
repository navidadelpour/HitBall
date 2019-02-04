using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleManager : MonoBehaviour {
    
    public static ScaleManager self;

    private float camera_size;
    private float camera_max_size = 10f;
    private float camera_normal_size;
    private float camera_range_size = 1f;
    private float size_increase_amount = .0001f;
    private float zoom_scale = 2;

    private Camera main_camera;
    private float height;
    private float width;
    private GameObject texture;
    private GameObject fixed_background;
    private GameObject grounds;
	private GameObject ground_prefab;
    private Vector3 ground_offset;

    private GameObject player;

	void Awake() {
		self = this;

		ground_prefab = Resources.Load <GameObject>("prefabs/Ground");
        texture = GameObject.Find("Texture");
        fixed_background = GameObject.Find("FixedBackground");
        grounds = GameObject.Find ("Grounds");
        main_camera = Camera.main;
        ground_offset = ground_prefab.GetComponent<BoxCollider2D> ().size * (Vector2) ground_prefab.transform.lossyScale / 2;
        camera_normal_size = main_camera.orthographicSize;
        camera_size = camera_normal_size;

        player = GameObject.Find("Player");
	}

	void Start () {

	} 

    void Update() {
        // setting background height and width to match the area that camera showes
        height = main_camera.orthographicSize;
        width = height * Screen.width / Screen.height;
        fixed_background.transform.localScale = new Vector3(width * 2.0f / 10 + .05f, 1, height * 2.0f / 10 + .05f);

        // setting the position
        Vector3 offset = -ground_offset + grounds.transform.position + new Vector3 (width, height, 0);
        fixed_background.transform.position = offset + Vector3.forward;
        main_camera.transform.position = offset + Vector3.forward * main_camera.transform.position.z + ScreenShake.self.amount;

        // adding an amount to size as the time goes by...
        if(camera_normal_size < camera_max_size && GameManager.self.started) 
            camera_normal_size += size_increase_amount;

        // player position
        player.transform.position = new Vector3(
            offset.x * 2 / 6,
            player.transform.position.y,
            player.transform.position.z
        );

		if(!ItemManager.self.actives[Item.WINGS]) {
            bool has_zoom = ItemManager.self.actives[Item.ZOOM];
            switch (SpeedManager.self.state) {
                case SpeedStates.INCREASE:
                    if(main_camera.orthographicSize < (camera_normal_size + camera_range_size) * (has_zoom ? zoom_scale : 1f)) {
                        Util.Ease(
                            ref camera_size,
                            (camera_normal_size + camera_range_size) * (has_zoom ? zoom_scale : 1f),
                            .5f
                        );
                        main_camera.orthographicSize = camera_size;
                    }
                    break;
                case SpeedStates.NORMALIZE:
                    if(main_camera.orthographicSize < camera_normal_size * (has_zoom ? zoom_scale : 1f)){ 
                        Util.Ease(
                            ref camera_size,
                            camera_normal_size * (has_zoom ? zoom_scale : 1f),
                            .5f
                        );
                        main_camera.orthographicSize = camera_size;
                    } else if (main_camera.orthographicSize > camera_normal_size * (has_zoom ? zoom_scale : 1f)){
                        Util.Ease(
                            ref camera_size,
                            camera_normal_size * (has_zoom ? zoom_scale : 1f),
                            .5f
                        );
                        main_camera.orthographicSize = camera_size;
                    }
                    break;
                case SpeedStates.DECREASE:
                    if(main_camera.orthographicSize > (camera_normal_size - camera_range_size) * (has_zoom ? zoom_scale : 1f)){
                        Util.Ease(
                            ref camera_size,
                            (camera_normal_size - camera_range_size) * (has_zoom ? zoom_scale : 1f),
                            .5f
                        );
                        main_camera.orthographicSize = camera_size;
                    }
                    break;
            }
        }
    }

}