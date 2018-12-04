using UnityEngine;

public class PlayerIceBreak : MonoBehaviour
{
    public InputState inputState;
    public float breakIceInterval;

    private PlayerAVFX avfx;
    private Ice IceComponent;
    private float breakIceTimer;

    void OnEnable()
    {
        this.avfx = GetComponentInChildren<PlayerAVFX>();
        this.IceComponent = GetComponentInChildren<Ice>();
        this.breakIceTimer = this.breakIceInterval;
        inputState.EnableButtons = true;
    }

    void OnDisable()
    {
        inputState.EnableButtons = false;
    }
    
    void Update()
    {
        this.breakIceTimer -= Time.deltaTime;
        if (!inputState.BtnPrimary)
        {
            return;
        }

        if (inputState.BtnPrimary && this.breakIceTimer <= 0f)
        {
            if (!this.IceComponent)
            {
                this.IceComponent = GetComponentInChildren<Ice>();
            }

            this.breakIceTimer = this.breakIceInterval;
            this.IceComponent.Damage();

            if (!this.avfx)
            {
                this.avfx = GetComponentInChildren<PlayerAVFX>();
            }

            this.avfx.BreakIce();
        }
    }
}

