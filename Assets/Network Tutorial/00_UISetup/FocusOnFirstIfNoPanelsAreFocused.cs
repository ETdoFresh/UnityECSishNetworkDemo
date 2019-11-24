using ECSish;
using System.Linq;

public class FocusOnFirstIfNoPanelsAreFocused : MonoBehaviourSystem
{
    private void Update()
    {
        var entities = GetEntities<SplitScreenPanel>();
        var focusedPanels = entities.Where(e => e.Item1.isFocused);

        if (focusedPanels.Count() == 0 && entities.Count() > 0)
            entities.ElementAt(0).Item1.isFocused = true;
    }
}
