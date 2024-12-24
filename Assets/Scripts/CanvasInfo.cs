using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class CanvasInfo : MonoBehaviour {
    [SerializeField, FoldoutGroup("Dependencies")]
    private Canvas ReferenceCanvas;
    [SerializeField, FoldoutGroup("Status"), ReadOnly]
    private float CanvasScaleModifier;

    private void Awake() {
        CanvasScaleModifier = ReferenceCanvas.scaleFactor;
    }
}
