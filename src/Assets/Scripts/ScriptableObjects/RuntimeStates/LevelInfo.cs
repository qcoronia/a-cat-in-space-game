using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "LevelInfo",
    menuName = "Cat's Ship/Runtime States/Level Info",
    order = 0)]
public class LevelInfo : ScriptableObject
{
    public int LevelIndex;
    public int BuildIndex;
    public int RequiredGems;
    public int TotalGems;
    public int FoundGems;
    public Sprite LevelGraphic;
}
