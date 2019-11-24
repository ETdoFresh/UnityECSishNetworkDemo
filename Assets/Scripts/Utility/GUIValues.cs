using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIValues : MonoBehaviour
{
    public static Dictionary<string, object> enteredData;

    private void Awake()
    {
        if (enteredData == null)
            enteredData = new Dictionary<string, object>();
    }

    public static T GetUserSpecifiedOrExisting<T>(string key, T currentValue)
    {
        if (enteredData == null)
            enteredData = new Dictionary<string, object>();

        if (enteredData.ContainsKey(key))
            return (T)enteredData[key];

        return currentValue;
    }

    public void SetURLData(TMP_InputField inputField) => SetData("url", inputField.text);
    public void SetURLData(string value) => SetData("url", value);
    public void SetNameData(TMP_InputField inputField) => SetData("name", inputField.text);

    public void SetData(string key, object value)
    {
        if (enteredData.ContainsKey(key))
            enteredData[key] = value;
        else
            enteredData.Add(key, value);
    }
}
