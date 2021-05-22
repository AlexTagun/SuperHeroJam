using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform[] projectileStartPoints;
    [SerializeField] private Transform[] heroPoints;
    [SerializeField] private Transform enemyPrefab;
    [SerializeField] private Vector2 moveDelayRange;
    
    private int _curIndex = 1;
    private Transform _curProjectileStartPoint;
    private Transform _curHeroPoint;

    public Transform CurProjectileStartPoint => _curProjectileStartPoint;
    private void Awake() {
        UpdateState();

        StartCoroutine(Move());
    }

    private void OnLeftButtonClick() {
        _curIndex--;
        if (_curIndex < 0) _curIndex = 2;
        UpdateState();
    }
    
    private void OnRightButtonClick() {
        _curIndex++;
        if (_curIndex > 2) _curIndex = 0;
        UpdateState();
    }

    private void UpdateState() {
        _curProjectileStartPoint = projectileStartPoints[_curIndex];
        _curHeroPoint = heroPoints[_curIndex];

        enemyPrefab.position = _curHeroPoint.position;
    }

    private IEnumerator Move() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(moveDelayRange.x, moveDelayRange.y));

            if (Random.value > 0.5f) {
                OnLeftButtonClick();
            } else {
                OnRightButtonClick();
            }
        }
    }
}
