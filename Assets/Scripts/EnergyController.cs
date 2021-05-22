using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergyController : MonoBehaviour {
    [SerializeField] private int _maxEnergy;
    [SerializeField] private int _startEnergy;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private float gainTime;
    [SerializeField] private int gainNum;

    private int _curEnergy;
    public int CurEnergy => _curEnergy;

    private void Start() {
        _curEnergy = _startEnergy;

        UpdateText();

        StartCoroutine(GainCoroutine());
    }

    private IEnumerator GainCoroutine() {
        while (true) {
            yield return new WaitForSeconds(gainTime);

            AddEnergy(gainNum);
        }
    }

    private void UpdateText() {
        energyText.text = $"{_curEnergy}/{_maxEnergy}";
    }

    public void AddEnergy(int value) {
        _curEnergy += value;
        if (_curEnergy > _maxEnergy) _curEnergy = _maxEnergy;
        UpdateText();
    }

    public void SpendEnergy(int value) {
        _curEnergy -= value;
        UpdateText();
    }
}