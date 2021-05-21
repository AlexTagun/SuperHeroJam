using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] private ElementType _element;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;
    [SerializeField] private Rigidbody2D _rb;
    

    private void Awake() {
        _rb.velocity = new Vector2(0, _speed);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Collision");
        var tower = other.GetComponent<Tower>();
        if(tower == null) return;
        if (tower.IsEnemy) {
            tower.GetDamage(_damage);
            Destroy(gameObject);
        }
    }
}