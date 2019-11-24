using ECSish;
using TMPro;

public class UIConsole : MonoBehaviourComponentData
{
    public TextMeshProUGUI textMesh;

    private void OnValidate()
    {
        if (!textMesh) textMesh = GetComponent<TextMeshProUGUI>();
    }
}
