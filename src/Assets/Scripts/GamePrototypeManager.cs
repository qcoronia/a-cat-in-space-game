using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GamePrototypeManager : MonoBehaviour
{
    public InputState inputState;

    public PlayerSection player;
    public CompassSection compass;
    public ShipSection ship;

    [Header ("Game Events")]
    public UnityEvent OnHuggedRockHitIce;
    public UnityEvent OnIceDestroyed;
    public UnityEvent OnCatEnteredShip;
    public UnityEvent OnEndingCutsceneEnded;

    void Start ()
    {
        player.spawn = UnityHelpers.GetTransformByTag ("PlayerRespawn");
        ship.spawn = UnityHelpers.GetTransformByTag ("ShipRespawn");

        OnHuggedRockHitIce = OnHuggedRockHitIce ?? new UnityEvent ();
        OnIceDestroyed = OnIceDestroyed ?? new UnityEvent ();
        OnIceDestroyed.AddListener (EnableShipDoors);
        OnIceDestroyed.AddListener (DisableRockHugging);
        OnCatEnteredShip = OnCatEnteredShip ?? new UnityEvent ();
        OnCatEnteredShip.AddListener (TryEndGame);
        OnEndingCutsceneEnded = OnEndingCutsceneEnded ?? new UnityEvent ();
        OnEndingCutsceneEnded.AddListener (ShowSummary);
    }

    public void PlayGame ()
    {
        InstanceManager.View.Goto (ViewType.InGame);
        player.instance = Instantiate (player.prefab, player.spawn.position, player.spawn.rotation) as Transform;
        ship.instance = Instantiate (ship.prefab, ship.spawn.position, ship.spawn.rotation) as Transform;
        InstanceManager.Rocks.Reset ();
        inputState.IsEnabled = true;
    }

    public void PauseGame ()
    {
        InstanceManager.View.Goto (ViewType.Pause);
        Time.timeScale = 0f;
    }

    public void ResumeGame ()
    {
        InstanceManager.View.Goto (ViewType.InGame);
        Time.timeScale = 1f;
    }

    void EnableShipDoors ()
    {
        var shipAvfx = ship.instance.GetComponent<ShipAVFX> ();
        shipAvfx.enableDoors = true;
    }

    void DisableRockHugging ()
    {
        var plyr = player.instance.GetComponent<Player> ();
        plyr.enableRockHugging = false;
    }

    void TryEndGame ()
    {
        var shipAvfx = ship.instance.GetComponent<ShipAVFX> ();
        if (shipAvfx.isCatInside)
        {
            player.instance.gameObject.SetActive (false);
            EndGame ();
        }
    }

    public void EndGame ()
    {
        inputState.IsEnabled = false;
    }

    public void ShowSummary ()
    {
        InstanceManager.View.Goto (ViewType.Summary);
    }

    public void HomeScreen ()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
    }

    public void RetryGame ()
    {
        Destroy (player.instance.gameObject);
        Destroy (ship.instance.gameObject);
        player.instance = Instantiate (player.prefab, player.spawn.position, player.spawn.rotation) as Transform;
        ship.instance = Instantiate (ship.prefab, ship.spawn.position, ship.spawn.rotation) as Transform;
        inputState.IsEnabled = true;
    }
}

[System.Serializable]
public class PlayerSection
{
    public Transform prefab;
    public Transform spawn;
    [HideInInspector]
    public Transform instance;
}

[System.Serializable]
public class CompassSection
{
    public CompassPointer pointer;
}

[System.Serializable]
public class ShipSection
{
    public Transform prefab;
    public Transform spawn;
    [HideInInspector]
    public Transform instance;
}
