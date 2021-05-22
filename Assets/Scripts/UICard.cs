using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ElementType {
    Fire,
    Water,
    Earth,
    Glue
}

public class UICard : MonoBehaviour {
    [SerializeField] private ElementType _element;
    [SerializeField] private int _rank;
    public int EnergyCost;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private TextMeshProUGUI _energyText;

    private void Awake() {
        _energyText.text = EnergyCost.ToString();
    }

    public ElementType Element => _element;
    public int Rank => _rank;
    public Projectile ProjectilePrefab => _projectilePrefab;
}