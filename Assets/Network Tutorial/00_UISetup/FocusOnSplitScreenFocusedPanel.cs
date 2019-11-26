using ECSish;
using System.Linq;

public class FocusOnSplitScreenFocusedPanel : MonoBehaviourSystem
{
    private void Update()
    {
        foreach (var entity in GetEntities<SplitScreenFocusedPanel>())
        {
            var focusedPanel = entity.Item1;
            var index = focusedPanel.index;
            var panels = GetAllEntities<SplitScreenPanel>();
            focusedPanel.count = panels.Count();

            foreach (var focusPanel in panels)
                focusPanel.Item1.isFocused = false;

            if (index < focusedPanel.count)
                panels.ElementAt(index).Item1.isFocused = true;
            else if (focusedPanel.count >= 1)
                panels.ElementAt(0).Item1.isFocused = true;
        }
    }
}
