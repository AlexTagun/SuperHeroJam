using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine;
using Spine.Unity;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour {
    public Transform[] projectileStartPoints;
    [SerializeField] private Transform[] heroPoints;
    [SerializeField] private Transform enemyPrefab;
    [SerializeField] private Vector2 moveDelayRange;
    [SerializeField] private SkeletonAnimation skeletonAnimation;

    private int _curIndex = 1;
    private int _prevIndex = 1;
    private Transform _curProjectileStartPoint;
    private Transform _curHeroPoint;

    public Transform CurProjectileStartPoint => _curProjectileStartPoint;

    private void Awake() {
        

        StartCoroutine(Move());
    }

    private void Start() {
        UpdateState();
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

        var dif = enemyPrefab.transform.position.x - _curHeroPoint.position.x;
        SetAnimation(dif < 0 ? "l_flight" : "r_flight", skeletonAnimation);
        enemyPrefab.DOMove(_curHeroPoint.position, 0.3f).OnComplete(() => SetAnimation("idle", skeletonAnimation));
        // enemyPrefab.position = _curHeroPoint.position;
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
    
    public void SetAnimation(string animationId, SkeletonAnimation skeletonGraphic) {
        string curAnim = null;
        float animationTime = 0;
        if (skeletonGraphic.AnimationState.Tracks.Count > 0) {
            curAnim = skeletonGraphic.AnimationState.Tracks.Items[0].Animation.Name;
            animationTime = skeletonGraphic.AnimationState.GetCurrent(0).AnimationTime;
        }

        if (curAnim != animationId) {
            var entry = skeletonGraphic.AnimationState.SetAnimation(0, animationId, false);
            entry.AnimationStart = animationTime;
            skeletonGraphic.AnimationState.AddAnimation(0, animationId, true, 0);
        }
    }
}