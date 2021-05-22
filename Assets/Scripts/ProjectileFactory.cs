﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileFactory : MonoBehaviour {
    [SerializeField] private Projectile[] pool;
    [SerializeField] private int[] weights;
    [SerializeField] private Vector2 reloadRange;
    [SerializeField] private Transform projectileStartPoint;


    private WeightedList<Projectile> _list = new WeightedList<Projectile>();

    private void Start() {
        for (int i = 0; i < pool.Length; i++) {
            _list.AddEntry(pool[i], weights[i]);
        }

        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(reloadRange.x, reloadRange.y));

            var prefab = _list.GetRandom();

            var projectile = Instantiate(prefab, projectileStartPoint);
            projectile.Speed *= -1;
            projectile.IsEnemy = true;
            projectile.StartMove();
        }
    }
}