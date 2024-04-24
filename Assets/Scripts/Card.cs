using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Sprite backSprite;
    [SerializeField] private Sprite frontSprite;

    private Image mainRenderer;

    private float rotateSpeed = 300f;

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(ShowCard());
    }

    private void Awake()
    {
        mainRenderer = GetComponent<Image>();

        mainRenderer.sprite = backSprite;
    }

    private IEnumerator ShowCard()
    {
        for(float i = 0f; i < 180f; i += rotateSpeed * Time.deltaTime)
        {
            transform.rotation = Quaternion.Euler(0f, i, 0f);
            if (i > 90 && mainRenderer.sprite != frontSprite)
            {
                mainRenderer.sprite = frontSprite;
            }
            yield return null;
        }
    }

    private IEnumerator HideCard()
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
    }
}
