using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileFactory : MonoBehaviour {
    [SerializeField] private Projectile[] pool;
    [SerializeField] private int[] weights;
    [SerializeField] private Vector2 reloadRange;

    private EnemyController _enemyController;

    private WeightedList<Projectile> _list = new WeightedList<Projectile>();

    private void Start() {
        _enemyController = GameObject.FindWithTag("GameController").GetComponent<EnemyController>();
        
        for (int i = 0; i < pool.Length; i++) {
            _list.AddEntry(pool[i], weights[i]);
        }

        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(reloadRange.x, reloadRange.y));

            _enemyController.enemyPrefab.transform.DOShakePosition(0.3f, 1);
            yield return new WaitForSeconds(0.3f);
            var prefab = _list.GetRandom();

            Projectile projectile;
            if (prefab.Form == FormType.Wall) {
                for (int i = 0; i < _enemyController.projectileStartPoints.Length; i++) {
                    projectile = Instantiate(prefab, _enemyController.projectileStartPoints[i]);
                    projectile.Speed *= -1;
                    projectile.IsEnemy = true;
                    projectile.StartMove(i);
                }
            } else {
                projectile = Instantiate(prefab, _enemyController.CurProjectileStartPoint);
                projectile.Speed *= -1;
                projectile.IsEnemy = true;
                projectile.StartMove(_enemyController.CurIndex);
            }
        }
    }
}