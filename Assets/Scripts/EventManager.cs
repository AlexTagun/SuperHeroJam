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
    
    public static Action OnGlueBall;
    
    public static void HandleOnGlueBall() {
        OnGlueBall?.Invoke();
    }
    
    public static Action<int> OnGlueLance;
    
    public static void HandleOnGlueLance(int index) {
        OnGlueLance?.Invoke(index);
    }
    
    public static Action<bool> OnEndGame;
    
    public static void HandleOnEndGame(bool value) {
        OnEndGame?.Invoke(value);
    }
}