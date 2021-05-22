using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager {
    public static bool IsDragging;

    public static Action OnGlueWall;

    public static void HandleOnGlueWall() {
        OnGlueWall?.Invoke();
    }
}