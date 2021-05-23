using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour {
    [SerializeField] private WinWindow winWindow;
    [SerializeField] private LoseWindow loseWindow;

    private void Start() {
        Time.timeScale = 1;
        EventManager.OnEndGame += OnEndGame;
    }

    private void OnDestroy() {
        EventManager.OnEndGame -= OnEndGame;
    }

    private void OnEndGame(bool value) {
        Time.timeScale = 0;
        if (value) {
            winWindow.gameObject.SetActive(true);
        } else {
            loseWindow.gameObject.SetActive(true);
        }
        
    }
}