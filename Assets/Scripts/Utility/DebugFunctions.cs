using UnityEngine;

public class DebugFunctions : MonoBehaviour
{
    public void DebugLog(string message) => Debug.Log(message);
    public void DebugWarn(string message) => Debug.LogWarning(message);
    public void DebugError(string message) => Debug.LogError(message);
}
