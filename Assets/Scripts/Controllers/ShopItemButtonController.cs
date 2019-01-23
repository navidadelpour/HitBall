using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemButtonController : MonoBehaviour {

	public void OnClick() {
		UiManager.self.EnableShopItem(this.gameObject);
	}

}