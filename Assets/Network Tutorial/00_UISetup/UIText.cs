using ECSish;
using TMPro;

public class UIText : MonoBehaviourComponentData
{
    public TextMeshProUGUI textMesh;

    private void OnValidate()
    {
        if (!textMesh) textMesh = GetComponentInChildren<TextMeshProUGUI>();
    }
}