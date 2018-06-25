using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnimation : MonoBehaviour {

	public GameObject objectToMove;
	public RectTransform Scroll;
	public GameObject content;
	public float targetHeight;

	private void OnEnable()
	{
		ShowPanel();
	}

	public void ShowPanel()
	{
		objectToMove.transform.localPosition = new Vector3(0f, -2000f, 0f);
		Hashtable hash = new Hashtable();
		hash.Add("position", new Vector3(0f, 0f, 0f));
		hash.Add("time", 1f);
		hash.Add("islocal", true);
		hash.Add("oncomplete", "OnShowComplete");
		hash.Add("oncompletetarget", gameObject);

		iTween.MoveTo(objectToMove, hash);
	}

	public void HidePanel(string aHideFunction)
	{
		content.SetActive(false);
		Hashtable hash = new Hashtable();
		hash.Add("from", targetHeight);
		hash.Add("to", 660);
		hash.Add("time", 0.5f);
		hash.Add("onupdate", "UpdateScrollValue");
		hash.Add("onupdatetarget", gameObject);
		hash.Add("oncomplete", "ScrollRolledUp");
		hash.Add("oncompletetarget", gameObject);
		hash.Add("oncompleteparams", aHideFunction);
		iTween.ValueTo(Scroll.gameObject, hash);
	}

	void OnShowComplete()
	{
		Hashtable hash = new Hashtable();
		hash.Add("from", 660);
		hash.Add("to", targetHeight);
		hash.Add("time", 0.5f);
		hash.Add("onupdate", "UpdateScrollValue");
		hash.Add("onupdatetarget", gameObject);
		hash.Add("oncomplete", "ScrollRolledDown");
		hash.Add("oncompletetarget", gameObject);
		iTween.ValueTo(Scroll.gameObject, hash);
	}

	void UpdateScrollValue(float aValue)
	{
		Scroll.sizeDelta = new Vector2(Scroll.sizeDelta.x, aValue);
	}

	void ScrollRolledDown()
	{
		content.SetActive(true);
	}

	void ScrollRolledUp(string aHideFunction)
	{
		Hashtable hash = new Hashtable();
		hash.Add("position", new Vector3(0f, -2000f, 0f));
		hash.Add("time", 1f);
		hash.Add("islocal", true);
		hash.Add("oncomplete", aHideFunction);
		hash.Add("oncompletetarget", gameObject);
		iTween.MoveTo(objectToMove, hash);
	}

}
