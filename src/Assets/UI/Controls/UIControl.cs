using System;
using UnityEngine;

[ExecuteInEditMode]
public class UIControl : MonoBehaviour {
    public Skin skin;

    public virtual void OnSkinUI() {

    }

    public virtual void Awake() {
        OnSkinUI();
    }
}