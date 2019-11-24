using UnityEngine;

public class ToggleConsole : MonoBehaviour
{
    public GameObject console;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
            console.SetActive(!console.activeSelf);
    }
}
