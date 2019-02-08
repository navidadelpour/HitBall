using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour {

	public static InputManager self;

	private bool gun_button_pressed;
	private bool jump_max_button_pressed;
	private bool jump_min_button_pressed;

	void Awake() {
		self = this;
	}

	void Start () {

	}

	void Update () {
		if(gun_button_pressed)
			GunController.self.Shot();

		if(jump_max_button_pressed)
			SpeedManager.self.state = SpeedStates.INCREASE;
		else if(jump_min_button_pressed)
			SpeedManager.self.state = SpeedStates.DECREASE;
		else
			SpeedManager.self.state = SpeedStates.NORMALIZE;

	}

	public void OnPlayButtonClick() {
		GameManager.self.started = true;
		AudioManager.self.Play("button");
	}

	public void OnPauseButtonClick() {
		if(GameManager.self.paused)
			Time.timeScale = 1;
		else
			Time.timeScale = 0;
		GameManager.self.paused = !GameManager.self.paused;
		UiManager.self.pause_panel.SetActive(GameManager.self.paused);
		AudioManager.self.Play("button");
	}

	public void OnResetButtonClick() {
		SceneManager.LoadScene("Scene1");
		AudioManager.self.Play("button");
	}

	public void OnRateButtonClick() {
		// TODO : rate buttion functionality
	}

	public void OnGunbuttonHold() {
		gun_button_pressed = !gun_button_pressed;
		AudioManager.self.Play("button");
	}

	public void OnSpecialAbilitybuttonClick() {
		SpecialAbilitiesManager.self.Active();
		AudioManager.self.Play("button");
	}

	public void OnJumpMaxButtonHold() {
		jump_max_button_pressed = !jump_max_button_pressed;
		AudioManager.self.Play("button");
	}

	public void OnJumpMinButtonHold() {
		jump_min_button_pressed = !jump_min_button_pressed;
		AudioManager.self.Play("button");
	}

	public void OnItemButtonClick() {
		int index = int.Parse(EventSystem.current.currentSelectedGameObject.name);
		if(ItemManager.self.available_items[index].item != Items.NOTHING) {
			ItemManager.self.ActiveItem(index);
		}
		AudioManager.self.Play("button");
	}

}
