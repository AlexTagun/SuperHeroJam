using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projectile : MonoBehaviour {
    [SerializeField] private ElementType _element;
    [SerializeField] private FormType _form;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private GameObject towerCollisionEffect;
    [SerializeField] private GameObject projectileCollisionEffect;

    public bool IsEnemy;
    public ElementType Element => _element;
    public FormType Form => _form;
    public int LineIndex;

    public float Speed {
        get => _speed;
        set => _speed = value;
    }
    
    public float Damage {
        get => _damage;
        set => _damage = value;
    }


    public void StartMove(int lineIndex) {
        LineIndex = lineIndex;
        if (SceneManager.GetActiveScene().name == "Gameplay_PvP" && IsEnemy) {
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
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
        var tower = other.GetComponent<Tower>();
        if (tower != null) HandleTower(tower);

        var projectile = other.GetComponent<Projectile>();
        if (projectile != null) HandleProjectile(projectile);
    }

    private void HandleTower(Tower tower) {
        if (tower.IsEnemy == IsEnemy) return;
        if (gameObject.name.StartsWith("Water_p_1_wall_glue")) EventManager.HandleOnGlueWall();
        if (gameObject.name.StartsWith("Fire_p_1_ball_glue")) EventManager.HandleOnGlueBall();
        if (gameObject.name.StartsWith("Earth_p_1_lance_glue")) EventManager.HandleOnGlueLance(LineIndex);
        SpawnEffect(towerCollisionEffect);
        tower.GetDamage(_damage);
        Destroy(gameObject);
    }

    private void HandleProjectile(Projectile projectile) {
        if (projectile.IsEnemy == IsEnemy) return;

        SpawnEffect(projectileCollisionEffect);
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

    private void SpawnEffect(GameObject prefab) {
        var effect = Instantiate(prefab);
        effect.transform.position = transform.position;
    }
}