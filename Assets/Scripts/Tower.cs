using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour {
    [SerializeField] private float _maxHP;
    [SerializeField] private bool _isEnemy;
    [SerializeField] private Image hpBar;
    [SerializeField] private TextMeshProUGUI _text;

    public bool IsEnemy => _isEnemy;

    private float _curHP;

    private void Awake() {
        _curHP = _maxHP;
        UpdateHpBar();
    }

    public void GetDamage(float value) {
        _curHP -= value;
        UpdateHpBar();
        if (_curHP <= 0) {
            EventManager.HandleOnEndGame(_isEnemy);
            Destroy(gameObject);
        }
    }

    private void UpdateHpBar() {
        if(hpBar == null) return;
        hpBar.fillAmount = _curHP / _maxHP;
        _text.text = Mathf.RoundToInt(_curHP).ToString();
    }
}