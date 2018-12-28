using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util {
    public static bool HasChance(int chance) {
		return Random.Range (0, 100) < chance;
	}

    public static float Ease(float start, float target, float scale, int sign = 1) {
        target += Mathf.Abs(target - start) * scale * sign;
        return target;
    }

    public static Things GetKeyByChance(Dictionary<Things, int> chances) {
		int chances_sum = 0;
		foreach (KeyValuePair<Things, int> item in chances) {
			chances_sum += item.Value;
		}

		int chance = Random.Range (0, chances_sum);
		int previous_chances = 0;

		foreach (KeyValuePair<Things, int> item in chances) {
			if (chance < item.Value + previous_chances)
				return item.Key;
			previous_chances += item.Value; 
		}
		return Things.OBSTACLE;
	}

}