using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : Panel {

    private void Awake()
    {
        panelType = PanelEnum.MainMenuPanel;
    }

    public void StartGame()
    {
        CanvasManager.OpenPanel(PanelEnum.LevelSelectPanel, panelType);
    }
}
