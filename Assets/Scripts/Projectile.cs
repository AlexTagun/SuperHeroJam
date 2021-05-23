using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] private ElementType _element;
    [SerializeField] private FormType _form;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;
    [SerializeField] private Rigidbody2D _rb;

    public bool IsEnemy;
    public ElementType Element => _element;
    public FormType Form => _form;

    public float Speed {
        get => _speed;
        set => _speed = value;
    }
    
    public float Damage {
        get => _damage;
        set => _damage = value;
    }


    public void StartMove() {
        if (_form == FormType.Wall) {
            return;
        }
        _rb.velocity = new Vector2(0, _speed);
    }

    private void Update() {
        if (_form == FormType.Wall) {
            _rb.velocity += new Vector2(0, _speed * Time.deltaTime);
        }
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
        if (gameObject.name.StartsWith("Water_p_1_wall_glue")) EventManager.HandleOnGlueWall();
        if (gameObject.name.StartsWith("Fire_p_1_ball_glue")) EventManager.HandleOnGlueBall();
        tower.GetDamage(_damage);
        Destroy(gameObject);
    }

    private void HandleProjectile(Projectile projectile) {
        if (projectile.IsEnemy == IsEnemy) return;

        if (projectile.Element == _element) {
            Destroy(projectile.gameObject);
            Destroy(gameObject);
            return;
        }

        if (IsDominantElement(this, projectile)) {
            Destroy(projectile.gameObject);
            Damage *= 2;
        } else {
            projectile.Damage *= 2;
            Destroy(gameObject);
        }
    }

    private bool IsDominantElement(Projectile a, Projectile b) {
        if (a.Element == ElementType.Water && b.Element == ElementType.Fire) return true;
        if (a.Element == ElementType.Fire && b.Element == ElementType.Earth) return true;
        if (a.Element == ElementType.Earth && b.Element == ElementType.Water) return true;
        return false;
    }
}