using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletePanel : Panel {

    public GameObject objectToMove;
    public GameObject nextLevelButton;

    private void Awake()
    {
        panelType = PanelEnum.LevelComplete;
    }

    private void OnEnable()
    {
        objectToMove.transform.localPosition = new Vector3(0f, -2000f, 0f);
        Hashtable hash = new Hashtable();
        hash.Add("position", new Vector3(0f, 0f, 0f));
        hash.Add("time", 1f);
        hash.Add("islocal", true);

        iTween.MoveTo(objectToMove, hash);

        nextLevelButton.SetActive(GameManager.IsLastLevel() == false);

        PlayerPrefs.SetInt("LevelComplete" + GameManager.CurrentLevelIndex.ToString(), 1);
    }

    void HidePanel(string aHideCompleteFunction)
    {
        Hashtable hash = new Hashtable();
        hash.Add("position", new Vector3(0f, -2000f, 0f));
        hash.Add("time", 1f);
        hash.Add("islocal", true);
        hash.Add("oncomplete", aHideCompleteFunction);
        hash.Add("oncompletetarget", gameObject);
        iTween.MoveTo(objectToMove, hash);
    }

    public void NextLevel()
    {
        HidePanel("OnHidingNextLevel");
    }

    void OnHidingNextLevel()
    {
        CanvasManager.ClosePanel(panelType);
        EventManager.NextLevel(GameManager.GetNextLevel());
    }

    public void Quit()
    {
        HidePanel("OnHidingQuit");
    }

    void OnHidingQuit()
    {
        CanvasManager.OpenPanel(PanelEnum.LevelSelectPanel, panelType);
    }
}