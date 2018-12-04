using UnityEngine;

[AddComponentMenu("UI/View Management/View")]
public class View : MonoBehaviour {
    public ViewType viewType;

    public void OnExitAnimationEnded() {
        var viewManager = transform.parent.GetComponent<ViewManager>();
        viewManager.OnExitAnimationEnded();
    }
}