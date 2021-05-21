using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {
    [SerializeField] private UICard[] _deck;
    [SerializeField] private RectTransform _handContainer;

    private List<UICard> _hand = new List<UICard>();

    public void FillHand() {
        while (_hand.Count < 5) {
            var newCard = Instantiate(_deck[Random.Range(0, _deck.Length)], _handContainer);
            _hand.Add(newCard);
        }
    }
}