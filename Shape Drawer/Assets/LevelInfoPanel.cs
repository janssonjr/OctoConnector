using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfoPanel : Panel{


    public GameObject objectToMove;
    public Text myTitle;
    public Image myStar;
    public Sprite greyStar;
    public Sprite star;
	public RectTransform background;
	public GameObject content;

    LevelData myLevelData;

	private void Awake()
    {
        panelType = PanelEnum.LevelInfo;
    }

    private void OnEnable()
    {
		objectToMove.transform.localPosition = new Vector3(0f, -2000f, 0f);
        Hashtable hash = new Hashtable();
        hash.Add("position", new Vector3(0f, 0f, 0f));
        hash.Add("time", 1f);
        hash.Add("islocal", true);
		hash.Add("oncomplete", "OnShowComplete");
		hash.Add("oncompletetarget", gameObject);

        iTween.MoveTo(objectToMove, hash);

        if(myLevelData.IsComplete == true)
        {
            myStar.sprite = star;
        }
        else
        {
            myStar.sprite = greyStar;
        }

        myTitle.text = "Level " + (myLevelData.LevelIndex + 1).ToString();
    }

	void OnShowComplete()
	{
		iTween.ValueTo(background.gameObject, iTween.Hash("from", 660, "to", 1300, "time", 0.5f, "onupdate","UpdateValue", "onupdatetarget",gameObject));
	}

	void UpdateValue(float aValue)
	{
		background.sizeDelta = new Vector2(background.sizeDelta.x, aValue);
		if((int)aValue > 1250 && content.activeSelf == false)
		{
			content.SetActive(true);
		}
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

    public void Play()
    {
        HidePanel("OnHideDonePlay");
    }

    void OnHideDonePlay()
    {
        EventManager.StartGame(myLevelData);
        CanvasManager.OpenPanel(PanelEnum.InGamePanel, panelType, new GamePanelData{ myLevelData = myLevelData});
        CanvasManager.ClosePanel(PanelEnum.LevelSelectPanel);
    }

    public void Cancle()
    {
		iTween.ValueTo(background.gameObject, 
			iTween.Hash(
				"from", 1300, 
				"to", 660, 
				"time", 0.5f, 
				"onupdate", "UpdateScrollHide", 
				"onupdatetarget", gameObject,
				"oncomplete", "HidePanelScrollDone",
				"oncompletetarget", gameObject));


	}

	void HidePanelScrollDone()
	{
		HidePanel("OnHideDoneCancle");
	}

	void UpdateScrollHide(float aValue)
	{
		background.sizeDelta = new Vector2(background.sizeDelta.x, aValue);
		if ((int)aValue < 1250 && content.activeSelf == true)
		{
			content.SetActive(false);
		}
	}


	void OnHideDoneCancle()
    {
        CanvasManager.ClosePanel(panelType);
    }

    public override void SetPanelData(PanelData aPanelData)
    {
        LevelInfoPanelData pd = aPanelData as LevelInfoPanelData;
        if (pd == null)
            return;
        myLevelData = pd.myLevelData;
    }

}

public class LevelInfoPanelData : PanelData
{
    public LevelData myLevelData;
}
