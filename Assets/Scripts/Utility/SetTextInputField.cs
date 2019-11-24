using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetTextInputField : MonoBehaviour
{
    public TMP_InputField inputField;

    private void OnValidate()
    {
        if (!inputField) inputField = GetComponent<TMP_InputField>();
    }

    private void OnEnable()
    {
        inputField.text = GUIValues.GetUserSpecifiedOrExisting(name, inputField.text);
    }

    public void ChangeText(string text)
    {
        inputField.text = text;
    }

    public void ChangeGuestText()
    {
        inputField.text = "Guest" + Random.Range(0,10000).ToString("0000");
    }
}
