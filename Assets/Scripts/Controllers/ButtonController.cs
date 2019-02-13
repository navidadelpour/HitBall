using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour {

    private Button button;

    public void Awake() {
        button = GetComponent<Button>();
        
        // disabling all the event listeners
        SetListenersState(UnityEventCallState.Off);
    }

    private void OnButtonClick() {   
        // enabling the listeners again
        SetListenersState(UnityEventCallState.EditorAndRuntime);

        // calling all listeres
        button.onClick.Invoke();

        // disabling all the event listeners
        SetListenersState(UnityEventCallState.Off);
	}

    private void PlaySfx() {
		AudioManager.self.Play("button");
    }

    private void SetListenersState(UnityEventCallState state) {
        for(int i = 0; i < button.onClick.GetPersistentEventCount(); i++) {
            button.onClick.SetPersistentListenerState(i, state);
        }
    }

}