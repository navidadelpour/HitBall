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
	}

	public void OnPauseButtonClick() {
		if(GameManager.self.paused)
			Time.timeScale = 1;
		else
			Time.timeScale = 0;
		GameManager.self.paused = !GameManager.self.paused;
	}

	public void OnResetButtonClick() {
		OnPauseButtonClick();
		SceneManager.LoadScene("Scene1");
	}

	public void OnGunbuttonHold() {
		gun_button_pressed = !gun_button_pressed;
	}

	public void OnSpecialAbilitybuttonClick() {
		SpecialAbilityManager.self.Active();
	}

	public void OnJumpMaxButtonHold() {
		jump_max_button_pressed = !jump_max_button_pressed;
	}

	public void OnJumpMinButtonHold() {
		jump_min_button_pressed = !jump_min_button_pressed;
	}

	public void OnItemButtonClick() {
		int index = int.Parse(EventSystem.current.currentSelectedGameObject.name);
		if(ItemManager.self.available_items[index].item != Item.NOTHING) {
			ItemManager.self.ActiveItem(index);
		}
	}

}
