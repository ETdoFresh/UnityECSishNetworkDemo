using ECSish;
using UnityEngine;

public class SplitScreenCamera : MonoBehaviourComponentData
{
    public new Camera camera;
    public Rect rect;

    private void OnValidate()
    {
        if (!camera) camera = GetComponent<Camera>();
    }
}