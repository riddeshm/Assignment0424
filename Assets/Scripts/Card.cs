using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Card : MonoBehaviour, IPointerDownHandler
{
    public event System.Action<Card> OnCardSelected;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private Sprite frontSprite;

    private int id = -1;

    private Image mainRenderer;

    private float rotateSpeed = 300f;

    private bool isSelectable = true;

    public int Id { get { return id; } }

    public void UpdateCards(Sprite _frontSprite, Sprite _backSprite, int _id)
    {
        frontSprite = _frontSprite;
        backSprite = _backSprite;
        id = _id;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isSelectable)
            return;
        StartCoroutine(RotateToShowCard());
    }

    private void Awake()
    {
        mainRenderer = GetComponent<Image>();

        mainRenderer.sprite = backSprite;
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
        isSelectable = true;
    }

    public void ResetCard()
    {
        StartCoroutine(RotateToResetCard());
    }
    public void HideCard()
    {
        mainRenderer.enabled = false;
    }
}
