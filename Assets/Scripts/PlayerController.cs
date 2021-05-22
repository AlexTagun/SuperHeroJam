using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public Transform[] projectileStartPoints;
    [SerializeField] private Transform[] heroPoints;
    [SerializeField] private Transform heroPrefab;

    private int _curIndex = 1;
    private Transform _curHeroPoint;

    private void Awake() {
        UpdateState(1);
        
    }

    // private void OnLeftButtonClick() {
    //     _curIndex--;
    //     if (_curIndex < 0) _curIndex = 2;
    //     UpdateState();
    // }
    //
    // private void OnRightButtonClick() {
    //     _curIndex++;
    //     if (_curIndex > 2) _curIndex = 0;
    //     UpdateState();
    // }

    public void UpdateState(int lineIndex) {
        _curHeroPoint = heroPoints[lineIndex];

        heroPrefab.position = _curHeroPoint.position;
    }
}