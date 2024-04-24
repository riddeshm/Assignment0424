using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public enum CardState
{
    back,
    front,
    complete
}

public class Card : MonoBehaviour, IPointerDownHandler
{
    public event System.Action<Card> OnCardSelected;
    private Sprite backSprite;
    private Sprite frontSprite;

    private int id = -1;

    private Image mainRenderer;

    private float rotateSpeed = 300f;

    private bool isSelectable = true;

    private CardState currentState;

    
    private void Awake()
    {
        mainRenderer = GetComponent<Image>();

        currentState = CardState.back;
    }

    public int GetId()
    { 
        return id;
    }

    public void UpdateCards(Sprite _frontSprite, Sprite _backSprite, int _id, CardState _currentState = CardState.back)
    {
        frontSprite = _frontSprite;
        backSprite = _backSprite;
        id = _id;
        mainRenderer.enabled = true;
        currentState = _currentState;
        Debug.Log("_backSprite " + _backSprite);
        Debug.Log("backSprite " + backSprite);
        mainRenderer.sprite = backSprite;
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isSelectable)
            return;
        StartCoroutine(RotateToShowCard());
    }

    private IEnumerator RotateToShowCard()
    {
        isSelectable = false;
        for (float i = 0f; i < 180f; i += rotateSpeed * Time.deltaTime)
        {
            transform.rotation = Quaternion.Euler(0f, i, 0f);
            if (i > 90 && mainRenderer.sprite != frontSprite)
            {
                mainRenderer.sprite = frontSprite;
            }
            yield return null;
        }
        currentState = CardState.front;
        OnCardSelected?.Invoke(this);
    }

    private IEnumerator RotateToResetCard()
    {
        for (float i = 180f; i >= 0f; i -= rotateSpeed * Time.deltaTime)
        {
            transform.rotation = Quaternion.Euler(0f, i, 0f);
            if (i < 90 && mainRenderer.sprite != backSprite)
            {
                mainRenderer.sprite = backSprite;
            }
            yield return null;
        }
        currentState = CardState.back;
        isSelectable = true;
    }

    public void ResetCard()
    {
        StartCoroutine(RotateToResetCard());
    }
    public void HideCard()
    {
        currentState = CardState.complete;
        mainRenderer.enabled = false;
    }
}
