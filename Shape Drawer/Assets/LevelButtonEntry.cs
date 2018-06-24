using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

[Serializable]
public class LevelButtonEntry : MonoBehaviour {

    public Text myLevelNumber;
    public Button myButton;
    public LevelData myLevelData;
    public Image octoImage;
    public Sprite[] octoSprites;


    public void SetUp(LevelData aLevelData)
    {
        myLevelNumber.text = (aLevelData.LevelIndex + 1).ToString();
        myLevelData = aLevelData;
        int levelComplete = PlayerPrefs.GetInt("LevelComplete" + myLevelData.LevelIndex.ToString(), 0);
        octoImage.sprite = octoSprites[levelComplete];
        myLevelData.IsComplete = levelComplete == 1;
    }

    public void EneterLevel()
    {
        CanvasManager.OpenPanel(PanelEnum.LevelInfo, new LevelInfoPanelData { myLevelData = myLevelData});
        //EventManager.StartGame(myLevelData);
    }
}
