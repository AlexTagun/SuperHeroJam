using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HandController : MonoBehaviour {
    [SerializeField] private UICard[] _deckRank1;
    [SerializeField] private UICard[] _deckRank2;
    [SerializeField] private UICard[] _deckRank3;
    [SerializeField] private UICard[] _deckWalls;
    [SerializeField] private UICard[] _deckBalls;
    [SerializeField] private UICard[] _deckLances;
    [SerializeField] private int[] _deckWeights;
    [SerializeField] private RectTransform _handContainer;
    [SerializeField] private RectTransform _leftLine;
    [SerializeField] private RectTransform _middleLine;
    [SerializeField] private RectTransform _rightLine;
    [SerializeField] private float _drawCardTime;
    [SerializeField] private int _maxCardsInHand;
    [SerializeField] private int _startCardCount;
    [SerializeField] private float _lockLineTime;

    private List<UICard> _hand = new List<UICard>();
    private PlayerController _playerController;
    private EnergyController _energyController;
    private bool _isGlueNextCard;


    private GameObject _leftLineContainer;
    private GameObject _middleLineContainer;
    private GameObject _rightLineContainer;

    private GameObject _leftLineLockContainer;
    private GameObject _middleLineLockContainer;
    private GameObject _rightLineLockContainer;
    
    private WeightedList<UICard> _deck = new WeightedList<UICard>();

    private void Awake() {
        _playerController = GameObject.FindWithTag("GameController").GetComponent<PlayerController>();
        _energyController = GameObject.FindWithTag("GameController").GetComponent<EnergyController>();
        
        for (int i = 0; i < _deckRank1.Length; i++) {
            _deck.AddEntry(_deckRank1[i], _deckWeights[i]);
        }
        
        DrawCard(_startCardCount);

        for (int i = _startCardCount; i < _maxCardsInHand; i++) {
            var placeholder = new GameObject();
            placeholder.transform.SetParent(_handContainer);
            placeholder.name = "placeholder";
            placeholder.AddComponent<LayoutElement>();
        }

        _leftLineContainer = _leftLine.GetChild(0).gameObject;
        _middleLineContainer = _middleLine.GetChild(0).gameObject;
        _rightLineContainer = _rightLine.GetChild(0).gameObject;

        _leftLineLockContainer = _leftLine.GetChild(1).gameObject;
        _middleLineLockContainer = _middleLine.GetChild(1).gameObject;
        _rightLineLockContainer = _rightLine.GetChild(1).gameObject;

        _leftLineLockContainer.SetActive(false);
        _middleLineLockContainer.SetActive(false);
        _rightLineLockContainer.SetActive(false);

        StartCoroutine(DrawCoroutine());
        // StartCoroutine(MergeHintCoroutine());
        UpdateLinesHighlighting(null);

        EventManager.OnGlueWall += LockRandomCard;
        EventManager.OnGlueBall += GlueNextCard;
        EventManager.OnGlueLance += LockLine;
    }

    private void OnDestroy() {
        EventManager.OnGlueWall -= LockRandomCard;
        EventManager.OnGlueBall -= GlueNextCard;
        EventManager.OnGlueLance -= LockLine;
    }

    private IEnumerator MergeHintCoroutine() {
        while (true) {
            yield return new WaitForSeconds(5);

            UICard a = null;
            UICard b = null;
            for (int i = 0; i < _hand.Count - 1; i++) {
                if (_hand[i].Form != FormType.Base) continue;
                if (_hand[i].Type != CardType.Base) continue;
                a = _hand[i];
                for (int j = 0; j < _hand.Count; j++) {
                    if (i == j) continue;
                    if (_hand[j].Type == CardType.Form) {
                        b = _hand[j];
                        break;
                    }

                    if (_hand[j].Type == CardType.Base) {
                        if (a.Element != _hand[j].Element) continue;
                        b = _hand[j];
                        break;
                    }
                }

                if (b != null) break;
            }

            if (a != null && b != null) {
                a.MergeHintAnimation();
                b.MergeHintAnimation();
            }
        }
    }

    public void ShowHint(UICard card) {
        if (card.Type == CardType.Base) {
            for (int j = 0; j < _hand.Count; j++) {
                if (_hand[j].Type == CardType.Form) {
                    _hand[j].MergeHintAnimation();
                }

                if (_hand[j].Type == CardType.Base) {
                    if (card.Element != _hand[j].Element) continue;
                    if (card.Rank != _hand[j].Rank) continue;
                    _hand[j].MergeHintAnimation();
                }
            }
        } else {
            for (int j = 0; j < _hand.Count; j++) {
                if (_hand[j].Type == CardType.Form) continue;

                if (_hand[j].Type == CardType.Base) {
                    _hand[j].MergeHintAnimation();
                }
            }
        }
        
    }

    public void StopHint() {
        for (int i = 0; i < _hand.Count; i++) {
            _hand[i].transform.DOComplete();
            _hand[i].transform.DOKill();
            _hand[i].transform.eulerAngles = Vector3.zero;
        }
    }

    private IEnumerator DrawCoroutine() {
        while (true) {
            yield return new WaitForSeconds(_drawCardTime);

            if (_isGlueNextCard) {
                _isGlueNextCard = false;
            } else {
                DrawCard(1);
            }
        }
    }

    private void DrawCard(int count) {
        for (int i = 0; i < count; i++) {
            if (_hand.Count == _maxCardsInHand) break;
            var newCard = Instantiate(_deck.GetRandom(), _handContainer);
            newCard.transform.SetSiblingIndex(CalculateSiblingIndex());
            _hand.Add(newCard);
        }
    }

    public void MergeCards(UICard card1, UICard card2) {
        if (card1.Type == CardType.Base && card2.Type == CardType.Base) HandleMergeBase(card1, card2);
        if (card1.Type == CardType.Base && card2.Type == CardType.Form
            || card1.Type == CardType.Form && card2.Type == CardType.Base) HandleMergeForm(card1, card2);

        // FillHand();
    }

    private void HandleMergeBase(UICard card1, UICard card2) {
        if (card1.Rank != card2.Rank) return;
        if (card1.Element != card2.Element) return;
        if (card1.Form != card2.Form) return;
        if (card1.Rank == 3) return;

        UICard prefab = null;
        switch (card1.Form) {
            case FormType.Base:
                var pool = card1.Rank == 2 ? _deckRank3 : _deckRank2;
                prefab = pool.First(card => card.Element == card1.Element);
                break;
            case FormType.Wall:
                prefab = _deckWalls.First(card => card.Element == card1.Element && card.Rank == card1.Rank + 1);
                break;
            case FormType.Ball:
                prefab = _deckBalls.First(card => card.Element == card1.Element && card.Rank == card1.Rank + 1);
                break;
            case FormType.Lance:
                prefab = _deckLances.First(card => card.Element == card1.Element && card.Rank == card1.Rank + 1);
                break;
        }

        var newCard = Instantiate(prefab, _handContainer);
        newCard.transform.SetSiblingIndex(card2.transform.GetSiblingIndex());
        _hand.Remove(card1);
        _hand.Remove(card2);

        var placeholder = new GameObject();
        placeholder.transform.SetParent(_handContainer);
        placeholder.name = "placeholder";
        placeholder.transform.SetSiblingIndex(card1.transform.GetSiblingIndex());
        placeholder.AddComponent<LayoutElement>();

        Destroy(card1.gameObject);
        Destroy(card2.gameObject);

        _hand.Add(newCard);
    }

    private void HandleMergeForm(UICard card1, UICard card2) {
        var baseCard = card1.Type == CardType.Base ? card1 : card2;
        var formCard = card1.Type == CardType.Form ? card1 : card2;
        if (baseCard.Form != FormType.Base) return;

        UICard[] pool = new UICard[0];
        switch (formCard.Form) {
            case FormType.Wall:
                pool = _deckWalls;
                break;
            case FormType.Ball:
                pool = _deckBalls;
                break;
            case FormType.Lance:
                pool = _deckLances;
                break;
        }

        var newCard = Instantiate(pool.First(card => card.Element == baseCard.Element && card.Rank == baseCard.Rank),
            _handContainer);

        newCard.transform.SetSiblingIndex(card2.transform.GetSiblingIndex());
        _hand.Remove(card1);
        _hand.Remove(card2);

        var placeholder = new GameObject();
        placeholder.transform.SetParent(_handContainer);
        placeholder.name = "placeholder";
        placeholder.transform.SetSiblingIndex(card1.transform.GetSiblingIndex());
        placeholder.AddComponent<LayoutElement>();

        Destroy(card1.gameObject);
        Destroy(card2.gameObject);

        _hand.Add(newCard);
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
        if (card.Type != CardType.Base) return;
        if (lineIndex == 0 && _leftLineLockContainer.activeSelf
            || lineIndex == 1 && _middleLineLockContainer.activeSelf
            || lineIndex == 2 && _rightLineLockContainer.activeSelf) return;

        _hand.Remove(card);

        Projectile projectile;
        if (card.ProjectilePrefab.Form == FormType.Wall) {
            for (int i = 0; i < _playerController.projectileStartPoints.Length; i++) {
                projectile = Instantiate(card.ProjectilePrefab, _playerController.projectileStartPoints[i]);
                projectile.StartMove(lineIndex);
            }
        } else {
            projectile = Instantiate(card.ProjectilePrefab, _playerController.projectileStartPoints[lineIndex]);
            _playerController.UpdateState(lineIndex);
            projectile.StartMove(lineIndex);
        }

        _energyController.SpendEnergy(card.EnergyCost);
        Destroy(card.gameObject);
    }

    private void LockRandomCard() {
        float r = Random.value * _hand.Count;

        for (int i = 0; i < _hand.Count; i++) {
            if (_hand[i].IsLocked) continue;
            if (i >= r) {
                _hand[i].Lock();
                return;
            }
        }
    }

    private void GlueNextCard() {
        _isGlueNextCard = true;
    }

    private void LockLine(int index) {
        // Debug.Log("lock");
        // Debug.Log(projectilePos);
        // var pos_l = _leftLine.InverseTransformPoint(projectilePos);
        // var pos_m = _middleLine.InverseTransformPoint(projectilePos);
        // var pos_r = _rightLine.InverseTransformPoint(projectilePos);
        //
        // var min = Mathf.Min(pos_l.x, Mathf.Min(pos_m.x, pos_r.x));
        //
        // if (Math.Abs(pos_l.x - min) < 1) StartCoroutine(LockLineCoroutine(_leftLineLockContainer));
        //
        // if (Math.Abs(pos_m.x - min) < 1) StartCoroutine(LockLineCoroutine(_middleLineLockContainer));
        //
        // if (Math.Abs(pos_r.x - min) < 1) StartCoroutine(LockLineCoroutine(_rightLineLockContainer));

        switch (index) {
            case 0:
                StartCoroutine(LockLineCoroutine(_leftLineLockContainer));
                break;
            case 1:
                StartCoroutine(LockLineCoroutine(_middleLineLockContainer));
                break;
            case 2:
                StartCoroutine(LockLineCoroutine(_rightLineLockContainer));
                break;
        }
    }

    private IEnumerator LockLineCoroutine(GameObject line) {
        Debug.Log("cor");
        line.SetActive(true);
        yield return new WaitForSeconds(_lockLineTime);
        line.SetActive(false);
    }

    private int CalculateSiblingIndex() {
        int count = _handContainer.transform.childCount;
        for (int i = 0; i < count; i++) {
            var child = _handContainer.transform.GetChild(i).gameObject;
            if (child.name == "placeholder") {
                Destroy(child);
                return i;
            }
        }

        return _handContainer.transform.childCount;
    }
}