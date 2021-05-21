using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandController : MonoBehaviour {
    [SerializeField] private UICard[] _deckRank1;
    [SerializeField] private UICard[] _deckRank2;
    [SerializeField] private RectTransform _handContainer;

    private List<UICard> _hand = new List<UICard>();

    public void FillHand() {
        while (_hand.Count < 5) {
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
        FillHand();
    }

    public UICard IsOnUICard(RectTransform rectTransform) {
        foreach (var card in _hand) {
            var cardRect = card.transform as RectTransform;
            var pos = cardRect.InverseTransformPoint(rectTransform.position);
            if (cardRect.rect.Contains(pos)) return card;
        }

        return null;
    }
}