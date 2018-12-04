using System;
using UnityEngine;

public class AlienChaserAVFX : MonoBehaviour
{
    public GameState gameState;

    public FlailingLimb[] mouths;
    public FlailingLimb[] fins;

    public AlienChaser alienChaser;

    void Start()
    {
        alienChaser = transform.parent.GetComponent<AlienChaser>();
    }

    void Update()
    {
        switch (gameState.state)
        {
            case GameplayState.HuggingRock:
                SetState(AlienChaserState.Chasing);
                break;
            default:
                SetState(AlienChaserState.Roaming);
                break;
        }
    }

    public void SetState(AlienChaserState state)
    {
        if (alienChaser.state == state)
        {
            return;
        }

        var randomOffsetAngle = UnityEngine.Random.Range(0f, 360f);
        var offsetAngle = Mathf.FloorToInt(randomOffsetAngle);
        alienChaser.state = state;
        for (var i = 0; i < mouths.Length; i++)
        {
            mouths[i].SetProfile(alienChaser.state.ToString());
            mouths[i].SetOffset(offsetAngle);
        }

        for (var i = 0; i < fins.Length; i++)
        {
            fins[i].SetProfile(alienChaser.state.ToString());
            fins[i].SetOffset(offsetAngle);
        }

        alienChaser.AlienStateChange();
    }
}
