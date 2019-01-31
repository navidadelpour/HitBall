using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingManager : MonoBehaviour {

	public static SettingManager self;
    public GameObject setting_panel;

	void Awake() {
		self = this;

        setting_panel = GameObject.Find("SettingPanel");
	}

	void Start () {
        UiManager.self.BringPanelsToCenter(new GameObject[]{
			setting_panel,
		});
	}
	
	void Update () {
		
	}

    public void OnSettingButtonClick() {
		Util.GoToPanel(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject, setting_panel);
    }

}
