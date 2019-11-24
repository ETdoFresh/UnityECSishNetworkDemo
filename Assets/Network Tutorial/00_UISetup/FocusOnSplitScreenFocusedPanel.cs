using ECSish;
using System.Linq;

public class FocusOnSplitScreenFocusedPanel : MonoBehaviourSystem
{
    private void Update()
    {
        var entity = GetEntity<SplitScreenFocusedPanel>();
        if (entity == null) return;

        var focusedPanel = entity.Item1;
        var index = focusedPanel.index;
        var focusPanels = GetEntities<SplitScreenPanel>();
        focusedPanel.count = focusPanels.Count();

        foreach (var focusPanel in focusPanels)
            focusPanel.Item1.isFocused = false;

        if (index < focusedPanel.count)
            focusPanels.ElementAt(index).Item1.isFocused = true;
        else if (focusedPanel.count >= 1)
            focusPanels.ElementAt(0).Item1.isFocused = true;
    }
}
