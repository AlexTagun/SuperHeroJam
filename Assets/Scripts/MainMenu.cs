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
    [SerializeField] private GameObject komixContainer;
    [SerializeField] private Button komixButton;
    [SerializeField] private Button tutorButton;
    [SerializeField] private GameObject tutorContainer;

    private void Awake() {
        tutorButton.onClick.AddListener((() => tutorContainer.SetActive(false)));
        playPvE.onClick.AddListener(() => komixContainer.SetActive(true));
        komixButton.onClick.AddListener(() => SceneManager.LoadScene("Gameplay_PvE"));
        playPvP.onClick.AddListener(() => SceneManager.LoadScene("Gameplay_PvP"));
    }
}