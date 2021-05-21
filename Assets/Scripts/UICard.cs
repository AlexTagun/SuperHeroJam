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
    [SerializeField] private ElementType element;
    [SerializeField] private int rank;
    [SerializeField] private float damage;
    

    public void SetData() {
        
    }
}