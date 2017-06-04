using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsoleManager
{
    private static DebugConsoleManager _instance;

    public static DebugConsoleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DebugConsoleManager();
            }
            return _instance;
        }
    }

    public bool IsUpdate { get; private set; }
    public string DebugConsoleText { get; set; }

    public void UpdateUI(Text debugConsole)
    {
        debugConsole.text = DebugConsoleText;
        Canvas.ForceUpdateCanvases();
        IsUpdate = false;
    }

    public void AddLineWithTime(string line)
    {
        DebugConsoleText += String.Format("{0} - {1} \r\n", line, DateTime.Now.TimeOfDay);
        IsUpdate = true;
    }

    public void AddLine(string line, double doubleVal)
    {
        DebugConsoleText += String.Format("{0} - {1} \r\n", line, doubleVal);
        IsUpdate = true;
    }
    public void AddLine(string line, string stringVal)
    {
        DebugConsoleText += String.Format("{0} - {1} \r\n", line, stringVal);
        IsUpdate = true;
    }
    public void AddLine(string line, bool boolVal)
    {
        DebugConsoleText += String.Format("{0} - {1} \r\n", line, boolVal);
        IsUpdate = true;
    }
}