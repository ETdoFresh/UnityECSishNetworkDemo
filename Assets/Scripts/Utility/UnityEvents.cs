using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class UnityEvent : UnityEngine.Events.UnityEvent { }
[Serializable] public class UnityEventString : UnityEvent<string> { }
[Serializable] public class UnityEventInt : UnityEvent<int> { }

public static class UnityEventExtensions
{
    public static void AddEditorListener(this UnityEngine.Events.UnityEvent unityEvent, UnityAction call)
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
            UnityEditor.Events.UnityEventTools.AddPersistentListener(unityEvent, call);
        else
            unityEvent.AddListener(call);
#else
        unityEvent.AddListener(call);
#endif
    }

    public static void RemoveEditorListener(this UnityEngine.Events.UnityEvent unityEvent, UnityAction call)
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(unityEvent, call);
        else
            unityEvent.RemoveListener(call);
#else
        unityEvent.RemoveListener(call);
#endif
    }

    public static void AddEditorListener<T0>(this UnityEvent<T0> unityEvent, UnityAction<T0> call)
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
            UnityEditor.Events.UnityEventTools.AddPersistentListener(unityEvent, call);
        else
            unityEvent.AddListener(call);
#else
        unityEvent.AddListener(call);
#endif
    }

    public static void RemoveEditorListener<T0>(this UnityEvent<T0> unityEvent, UnityAction<T0> call)
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(unityEvent, call);
        else
            unityEvent.RemoveListener(call);
#else
        unityEvent.RemoveListener(call);
#endif
    }

    public static void AddEditorListener<T0, T1>(this UnityEvent<T0, T1> unityEvent, UnityAction<T0, T1> call)
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
            UnityEditor.Events.UnityEventTools.AddPersistentListener(unityEvent, call);
        else
            unityEvent.AddListener(call);
#else
        unityEvent.AddListener(call);
#endif
    }

    public static void RemoveEditorListener<T0, T1>(this UnityEvent<T0, T1> unityEvent, UnityAction<T0, T1> call)
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(unityEvent, call);
        else
            unityEvent.RemoveListener(call);
#else
        unityEvent.RemoveListener(call);
#endif
    }

    public static void AddEditorListener<T0, T1, T2>(this UnityEvent<T0, T1, T2> unityEvent, UnityAction<T0, T1, T2> call)
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
            UnityEditor.Events.UnityEventTools.AddPersistentListener(unityEvent, call);
        else
            unityEvent.AddListener(call);
#else
        unityEvent.AddListener(call);
#endif
    }

    public static void RemoveEditorListener<T0, T1, T2>(this UnityEvent<T0, T1, T2> unityEvent, UnityAction<T0, T1, T2> call)
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(unityEvent, call);
        else
            unityEvent.RemoveListener(call);
#else
        unityEvent.RemoveListener(call);
#endif
    }

    public static void AddEditorListener<T0, T1, T2, T3>(this UnityEvent<T0, T1, T2, T3> unityEvent, UnityAction<T0, T1, T2, T3> call)
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
            UnityEditor.Events.UnityEventTools.AddPersistentListener(unityEvent, call);
        else
            unityEvent.AddListener(call);
#else
        unityEvent.AddListener(call);
#endif
    }

    public static void RemoveEditorListener<T0, T1, T2, T3>(this UnityEvent<T0, T1, T2, T3> unityEvent, UnityAction<T0, T1, T2, T3> call)
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(unityEvent, call);
        else
            unityEvent.RemoveListener(call);
#else
        unityEvent.RemoveListener(call);
#endif
    }

    public static void AddStringEditorListener(this UnityEngine.Events.UnityEvent unityEvent, UnityAction<string> call, string argument)
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
            UnityEditor.Events.UnityEventTools.AddStringPersistentListener(unityEvent, call, argument);
        else
            unityEvent.AddListener(() => call.Invoke(argument));
#else
        unityEvent.AddListener(() => call.Invoke(argument));
#endif
    }

    public static void RemoveStringEditorListener(this UnityEngine.Events.UnityEvent unityEvent, UnityAction<string> call)
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(unityEvent, call);
        else
            unityEvent.RemoveListener(() => call.Invoke("")); // TODO: Fix this, this is not correct. This is making a new event/delegate to remove
#else
        unityEvent.RemoveListener(() => call.Invoke(""));
#endif
    }
}