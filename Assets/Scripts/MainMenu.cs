using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    [SerializeField] private GameObject bot;
    [SerializeField] private GameObject lockImg;
    [SerializeField] private Button playPvE;
    [SerializeField] private Button playPvP;

    private void Awake() {
        playPvE.onClick.AddListener(() => SceneManager.LoadScene("Gameplay_PvE"));
        playPvP.onClick.AddListener(() => SceneManager.LoadScene("Gameplay_PvP"));
    }
}