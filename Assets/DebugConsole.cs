using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugConsole : MonoBehaviour
{

    Dictionary<string, string> debugLogs = new Dictionary<string, string>();
    public TMP_Text display;

    private void Update()
    {
        Debug.Log("Time: " + Time.time);


        //Camera camera = Camera.main;
        //transform.LookAt(transform.position + camera.transform.rotation * Vector3.back, camera.transform.rotation * Vector3.up);
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Log)
        {
            string[] splitstring = logString.Split(char.Parse(":"));
            string debugKey = splitstring[0];
            string debugValue = splitstring.Length > 1 ? splitstring[1] : "";

            if (debugLogs.ContainsKey(debugKey))
                debugLogs[debugKey] = debugValue;
            else
                debugLogs.Add(debugKey, debugValue);
        }

        string displayText = "";
        foreach (KeyValuePair<string, string> log in debugLogs)
        {
            if (log.Value == "")
                displayText += log.Key + "\n";
            else
                displayText += log.Key + ": " + log.Value + "\n";
        }
        display.text = displayText;
    }
}
