using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectPanel : Panel {

    public LevelButtonEntry levelButtonPrfab;
    public RectTransform levelParent;
    public ScrollRect scrollRect;
    List<LevelButtonEntry> buttons = new List<LevelButtonEntry>();
	bool haveSetPosition;
    private void Awake()
    {
        panelType = PanelEnum.LevelSelectPanel;
    }

    private void OnEnable()
    {
		haveSetPosition = false;
		//Debug.Log("0%3 = " + (0 % 3));
		//Debug.Log("1%3 = " + (1 % 3));
		//Debug.Log("2%3 = " + (2 % 3));
		//Debug.Log("3%3 = " + (3 % 3));
		//Debug.Log("4%3 = " + (4 % 3));
		//Debug.Log("5%3 = " + (5 % 3));

		ClearLevelButtons();
        List<LevelData> levels = GameManager.Levels.Levels;
        for(int i = 0; i < levels.Count; ++i)
        {
            var entry = Instantiate(levelButtonPrfab, levelParent);
			levelParent.sizeDelta += new Vector2(0, entry.GetComponent<LayoutElement>().preferredHeight + 50);
            entry.SetUp(levels[i]);
			//entry.transform.SetAsFirstSibling();           

			RectTransform rect = entry.GetComponent<RectTransform>();
			float x = (i % 2) == 0 ? -250 : 250;
            rect.localPosition = new Vector3(x, i * 400 + 50, 0f);
			entry.gameObject.SetActive(true);
            buttons.Add(entry);
        }

        scrollRect.verticalNormalizedPosition = 0f;

    }

	private void Update()
	{
		if (haveSetPosition == false)
		{
			for (int i = 0; i < buttons.Count; ++i)
			{
				var entry = buttons[i];

				RectTransform rect = entry.GetComponent<RectTransform>();
				float x = (i % 2) == 0 ? -250 : 250;
				rect.localPosition = new Vector3(x, i * 400 + 50, 0f);
			}
			haveSetPosition = true;
		}
	}

	private void ClearLevelButtons()
    {
        for(int i = 0; i < buttons.Count; ++i)
        {
            Destroy(buttons[i].gameObject);
        }
        buttons.Clear();
    }
}
