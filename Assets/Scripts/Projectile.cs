using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] private ElementType _element;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;
    [SerializeField] private Rigidbody2D _rb;

    public bool IsEnemy;
    public ElementType Element => _element;

    public float Speed {
        get => _speed;
        set => _speed = value;
    }


    public void StartMove() {
        _rb.velocity = new Vector2(0, _speed);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Collision");
        var tower = other.GetComponent<Tower>();
        if (tower != null) HandleTower(tower);

        var projectile = other.GetComponent<Projectile>();
        if (projectile != null) HandleProjectile(projectile);
    }

    private void HandleTower(Tower tower) {
        if (tower.IsEnemy == IsEnemy) return;
        tower.GetDamage(_damage);
        Destroy(gameObject);
    }

    private void HandleProjectile(Projectile projectile) {
        if (projectile.IsEnemy == IsEnemy) return;

        if (projectile.Element == _element) {
            Destroy(projectile.gameObject);
            Destroy(gameObject);
        }
    }
}