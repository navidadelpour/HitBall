using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAbilityManager : MonoBehaviour {

	public static SpecialAbilityManager self;
    public SpecialAbility current_ability;
    public float time = 5f;
    public float range = .5f;
    public bool has = false;
    public bool active = false;
    public bool set_active = false;
    public float disable_time = 5f;


	void Awake() {
		self = this;

        current_ability = SpecialAbility.LUCKY;
	}

	void Start () {
        UiManager.self.SetSpecialAbility(current_ability);
        StartCoroutine(Disable(0));
	}
	
	void Update () {

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

    public bool Has(SpecialAbility special_ability) {
        return current_ability == special_ability && active;
    }

}
