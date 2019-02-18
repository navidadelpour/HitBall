using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAbilitiesManager : MonoBehaviour {

	public static SpecialAbilitiesManager self;
    public SpecialAbilities current_ability;
    public float give_time = 5f;
    public float active_time = 10f;
    public bool has = false;
    public bool active = false;
    private bool started;

	void Awake() {
		self = this;
	}

	void Start () {
        UiManager.self.DisableSpecialAbility();
	}
	
	void Update () {
    
    }

    public void StartAct() {
        StartCoroutine(Give());
    }

    IEnumerator Give() {
        float time = 0;
        
        while(time < give_time) {
            UiManager.self.SetSpecialAbilitySlider(time / give_time, true);
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        UiManager.self.SetSpecialAbilitySlider(time / give_time, true);

        has = true;
        UiManager.self.EnableSpecialAbility();
    }

    public IEnumerator Active() {
        UiManager.self.DisableSpecialAbility();
        active = true;
        float time = active_time;

        while(time > 0) {
            UiManager.self.SetSpecialAbilitySlider(time / active_time, false);
            time -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        UiManager.self.SetSpecialAbilitySlider(time / active_time, false);

        active = false;
        has = false;
        StartCoroutine(Give());
    }

    public bool Has(SpecialAbilities special_ability) {
        return current_ability == special_ability && active;
    }

}
