using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {
    public static TutorialManager self;
    private int current;
    private Sprite[] tutorial_sprites;
    private Image tutorial_image;
    private Button tutorial_back_button;

    void Awake() {
        self = this;

        tutorial_sprites = Resources.LoadAll<Sprite>("Textures/Tutorials");
        tutorial_image = GameObject.Find("TutorialPanel").transform.Find("Image").GetComponent<Image>();
        tutorial_back_button = GameObject.Find("TutorialPanel").transform.Find("BackButton").GetComponent<Button>();
    }

    void Start() {
        CheckForTutorialShown();
    }

    void Update() {

    }

    void CheckForTutorialShown() {
        if(PlayerPrefs.GetInt("tutorial_shown") != 1) {
            OnTutorialButtonClick();
            PlayerPrefs.SetInt("tutorial_shown", 1);
            tutorial_back_button.gameObject.SetActive(false);
        }
    }

    public void OnTutorialButtonClick() {
		current = 0;
        tutorial_image.sprite = tutorial_sprites[current];
        Util.GoToPanel(UiManager.self.menu_panel, UiManager.self.tutorial_panel);
		GameManager.self.on_player_views = false;
	}

    public void OnNextTutorialButtonClick() {
        if(current < tutorial_sprites.Length - 1)
            current++;
        if(current == tutorial_sprites.Length - 1)
            tutorial_back_button.gameObject.SetActive(true);
        tutorial_image.sprite = tutorial_sprites[current];
	}

    public void OnPreviousTutorialButtonClick() {
        if(current >= 1)
            current--;
        tutorial_image.sprite = tutorial_sprites[current];
	}


}