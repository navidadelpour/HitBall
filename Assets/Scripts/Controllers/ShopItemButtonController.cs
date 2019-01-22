using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemButtonController : MonoBehaviour {

	private GameObject clicked_object;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnClick() {
		Debug.Log(name);
	}
}
