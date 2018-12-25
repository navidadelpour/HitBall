using UnityEngine;
public class Util {
    public static bool HasChance(int chance) {
		return Random.Range (0, 100) < chance;
	}

    public static float Ease(float start, float target, float scale, int sign = 1) {
        target += Mathf.Abs(target - start) * scale * sign;
        return target;
    }

}