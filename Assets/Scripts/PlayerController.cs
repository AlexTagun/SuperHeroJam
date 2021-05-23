using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public Transform[] projectileStartPoints;
    [SerializeField] private Transform[] heroPoints;
    [SerializeField] private Transform heroPrefab;
    [SerializeField] private SkeletonAnimation skeletonAnimation;

    private int _curIndex = 1;
    private Transform _curHeroPoint;

    private void Start() {
        UpdateState(1);
        
    }

    // private void OnLeftButtonClick() {
    //     _curIndex--;
    //     if (_curIndex < 0) _curIndex = 2;
    //     UpdateState();
    // }
    //
    // private void OnRightButtonClick() {
    //     _curIndex++;
    //     if (_curIndex > 2) _curIndex = 0;
    //     UpdateState();
    // }

    public void UpdateState(int lineIndex) {
        _curHeroPoint = heroPoints[lineIndex];
        
        var dif = heroPrefab.transform.position.x - _curHeroPoint.position.x;
        SetAnimation(dif < 0 ? "l_flight" : "r_flight", skeletonAnimation);

        heroPrefab.DOMove(_curHeroPoint.position, 0.3f).OnComplete(() => SetAnimation("idle", skeletonAnimation));
        // heroPrefab.position = _curHeroPoint.position;
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