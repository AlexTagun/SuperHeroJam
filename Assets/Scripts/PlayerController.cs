using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    [SerializeField] private Transform[] projectileStartPoints;
    [SerializeField] private Transform[] heroPoints;
    [SerializeField] private Transform heroPrefab;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    private int _curIndex = 1;
    private Transform _curProjectileStartPoint;
    private Transform _curHeroPoint;

    public Transform CurProjectileStartPoint => _curProjectileStartPoint;
    private void Awake() {
        UpdateState();
        
        leftButton.onClick.AddListener(OnLeftButtonClick);
        rightButton.onClick.AddListener(OnRightButtonClick);
        
    }

    private void OnLeftButtonClick() {
        _curIndex--;
        if (_curIndex < 0) _curIndex = 2;
        UpdateState();
    }
    
    private void OnRightButtonClick() {
        _curIndex++;
        if (_curIndex > 2) _curIndex = 0;
        UpdateState();
    }

    private void UpdateState() {
        _curProjectileStartPoint = projectileStartPoints[_curIndex];
        _curHeroPoint = heroPoints[_curIndex];

        heroPrefab.position = _curHeroPoint.position;
    }
}