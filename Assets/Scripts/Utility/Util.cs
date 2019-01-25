using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util {
    public static bool HasChance(int chance) {
		return Random.Range (0, 100) < chance;
	}

	public static void Ease(ref float start, float target, float time) {
        start += (target - start) * Time.deltaTime * time;
    }

    public static System.Enum GetKeyByChance(Dictionary<System.Enum, int> chances) {
		int chances_sum = 0;
		foreach (KeyValuePair<System.Enum, int> item in chances) {
			chances_sum += item.Value;
		}

		int chance = Random.Range (0, chances_sum);
		int previous_chances = 0;

		foreach (KeyValuePair<System.Enum, int> item in chances) {
			if (chance < item.Value + previous_chances)
				return item.Key;
			previous_chances += item.Value; 
		}
		return null;
	}

	public static Transform FindDeepChild(Transform aParent, string aName){
		Transform result = aParent.Find(aName);
		if (result != null)
			return result;
		foreach(Transform child in aParent) {
			result = FindDeepChild(child, aName);
			if (result != null)
				return result;
		}
		return null;
	}

	public static void GoToPanel(GameObject from, GameObject to) {
		from.SetActive(false);
		to.SetActive(true);
	}

}