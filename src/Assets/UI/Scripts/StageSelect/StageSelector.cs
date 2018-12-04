using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelector : MonoBehaviour
{
    public Profile profile;
    public LevelInfo currentLevelInfo;
    public LevelInfo comingSoonLevelInfo;
    public List<LevelInfo> levels;

    [Header("UI Elements")]
    public Text LevelName;
    public Image LevelGraphicDisplay;

    private int currentLevelInfoIndex = 0;
    private List<LevelInfo> availableLevels;

    void Start()
    {
        var reachedAvailableLevel = this.profile.ReachedLevel + 1;
        this.availableLevels = this.levels.Where(levelInfo =>
        {
            return reachedAvailableLevel >= levelInfo.LevelIndex;
        }).ToList();
        this.SelectLevel(Mathf.Max(0, this.availableLevels.Count - 1));
        if (this.levels.All(e => reachedAvailableLevel >= e.LevelIndex))
        {
            this.availableLevels.Add(this.comingSoonLevelInfo);
        }
    }
    
    public void ShowNextLevel()
    {
        this.SelectLevel(this.currentLevelInfoIndex + 1);
    }

    public void ShowPrevLevel()
    {
        this.SelectLevel(this.currentLevelInfoIndex - 1);
    }

    public void SelectLevel(int index)
    {
        this.currentLevelInfoIndex = Convert.ToInt32(
            Mathf.Repeat(index, this.availableLevels.Count - 0)
        );
        this.currentLevelInfo = this.availableLevels[this.currentLevelInfoIndex];

        this.LevelName.text = this.currentLevelInfo.name;
        this.LevelGraphicDisplay.sprite = this.currentLevelInfo.LevelGraphic;
    }

    public void SelectLevelWithBuildIndex(int index)
    {
        for (int i = 0; i < this.availableLevels.Count; i++)
        {
            if (this.availableLevels[i].BuildIndex == index)
            {
                this.SelectLevel(i);
                break;
            }
        }
    }

    public void LoadSelectedLevel()
    {
        if (this.currentLevelInfo.BuildIndex < 0)
        {
            return;
        }

        SceneManager.LoadScene(this.currentLevelInfo.BuildIndex);
    }
}
