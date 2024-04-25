using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.AddressableAssets;

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
    private string frontSpriteAddress;

    private int id = -1;

    private Image mainRenderer;

    private float rotateSpeed = 300f;

    private bool isSelectable = true;

    private CardState currentState;

    public string FrontSpriteAddress { get { return frontSpriteAddress; } }

    public CardState CurrentState { get { return currentState; } }


    private void Awake()
    {
        mainRenderer = GetComponent<Image>();

        currentState = CardState.back;
    }

    public int GetId()
    { 
        return id;
    }

    public void UpdateData(string _frontSpriteAddress, int _id, CardState _currentState = CardState.back)
    {
        if(frontSprite != null)
        {
            Addressables.Release<Sprite>(frontSprite);
        }
        frontSpriteAddress = _frontSpriteAddress;
        Addressables.LoadAssetAsync<Sprite>(frontSpriteAddress).Completed += OnFrontSpriteComplete;
        
        id = _id;
        currentState = _currentState;
    }

    public void UpdateView(Sprite _backSprite)
    {
        backSprite = _backSprite;
        mainRenderer.enabled = true;
        
        mainRenderer.sprite = backSprite;
        Debug.Log("mainRenderer.sprite " + mainRenderer.sprite);
        switch (currentState)
        {
            case CardState.back:
                transform.rotation = Quaternion.identity;
                mainRenderer.sprite = backSprite;
                isSelectable = true;
                break;
            case CardState.front:
                transform.rotation = Quaternion.Euler(0f, 180, 0f);
                mainRenderer.sprite = frontSprite;
                isSelectable = false;
                OnCardSelected?.Invoke(this);
                break;
            case CardState.complete:
                HideCard();
                break;
        }
    }

    private void OnFrontSpriteComplete(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<Sprite> obj)
    {
        frontSprite = obj.Result;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isSelectable)
            return;
        AudioController.Instance.PlayFlip();
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
