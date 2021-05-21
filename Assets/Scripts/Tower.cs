using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour {
    [SerializeField] private float _maxHP;
    [SerializeField] private bool _isEnemy;

    public bool IsEnemy => _isEnemy;


    private float _curHP;
    private void Awake() {
        _curHP = _maxHP;
    }

    public void GetDamage(float value) {
        _curHP -= value;
        
        if(_curHP <= 0) Destroy(gameObject);
    }
}