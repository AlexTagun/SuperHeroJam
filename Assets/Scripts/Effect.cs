using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {
    [SerializeField] private float endTime;

    private void Awake() {
        StartCoroutine(End());
    }

    private IEnumerator End() {
        yield return new WaitForSeconds(endTime);
        Destroy(gameObject);
    }
}