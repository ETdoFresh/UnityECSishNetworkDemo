using ECSish;
using TMPro;
using UnityEngine;

public class InputFieldEventListener : MonoBehaviour
{
    public bool wasFocused;
    public TMP_InputField inputField;

    private void OnValidate()
    {
        if (!inputField) inputField = GetComponent<TMP_InputField>();
    }

    private void Update()
    {
        if (!inputField) return;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            if (inputField.isFocused || wasFocused)
                CreateSubmitEvent();

        wasFocused = inputField.isFocused;
    }

    public void CreateSubmitEvent()
    {
        var text = inputField.text;
        if (text.Trim() != "")
        {
            ECSEvent.Add(() =>
            {
                var submitEvent = gameObject.AddComponent<InputFieldSubmitEvent>();
                submitEvent.text = text;
                return submitEvent;
            });
        }
        inputField.text = "";
        inputField.ActivateInputField();
    }
}
