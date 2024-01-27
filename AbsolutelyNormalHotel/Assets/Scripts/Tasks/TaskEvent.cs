// Created 27.05.2023 by Krista Plagemann //

// Events for a task. //

using System;
using UnityEngine;


[CreateAssetMenu(fileName = "TaskEvent", menuName = "ANH/TaskEvent", order = 0)]
public class TaskEvent : ScriptableObject
{
    [field: SerializeField, Tooltip("Name of this pickup to check if owned.")]
    public string TaskName { get; private set; }

    [field: SerializeField, Tooltip("Icon for the inventory display.")]
    public Sprite Icon { get; private set; }

    [field: SerializeField, Tooltip("Next task after this.")]
    public TaskEvent NextTask { get; private set; }

    /// <summary>
    /// Triggers with when task is started.
    /// </summary>
    public event Action<TaskEvent, bool> OnStarted = delegate { };

    /// <summary>
    /// Triggers with when task is finished.
    /// </summary>
    public event Action<TaskEvent, bool> OnFinished = delegate { };


    public void FinishTask()
    {
        OnFinished?.Invoke(this, true);
    }
    
    public void StartTask()
    {
        OnStarted?.Invoke(this, true);
    }

}
