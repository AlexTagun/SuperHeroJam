using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyController : MonoBehaviour {
    [SerializeField] private int _maxEnergy;
    [SerializeField] private int _startEnergy;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private Image energyBar;
    [SerializeField] private Image glueBar;
    [SerializeField] private float gainTime;
    [SerializeField] private int gainNum;

    private int _curEnergy;
    public int CurEnergy => _curEnergy;

    private bool _isGlueNextTick;
    private bool _stopBar;
    private float _tickTime;

    private void Start() {
        _curEnergy = _startEnergy;
        energyBar.fillAmount = _curEnergy / (float) _maxEnergy;

        UpdateText();

        StartCoroutine(GainCoroutine());
        EventManager.OnGlueBall += GlueEnergy;
    }

    private IEnumerator GainCoroutine() {
        while (true) {
            // energyBar.DOKill();
            // energyBar.DOFillAmount((_curEnergy + 1) / (float) _maxEnergy, gainTime);
            _tickTime = Time.time;
            yield return new WaitForSeconds(gainTime);
            if (_stopBar) _stopBar = false;
            
            if (_isGlueNextTick) {
                glueBar.DOKill();
                glueBar.DOFade(1, 0f);
                glueBar.fillAmount = 1;
                glueBar.DOFillAmount(energyBar.fillAmount, Time.time - _tickTime).OnComplete(() => glueBar.DOFade(0, 0.2f));
                _stopBar = true;
                _isGlueNextTick = false;
            } else {
                AddEnergy(gainNum);
            }
        }
    }

    private void Update() {
        if(_stopBar) return;
        energyBar.fillAmount += ((gainNum / gainTime) * Time.deltaTime) / _maxEnergy;
    }

    private void UpdateText() {
        energyText.text = $"{_curEnergy}/{_maxEnergy}";
        // energyBar.DOKill();
        // energyBar.DOFillAmount((_curEnergy + 1) / (float) _maxEnergy, gainTime);
        // energyBar.fillAmount = _curEnergy / (float) _maxEnergy;
    }

    public void AddEnergy(int value) {
        _curEnergy += value;
        if (_curEnergy > _maxEnergy) _curEnergy = _maxEnergy;
        energyBar.fillAmount = _curEnergy / (float) _maxEnergy;
        UpdateText();
    }

    public void SpendEnergy(int value) {
        _curEnergy -= value;
        UpdateText();
        energyBar.fillAmount -= value / (float)_maxEnergy;
    }

    private void GlueEnergy() {
        _isGlueNextTick = true;
        
        // StartCoroutine(GlueEnergyCoroutine());
    }

    // private IEnumerator GlueEnergyCoroutine() {
    //     glueBar.DOKill();
    //     glueBar.fillAmount = 1;
    //     glueBar.DOFillAmount(energyBar.fillAmount, Time.time - _tickTime);
    // }
}