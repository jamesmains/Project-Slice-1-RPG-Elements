using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class DebugPrinter : MonoBehaviour {
    [SerializeField] [BoxGroup("Dependencies")]
    private TextMeshProUGUI DebugText;

    [SerializeField] [BoxGroup("Status")] [ReadOnly]
    private List<string> Status;
    
    public static DebugPrinter Singleton;

    private void Awake() {
        if (Singleton != null) {
            Destroy(gameObject);
        }
        else {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void LateUpdate() {
        DebugText.text = "";
        foreach (var t in Status) {
            DebugText.text += $"{t}<br>";
        }
        Status.Clear();
    }

    public void Print(string text) {
        Status.Add(text);
    }
}