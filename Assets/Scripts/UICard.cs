﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ElementType {
    Fire,
    Water,
    Earth
}

public enum FormType {
    Base,
    Ball,
    Wall,
    Lance
}

public class UICard : MonoBehaviour {
    [SerializeField] private ElementType _element;
    [SerializeField] private FormType _form;
    [SerializeField] private int _rank;
    public int EnergyCost;
    public bool IsLocked;
    [SerializeField] private float lockTime;
    [SerializeField] private GameObject lockContainer;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private TextMeshProUGUI _energyText;
    [SerializeField] private GameObject[] _stars;

    private void Awake() {
        EnergyCost = _rank + (_form == FormType.Base ? 0 : 1);
        _energyText.text = EnergyCost.ToString();
        for (int i = 0; i < _rank; i++) {
            _stars[i].SetActive(true);
        }
    }

    public ElementType Element => _element;
    public int Rank => _rank;
    public Projectile ProjectilePrefab => _projectilePrefab;

    public void Lock() {
        StartCoroutine(LockCoroutine());
    }

    private IEnumerator LockCoroutine() {
        IsLocked = true;
        lockContainer.SetActive(true);

        yield return new WaitForSeconds(lockTime);
        
        lockContainer.SetActive(false);
        IsLocked = false;
    }
}