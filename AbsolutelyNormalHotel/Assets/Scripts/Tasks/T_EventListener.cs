// Created 27.05.2023 by Krista Plagemann //

// Simple event listener for the state of a task. //

using UnityEngine;
using UnityEngine.Events;

public class P_EventListener : MonoBehaviour
{
    [SerializeField] private TaskEvent _taskEvent;           // the task this refers to

    [Tooltip("Event gets fired when we start the task.")]
    public UnityEvent OnStarted = new();

    [Tooltip("Event gets fired when we finish the task.")]
    public UnityEvent OnFinished = new();


    /// Add or remove to the task delegates. ///
    private void OnEnable()
    {
        _taskEvent.OnStarted += OnTaskStart;
        _taskEvent.OnFinished += OnTaskFinish;
    }


    private void OnDisable()
    {
        _taskEvent.OnStarted -= OnTaskStart;
        _taskEvent.OnFinished -= OnTaskFinish;
    }

    /// When the task state changes, we invoke events based on that ///
    private void OnTaskStart(TaskEvent taskEvent, bool state)
    {
        if (state)
            OnStarted?.Invoke();
    }

    /// When the task state changes, we invoke events based on that ///
    private void OnTaskFinish(TaskEvent taskEvent, bool state)
    {
        if (state)
            OnFinished?.Invoke();
        _taskEvent.NextTask?.StartTask();
    }

}
