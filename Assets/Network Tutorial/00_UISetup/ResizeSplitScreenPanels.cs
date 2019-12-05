using ECSish;
using System.Linq;

public class ResizeSplitScreenPanels : MonoBehaviourSystem
{
    private void Update()
    {
        var entities = GetAllEntities<SplitScreenPanel>();

        if (entities.Count() == 1)
        {
            var panel = entities.ElementAt(0).Item1;
            panel.anchorMin.x = 0; panel.anchorMin.y = 0;
            panel.anchorMax.x = 1; panel.anchorMax.y = 1;
            SetRectTransform(panel, 0, 0, 1, 1);
        }

        else if (entities.Count() == 2)
        {
            var panel = entities.ElementAt(0).Item1;
            SetRectTransform(panel, 0, 0, 0.5f, 1);

            panel = entities.ElementAt(1).Item1;
            SetRectTransform(panel, 0.5f, 0, 1, 1);
        }

        else if (entities.Count() == 3)
        {
            var panel = entities.ElementAt(0).Item1;
            SetRectTransform(panel, 0, 0, 0.5f, 1);

            panel = entities.ElementAt(1).Item1;
            SetRectTransform(panel, 0.5f, 0.5f, 1, 1);
            
            panel = entities.ElementAt(2).Item1;
            SetRectTransform(panel, 0.5f, 0, 1, 0.5f);
        }

        else if (entities.Count() == 4)
        {
            var panel = entities.ElementAt(0).Item1;
            SetRectTransform(panel, 0, 0.5f, 0.5f, 1);

            panel = entities.ElementAt(1).Item1;
            SetRectTransform(panel, 0.5f, 0.5f, 1, 1);

            panel = entities.ElementAt(2).Item1;
            SetRectTransform(panel, 0, 0, 0.5f, 0.5f);

            panel = entities.ElementAt(3).Item1;
            SetRectTransform(panel, 0.5f, 0, 1, 0.5f);
        }
    }

    private void SetRectTransform(SplitScreenPanel focusPanel, float anchorMinX, float anchorMinY, float anchorMaxX, float anchorMaxY)
    {
        focusPanel.anchorMin.x = anchorMinX; focusPanel.anchorMin.y = anchorMinY;
        focusPanel.anchorMax.x = anchorMaxX; focusPanel.anchorMax.y = anchorMaxY;
        focusPanel.rectTransform.anchorMin = focusPanel.anchorMin;
        focusPanel.rectTransform.anchorMax = focusPanel.anchorMax;
        focusPanel.rectTransform.offsetMin = focusPanel.offsetMin;
        focusPanel.rectTransform.offsetMax = -focusPanel.offsetMax;
    }
}
