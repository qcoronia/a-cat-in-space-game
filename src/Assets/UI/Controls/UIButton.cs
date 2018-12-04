using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class UIButton : UIControl {

    private Button button;
    public override void OnSkinUI() {
        button = GetComponent<Button>();

        button.image.sprite = skin.buttonSprite;
        button.spriteState = skin.buttonSpriteState;
        button.targetGraphic.color = skin.buttonColor;

        button.transition = Selectable.Transition.SpriteSwap;
    }
}