using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    public Transform[] projectileStartPoints;
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
        int i;
        for (i = 0; i < 3; i++) {
            _curIndex--;
            if (_curIndex < 0) _curIndex = 2;
            if (projectileStartPoints[_curIndex] != null) break;
        }

        if (i == 3) WinGame();

        UpdateState();
    }

    private void OnRightButtonClick() {
        int i;
        for (i = 0; i < 3; i++) {
            _curIndex++;
            if (_curIndex > 2) _curIndex = 0;
            if (projectileStartPoints[_curIndex] != null) break;
        }
        
        if (i == 3) WinGame();
        
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

    private void WinGame() {
        Debug.Log("WIN");
    }
}