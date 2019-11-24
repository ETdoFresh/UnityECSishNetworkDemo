using ECSish;

public class ShowSplitScreenOnTextUI : MonoBehaviourSystem
{
    private void Update()
    {
        foreach(var entity in GetEntities<SplitScreenInput, UIText>())
        {
            var input = entity.Item1;
            var textMesh = entity.Item2.textMesh;

            var output = "";
            if (input.horizontal != 0) output += "Horizontal: " + input.horizontal;
            if (input.vertical!= 0) output += (output == "" ? "" : "\n") + "Vertical: " +input.vertical;
            if (input.jumpHold) output += (output == "" ? "" : "\n") + "Jump";
            if (output == "") output = "!TEST!";
            
            textMesh.text = output;
        }
    }
}
