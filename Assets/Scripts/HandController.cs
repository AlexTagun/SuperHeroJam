﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class HandController : MonoBehaviour {
    [SerializeField] private UICard[] _deckRank1;
    [SerializeField] private UICard[] _deckRank2;
    [SerializeField] private RectTransform _handContainer;
    [SerializeField] private RectTransform _leftLine;
    [SerializeField] private RectTransform _middleLine;
    [SerializeField] private RectTransform _rightLine;
    [SerializeField] private float _drawCardTime;
    [SerializeField] private int _maxCardsInHand;
    [SerializeField] private int _startCardCount;

    private List<UICard> _hand = new List<UICard>();
    private PlayerController _playerController;
    private EnergyController _energyController;


    private GameObject _leftLineContainer;
    private GameObject _middleLineContainer;
    private GameObject _rightLineContainer;

    private void Awake() {
        _playerController = GameObject.FindWithTag("GameController").GetComponent<PlayerController>();
        _energyController = GameObject.FindWithTag("GameController").GetComponent<EnergyController>();
        DrawCard(_startCardCount);

        _leftLineContainer = _leftLine.GetChild(0).gameObject;
        _middleLineContainer = _middleLine.GetChild(0).gameObject;
        _rightLineContainer = _rightLine.GetChild(0).gameObject;

        StartCoroutine(DrawCoroutine());
        UpdateLinesHighlighting(null);

        EventManager.OnGlueWall += LockRandomCard;
    }

    private void OnDestroy() {
        EventManager.OnGlueWall -= LockRandomCard;
    }

    private IEnumerator DrawCoroutine() {
        while (true) {
            yield return new WaitForSeconds(_drawCardTime);

            DrawCard(1);
        }
    }

    private void DrawCard(int count) {
        for (int i = 0; i < count; i++) {
            if (_hand.Count == _maxCardsInHand) break;
            var newCard = Instantiate(_deckRank1[Random.Range(0, _deckRank1.Length)], _handContainer);
            _hand.Add(newCard);
        }
    }

    public void MergeCards(UICard card1, UICard card2) {
        if (card1.Rank != 1 && card2.Rank != 1) return;
        if (card1.Element != card2.Element) return;

        var newCard = Instantiate(_deckRank2.First(card => card.Element == card1.Element), _handContainer);
        _hand.Remove(card1);
        _hand.Remove(card2);
        Destroy(card1.gameObject);
        Destroy(card2.gameObject);

        _hand.Add(newCard);
        // FillHand();
    }

    public UICard IsOnUICard(RectTransform rectTransform, UICard draggableCard) {
        foreach (var card in _hand) {
            if (card.transform.position == draggableCard.transform.position) continue;
            var cardRect = card.transform as RectTransform;
            var pos = cardRect.InverseTransformPoint(rectTransform.position);
            if (cardRect.rect.Contains(pos)) return card;
        }

        return null;
    }

    public int CanPlayCard(RectTransform rectTransform) {
        var pos = _leftLine.InverseTransformPoint(rectTransform.position);
        if (_leftLine.rect.Contains(pos)) return 0;

        pos = _middleLine.InverseTransformPoint(rectTransform.position);
        if (_middleLine.rect.Contains(pos)) return 1;

        pos = _rightLine.InverseTransformPoint(rectTransform.position);
        if (_rightLine.rect.Contains(pos)) return 2;
        return -1;
    }

    public void UpdateLinesHighlighting(RectTransform rectTransform) {
        _leftLineContainer.SetActive(false);
        _middleLineContainer.SetActive(false);
        _rightLineContainer.SetActive(false);

        if (rectTransform == null) return;

        var pos = _leftLine.InverseTransformPoint(rectTransform.position);
        if (_leftLine.rect.Contains(pos)) _leftLineContainer.SetActive(true);

        pos = _middleLine.InverseTransformPoint(rectTransform.position);
        if (_middleLine.rect.Contains(pos)) _middleLineContainer.SetActive(true);

        pos = _rightLine.InverseTransformPoint(rectTransform.position);
        if (_rightLine.rect.Contains(pos)) _rightLineContainer.SetActive(true);
        ;
    }

    public void PlayCard(UICard card, int lineIndex) {
        _hand.Remove(card);
        var projectile = Instantiate(card.ProjectilePrefab, _playerController.projectileStartPoints[lineIndex]);
        _playerController.UpdateState(lineIndex);
        _energyController.SpendEnergy(card.EnergyCost);
        Destroy(card.gameObject);
        // FillHand();
        projectile.StartMove();
    }

    private void LockRandomCard() {
        float r = Random.value * _hand.Count;

        for (int i = 0; i < _hand.Count; i++) {
            if(_hand[i].IsLocked) continue;
            if (i >= r) {
                _hand[i].Lock();
                return;
            }
        }
    }
}