using System;
using System.Collections;
using UnityEngine;

public class MainThread : MonoBehaviour
{
    private static MainThread instance;
    public static MainThread Instance => FindInstance();
    public Queue actionQueue = Queue.Synchronized(new Queue());

    private static MainThread FindInstance()
    {
        if (instance) return instance;

        instance = FindObjectOfType<MainThread>();
        if (instance) return instance;

        return null;
    }

    private void Awake()
    {
        instance = FindInstance();
    }

    private void Update()
    {
        while (actionQueue.Count > 0)
            ((Action)actionQueue.Dequeue()).Invoke();
    }

    public static void Run(Action action)
        => Instance?.actionQueue.Enqueue(action);

}