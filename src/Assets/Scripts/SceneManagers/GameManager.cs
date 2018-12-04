using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameState GameState;
    public InputState InputState;
    public Transform ShipObj;
    public Transform CatObj;
    public int totalGems;
    public string goodieCode;
    public Profile Profile;
    public ItemList ItemList;
    public int levelIndex;
    public ViewManager ViewManager;
    public EventHandler OnHuggedRock;
    public EventHandler OnHuggedRockHitIce;
    public EventHandler OnHuggedRockBroke;
    public EventHandler OnAlienEatRock;
    public EventHandler OnIceDestroyed;
    public EventHandler OnCatFrozen;
    public EventHandler OnCatEnteredShip;
    public EventHandler OnPlayerConsumeGoodie;
    public EventHandler OnEndingCutsceneEnded;

    public void Start()
    {
        GameState.state = GameplayState.Roaming;
        GameState.totalGems = totalGems;
        InputState.IsEnabled = false;
        ShipObj.gameObject.SetActive(false);
        CatObj.gameObject.SetActive(false);

        OnHuggedRock.OnInvoke.AddListener(HuggedRock);
        OnHuggedRockHitIce.OnInvoke.AddListener(HuggedRockHitIce);
        OnHuggedRockBroke.OnInvoke.AddListener(HuggedRockBroke);
        OnAlienEatRock.OnInvoke.AddListener(AlienEatRock);
        OnIceDestroyed.OnInvoke.AddListener(IceDestroyed);
        OnCatEnteredShip.OnInvoke.AddListener(CatEnteredShip);
        OnPlayerConsumeGoodie.OnInvoke.AddListener(PlayerConsumeGoodie);
        OnEndingCutsceneEnded.OnInvoke.AddListener(LevelEnded);
    }

    void Update()
    {
        if (InputState.BtnBack)
        {
            if (ViewManager.currentView == ViewType.InGame)
            {
                Pause();
            }
            else if (ViewManager.currentView == ViewType.Pause)
            {
                Resume();
            }
        }
    }

    public void HuggedRock()
    {
        GameState.state = GameplayState.HuggingRock;
    }

    public void HuggedRockHitIce()
    {
        GameState.state = GameplayState.Roaming;
    }

    public void HuggedRockBroke()
    {
        GameState.state = GameplayState.Roaming;
    }

    public void AlienEatRock()
    {
        GameState.state = GameplayState.Roaming;
    }

    public void IceDestroyed()
    {
        GameState.state = GameplayState.ShipAvailable;
    }

    public void CatEnteredShip()
    {
        GameState.state = GameplayState.EndingCutscene;
    }

    public void PlayerConsumeGoodie()
    {
        var item = ItemList.GetByName(goodieCode);
        Profile.UnlockedItems.Add(item);
        GameState.goodieFound = true;
    }

    public void ResetGameState()
    {
        GameState.foundGems = 0;
        GameState.goodieFound = false;
    }

    public void Begin()
    {
        this.ResetGameState();
        ShipObj.gameObject.SetActive(true);
        CatObj.gameObject.SetActive(true);
        Time.timeScale = 1f;
        ViewManager.Goto(ViewType.InGame);
        InputState.IsEnabled = true;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        ViewManager.Goto(ViewType.Pause);
        InputState.IsEnabled = false;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        ViewManager.Goto(ViewType.InGame);
        InputState.IsEnabled = true;
    }

    public void LevelEnded()
    {
        ViewManager.Goto(ViewType.Summary);
        InputState.IsEnabled = false;
        if (levelIndex >= Profile.ReachedLevel)
        {
            Profile.Gems += GameState.foundGems;
            Profile.ReachedLevel = levelIndex;
            Profile.Apply();
        }
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToStageSelect()
    {
        Time.timeScale = 1f;
        var sceneLoadOptions = GameObject.CreatePrimitive(PrimitiveType.Quad);
        sceneLoadOptions.tag = "SceneLoadOptions";
        DontDestroyOnLoad(sceneLoadOptions);
        Destroy(sceneLoadOptions.GetComponent<MeshRenderer>());
        Destroy(sceneLoadOptions.GetComponent<Collider>());
        sceneLoadOptions.AddComponent<SceneLoadOptions>();

        var sceneLoadOptionComponent = sceneLoadOptions.GetComponent<SceneLoadOptions>();
        sceneLoadOptionComponent.TargetView = ViewType.LevelSelect;
        sceneLoadOptionComponent.ExitedLevel = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene("Home");
    }

    public void BackToHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Home");
    }
}
