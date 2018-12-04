using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Skin : ScriptableObject {

	[Header("Button")]
	public Sprite buttonSprite;
	public SpriteState buttonSpriteState;
	public Sprite buttonIcon;
	public Color buttonColor;
}