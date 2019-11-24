using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BackupEventSystem : MonoBehaviour
{
    public GameObject backupEventSystem;

    private void OnValidate()
    {
        if (!backupEventSystem)
            backupEventSystem = GetComponentInChildren<EventSystem>()?.gameObject;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += CheckForEventSystem;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= CheckForEventSystem;
    }

    private void CheckForEventSystem(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (loadSceneMode == LoadSceneMode.Single)
            if (!scene.FindObjectOfType<EventSystem>())
                backupEventSystem.SetActive(true);
            else
                backupEventSystem.SetActive(false);
    }
}
