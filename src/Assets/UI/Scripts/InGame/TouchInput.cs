using UnityEngine;
using System.Collections;

public class TouchInput : MonoBehaviour {
	public InputState inputState;

	public float knobRadiusLimit;

	private Transform background;
	private Transform knob;

	void Start() {
		background = transform.Find("Background");
		knob = transform.Find("Knob");
	}

	void FixedUpdate() {
        if (!inputState.HasInput) {
			background.gameObject.SetActive(false);
			knob.gameObject.SetActive(false);
            return;
        }

		background.gameObject.SetActive(true);
		knob.gameObject.SetActive(true);

		var initPosPx = inputState.JoyStick.initPos;
        var offset = inputState.JoyStick.value;

		background.position = initPosPx;
		knob.position = initPosPx + Vector2.ClampMagnitude(offset, knobRadiusLimit);
	}
}
