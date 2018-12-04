using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(
    fileName = "GameState",
    menuName = "Cat's Ship/Runtime States/Game State",
    order = 0)]
public class GameState : ScriptableObject
{
    public GameplayState state = GameplayState.Roaming;
    public int foundGems;
    public int totalGems;
    public bool goodieFound;
}

public enum GameplayState
{
    Roaming,
    HuggingRock,
    ShipAvailable,
    EndingCutscene,
}
