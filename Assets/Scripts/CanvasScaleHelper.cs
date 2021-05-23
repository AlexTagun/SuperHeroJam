using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasScaleHelper : MonoBehaviour
{
    private void Awake() {
        var canvasScaler = GetComponent<CanvasScaler>();
        var ratio = Screen.width / (float) Screen.height;
        canvasScaler.matchWidthOrHeight = ratio > 0.5625 ? 1 : 0;
    }
}
