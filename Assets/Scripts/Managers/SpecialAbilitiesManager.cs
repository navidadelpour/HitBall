using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAbilitiesManager : MonoBehaviour {

	public static SpecialAbilitiesManager self;
    public SpecialAbilities current_ability;
    public float time = 10f;
    public float range = .5f;
    public bool has = false;
    public bool active = false;
    public bool set_active = false;
    public float disable_time = 5f;
    private bool started;

	void Awake() {
		self = this;

        current_ability = SpecialAbilities.GUNNER;
	}

	void Start () {
	}
	
	void Update () {
        if(GameManager.self.started && !started) {
            started = true;
            StartCoroutine(Disable(0));
        }
	}

    IEnumerator Give() {
        yield return new WaitForSeconds(Random.Range(-range, range) + time);
        has = true;
        UiManager.self.EnableSpecialAbility();
    }

    public void Active() {
        if(has) {
            active = true;
            StartCoroutine(Disable(disable_time));
        }
    }

    IEnumerator Disable(float time) {
        UiManager.self.DisableSpecialAbility();
        yield return new WaitForSeconds(time);
        active = false;
        has = false;
        StartCoroutine(Give());
    }

    public bool Has(SpecialAbilities special_ability) {
        return current_ability == special_ability && active;
    }

}
