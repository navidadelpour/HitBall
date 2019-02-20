using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButtonsTimeManager : MonoBehaviour {

    public static JumpButtonsTimeManager self;
    
    public float get_full_time = 10f;
    public float get_empty_time = 1f;
    private float high_time = 1;
    private float low_time = 1;
    private SpeedStates state;

    void Awake() {
        self = this;
    }

    void Start() {

    }

    void Update() {
        state = InputManager.self.state;
        switch(state) {
            case SpeedStates.INCREASE:
                Decrease(ref high_time);
                Increase(ref low_time);
                break;
            case SpeedStates.NORMALIZE:
                Increase(ref high_time);
                Increase(ref low_time);
                break;
            case SpeedStates.DECREASE:
                Increase(ref high_time);
                Decrease(ref low_time);
                break;
        }
        SpeedManager.self.state = state;
        UiManager.self.SetJumpButtonSlidersValue(high_time, low_time);
    }

    void Decrease(ref float time) {
        if(time > 0)
            time -= Time.deltaTime / get_empty_time;
        else
            state = SpeedStates.NORMALIZE;
    }

    void Increase(ref float time) {
        if(time < 1)
            time += Time.deltaTime / get_full_time;
    }

}