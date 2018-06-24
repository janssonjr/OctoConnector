using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    public List<Panel> panels = new List<Panel>();

    static CanvasManager instance;
    

    private void Awake()
    {
        instance = this;
    }

    public CanvasManager Instance
    {
        get
        {
            if (instance == null)
                instance = this;
            return instance;
        }
    }

    private void OnEnable()
    {
        instance = this;
        //EventManager.onStateEvent += OnStateEvent;
    }

    private void OnDisable()
    {
        //EventManager.onStateEvent -= OnStateEvent;
    }

    public static void OpenPanel(PanelEnum aPanelToOpen, PanelData panelData = null)
    {
        instance.panels[(int)aPanelToOpen].SetPanelData(panelData);
        instance.panels[(int)aPanelToOpen].gameObject.SetActive(true);
    }

    public static void OpenPanel(PanelEnum aPanelToOpen, PanelEnum aPanelToClose, PanelData panelData = null)
    {
        instance.panels[(int)aPanelToClose].gameObject.SetActive(false);
        instance.panels[(int)aPanelToOpen].SetPanelData(panelData);
        instance.panels[(int)aPanelToOpen].gameObject.SetActive(true);
    }

    public static void OpenPanel(PanelEnum aPanelToOpen, PanelEnum[] aPanelsToClose, PanelData panelData = null)
    {
        instance.panels[(int)aPanelToOpen].SetPanelData(panelData);
        instance.panels[(int)aPanelToOpen].gameObject.SetActive(true);
        for(int i = 0; i < aPanelsToClose.Length; ++i)
        {
            instance.panels[(int)aPanelsToClose[i]].gameObject.SetActive(false);
        }
    }

    public static void ClosePanel(PanelEnum aPanelToClose)
    {
        instance.panels[(int)aPanelToClose].gameObject.SetActive(false);
    }
}
