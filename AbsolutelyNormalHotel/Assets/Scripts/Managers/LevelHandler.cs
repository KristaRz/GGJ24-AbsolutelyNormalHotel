using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    public static LevelHandler Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private int _chickensKilled = 0;

    public void CountChickenKill()
    {
        _chickensKilled++;
    }

    public int GetFinalChickenKillCount() => _chickensKilled;
}
