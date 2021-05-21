using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Projectile _projectilePrefab;

    public ElementType Element => _element;
    public int Rank => _rank;
    public Projectile ProjectilePrefab => _projectilePrefab;
}