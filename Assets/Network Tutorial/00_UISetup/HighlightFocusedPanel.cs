using ECSish;
using System.Linq;

public class HighlightFocusedPanel : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetAllEntities<SplitScreenHighlightPanel>())
        {
            var focusPanel = GetAllEntities<SplitScreenPanel>().Where(e => e.Item1.isFocused).FirstOrDefault();
            if (focusPanel != null)
            {
                entity.Item1.anchorMin = focusPanel.Item1.anchorMin;
                entity.Item1.anchorMax = focusPanel.Item1.anchorMax;
                SetRectTransform(entity.Item1);
            }
        }
    }

    private void SetRectTransform(SplitScreenHighlightPanel highlightPanel)
    {
        highlightPanel.rectTransform.anchorMin = highlightPanel.anchorMin;
        highlightPanel.rectTransform.anchorMax = highlightPanel.anchorMax;
        highlightPanel.rectTransform.offsetMin = highlightPanel.offsetMin;
        highlightPanel.rectTransform.offsetMax = -highlightPanel.offsetMax;
    }
}
