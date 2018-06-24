using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : Panel {

    public GameObject objectToMove;

    private void Awake()
    {
        panelType = PanelEnum.Pause;
    }

    private void OnEnable()
    {
        objectToMove.transform.localPosition = new Vector3(0f, -2000f, 0f);
        Hashtable hash = new Hashtable();
        hash.Add("position", new Vector3(0f, 0f, 0f));
        hash.Add("time", 1f);
        hash.Add("islocal", true);
        //hash.Add("oncomplete", "HidingComplete");
        //hash.Add("oncompletetarget", gameObject);
        iTween.MoveTo(objectToMove, hash);
    }

    public void ResumeGame()
    {
        Hashtable hash = new Hashtable();
        hash.Add("position", new Vector3(0f, -2000f, 0f));
        hash.Add("time", 1f);
        hash.Add("islocal", true);
        hash.Add("oncomplete", "HidingComplete");
        hash.Add("oncompletetarget", gameObject);
        iTween.MoveTo(objectToMove, hash);
    }

    void HidingComplete()
    {
        EventManager.ResumeGame();
        Time.timeScale = 1f;
        CanvasManager.ClosePanel(panelType);
    }

    void ShowComplete()
    {

    }

    public void Quit()
    {
        Hashtable hash = new Hashtable();
        hash.Add("position", new Vector3(0f, -2000f, 0f));
        hash.Add("time", 1f);
        hash.Add("islocal", true);
        hash.Add("oncomplete", "QuitComplete");
        hash.Add("oncompletetarget", gameObject);
        iTween.MoveTo(objectToMove, hash);
    }

    void QuitComplete()
    {
        CanvasManager.OpenPanel(PanelEnum.LevelSelectPanel, panelType);
        CanvasManager.ClosePanel(PanelEnum.InGamePanel);

    }
}
