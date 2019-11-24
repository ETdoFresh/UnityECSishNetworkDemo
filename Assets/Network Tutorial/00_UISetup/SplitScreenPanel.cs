using ECSish;
using UnityEngine;

public class SplitScreenPanel : MonoBehaviourComponentData
{
    public bool isFocused;
    public RectTransform rectTransform;
    public Vector2 anchorMin;
    public Vector2 anchorMax;
    public Vector2 offsetMin;
    public Vector2 offsetMax;

    private void OnValidate()
    {
        if (!rectTransform) rectTransform = GetComponent<RectTransform>();
    }
}
