using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class Settings : ScriptableObject
{
    public float BgmVolume = 0.8f;
    public float SfxVolume = 0.8f;

    public UnityEvent OnRevert = new UnityEvent ();

    void OnEnable ()
    {
        JsonUtility.FromJsonOverwrite (PlayerPrefs.GetString ("settings"), this);
    }

    public void Apply ()
    {
        PlayerPrefs.SetString ("settings", JsonUtility.ToJson (this));
    }

    public void Revert ()
    {
        JsonUtility.FromJsonOverwrite (PlayerPrefs.GetString ("settings"), this);
    }
}
