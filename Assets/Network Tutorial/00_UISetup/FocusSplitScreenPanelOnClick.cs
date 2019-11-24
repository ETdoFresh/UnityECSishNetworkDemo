using ECSish;
using UnityEngine;

public class FocusSplitScreenPanelOnClick : MonoBehaviourSystem
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var entity = GetEntity<SplitScreenFocusedPanel>();
            if (entity == null) return;

            var focusedPanel = entity.Item1;
            var numberOfSplitScreens = focusedPanel.count;
            var pos = Input.mousePosition;
            pos.x /= Screen.width;
            pos.y /= Screen.height;
            if (numberOfSplitScreens == 2 & pos.x <= 0.5f) focusedPanel.index = 0;
            if (numberOfSplitScreens == 2 & pos.x > 0.5f) focusedPanel.index = 1;

            if (numberOfSplitScreens == 3 & pos.x <= 0.5f) focusedPanel.index = 0;
            if (numberOfSplitScreens == 3 & pos.x > 0.5f && pos.y > 0.5f) focusedPanel.index = 1;
            if (numberOfSplitScreens == 3 & pos.x > 0.5f && pos.y <= 0.5f) focusedPanel.index = 2;

            if (numberOfSplitScreens == 4 & pos.x <= 0.5f && pos.y > 0.5f) focusedPanel.index = 0;
            if (numberOfSplitScreens == 4 & pos.x > 0.5f && pos.y > 0.5f) focusedPanel.index = 1;
            if (numberOfSplitScreens == 4 & pos.x <= 0.5f && pos.y <= 0.5f) focusedPanel.index = 2;
            if (numberOfSplitScreens == 4 & pos.x > 0.5f && pos.y <= 0.5f) focusedPanel.index = 3;

        }
    }
}