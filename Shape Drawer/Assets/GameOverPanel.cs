using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : Panel {

    public GameObject objectToMove;
	public PanelAnimation panelAnmation;

    private void Awake()
    {
        panelType = PanelEnum.GameOverPanel;
    }

    private void OnEnable()
    {
        //objectToMove.transform.localPosition = new Vector3(0f, -2000f, 0f);
        //Hashtable hash = new Hashtable();
        //hash.Add("position", new Vector3(0f, 0f, 0f));
        //hash.Add("time", 1f);
        //hash.Add("islocal", true);

        //iTween.MoveTo(objectToMove, hash);
    }

    void HidePanel(string aHideCompleteFunction)
    {
        //Hashtable hash = new Hashtable();
        //hash.Add("position", new Vector3(0f, -2000f, 0f));
        //hash.Add("time", 1f);
        //hash.Add("islocal", true);
        //hash.Add("oncomplete", aHideCompleteFunction);
        //hash.Add("oncompletetarget", gameObject);
        //iTween.MoveTo(objectToMove, hash);
    }

    public void Retry()
    {
		panelAnmation.HidePanel("OnHideDoneRetry");
        //HidePanel("OnHideDoneRetry");
    }

    void OnHideDoneRetry()
    {
        CanvasManager.ClosePanel(panelType);
        EventManager.NextLevel(GameManager.GetCurrentLevelData());  
    }

    public void Quit()
    {
		panelAnmation.HidePanel("OnHideDoneQuit");
		//HidePanel("OnHideDoneQuit");
	}

	void OnHideDoneQuit()
    {
        CanvasManager.OpenPanel(PanelEnum.LevelSelectPanel, panelType);
        CanvasManager.ClosePanel(PanelEnum.InGamePanel);
    }
}
