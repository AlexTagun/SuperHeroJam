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
    [SerializeField] private float _damage;

    public ElementType Element => _element;
    public int Rank => _rank;

    public void SetData() {
        
    }

    public void Play() {
        
    }
    
}