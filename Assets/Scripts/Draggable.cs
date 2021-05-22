using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
    [HideInInspector] public Transform parentToReturnTo = null;
    [HideInInspector] public Transform placeHolderParent = null;

    [SerializeField] private UICard card;
    [SerializeField] private Transform icon;

    private GameObject placeHolder = null;
    private HandController _handController;
    private EnergyController _energyController;

    private void Awake() {
        _handController = GameObject.FindWithTag("GameController").GetComponent<HandController>();
        _energyController = GameObject.FindWithTag("GameController").GetComponent<EnergyController>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        placeHolder = gameObject;
        // placeHolder.transform.SetParent(this.transform.parent);
        // LayoutElement le = placeHolder.AddComponent<LayoutElement>();
        // le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        // le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        // le.flexibleWidth = 0;
        // le.flexibleHeight = 0;
        //
        // placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        parentToReturnTo = this.transform.parent;
        placeHolderParent = parentToReturnTo;
        icon.transform.SetParent(this.transform.parent.parent);

        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        icon.transform.position = eventData.position;

        if (placeHolder.transform.parent != placeHolderParent) {
            placeHolder.transform.SetParent(placeHolderParent);
        }
        
        _handController.UpdateLinesHighlighting(icon.transform as RectTransform);
    }

    public void OnEndDrag(PointerEventData eventData) {
        _handController.UpdateLinesHighlighting(null);
        var isOnUICard = _handController.IsOnUICard(icon.transform as RectTransform, card);
        var canPlayCard = _handController.CanPlayCard(icon.transform as RectTransform);
        if (canPlayCard == -1 && isOnUICard != null) {
            _handController.MergeCards(card, isOnUICard);
        }

        EventManager.IsDragging = false;
        // int newSiblingIndex = placeHolderParent.childCount;
        icon.transform.SetParent(transform);
        // icon.transform.SetSiblingIndex(1);
        icon.localPosition = Vector3.zero;
        //
        // // this.transform.SetParent(parentToReturnTo);
        // for (int i = 0; i < placeHolderParent.childCount; i++) {
        //     var rect = (placeHolderParent.GetChild(i) as RectTransform);
        //     var pos = rect.InverseTransformPoint(eventData.position);
        //
        //     if (rect.rect.Contains(pos)) {
        //         newSiblingIndex = i;
        //
        //         var placeHolderIndex = placeHolder.transform.GetSiblingIndex();
        //         placeHolderParent.GetChild(newSiblingIndex).SetSiblingIndex(placeHolderIndex);
        //
        //         // if (placeHolder.transform.GetSiblingIndex() < newSiblingIndex) {
        //         //     newSiblingIndex++;
        //         // }
        //
        //         this.transform.SetSiblingIndex(newSiblingIndex);
        //
        //         this.GetComponent<CanvasGroup>().blocksRaycasts = true;
        //         // Destroy(placeHolder);
        //         EventManager.HandleOnItemSwapped();
        //         return;
        //     }
        // }
        //
        // this.transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
        //
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
        // card.Item?.Destroy();
        // card.Item = null;
        // EventManager.HandleOnItemSwapped();
        // Destroy(placeHolder);

        if (canPlayCard != -1 && _energyController.CurEnergy >= card.EnergyCost)
            _handController.PlayCard(card, canPlayCard);
    }

    public void OnPointerDown(PointerEventData eventData) {
        EventManager.IsDragging = true;
    }

    private void OnDestroy() {
        Destroy(icon.gameObject);
    }
}