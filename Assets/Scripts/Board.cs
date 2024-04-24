using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class Board : MonoBehaviour
{
	public event System.Action<Card> OnCardSelected;
	[SerializeField]private string backSpriteAddress;
	[SerializeField] private string[] faceSpritesAddress;
	[SerializeField] private GameObject inputFieldPrefab;
	private int rows;
	private int cols;
	private List<Card> cardPool = new List<Card>();
	private int totalcards;
	private int totalFaceSprites;

	private Sprite backSprite;
	private List<Sprite> selectedFaceSprites = new List<Sprite>();
	private RectTransform parentRect;
	private GridLayoutGroup gridLayout;

	private void Awake()
	{
		parentRect = gameObject.GetComponent<RectTransform>();
		gridLayout = gameObject.GetComponent<GridLayoutGroup>();
		
	}

	private void OnDestroy()
	{
		for (int i = 0; i < cardPool.Count; i++)
		{
			cardPool[i].OnCardSelected -= CardSelected;
		}
	}

    public void Init(int _rows, int _cols)
    {
		rows = _rows;
		cols = _cols;
		totalcards = rows * cols;
		
		selectedFaceSprites.Clear();

		FetchAssetsFromAddressables();
	}

	private void FetchAssetsFromAddressables()
    {
		totalFaceSprites = Mathf.CeilToInt((float)(rows * cols) / 2f);
		int randomFrontSpriteIndex = Random.Range(0, faceSpritesAddress.Length - totalFaceSprites);

		Addressables.LoadAssetAsync<Sprite>(backSpriteAddress).Completed += OnBackSpriteComplete;
		Debug.Log("randomFrontSpriteIndex " + randomFrontSpriteIndex);
		Debug.Log("totalFaceSprites " + totalFaceSprites);
		for (int i = 0, j = randomFrontSpriteIndex; i < totalFaceSprites; i++)
		{
			Debug.Log("LoadAssetAsync " + faceSpritesAddress[j]);
			Addressables.LoadAssetAsync<Sprite>(faceSpritesAddress[j]).Completed += OnFaceSpriteComplete;
			j++;
		}
	}

	private void OnBackSpriteComplete(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<Sprite> obj)
	{
		backSprite = obj.Result;
		UpdateCards();
		ShuffleList<Card>(cardPool);
		AddCardsOnBoard();
	}

	private void OnFaceSpriteComplete(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<Sprite> obj)
	{
		selectedFaceSprites.Add(obj.Result);
	}

	private void CreateCards(int count)
    {
		for(int i = 0; i < count; i++)
        {
			GameObject cardGo = Instantiate(inputFieldPrefab);
			Card card = cardGo.GetComponent<Card>();
			card.OnCardSelected += CardSelected;
			cardPool.Add(card);
		}
    }

	private void HideCards(int fromIndex)
    {
		Debug.Log("fromIndex " + fromIndex);
		for (int i = fromIndex; i < cardPool.Count; i++)
		{
			cardPool[i].gameObject.SetActive(false);
		}
	}

	private void UpdateCards()
    {
		int cardCount = 0;
		int id = 0;
		if(cardPool.Count < totalcards)
        {
			CreateCards(totalcards - cardPool.Count);
		}
		else if(cardPool.Count > totalcards)
        {
			int extraCards = cardPool.Count - totalcards;
			HideCards(cardPool.Count - extraCards);
		}
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				cardPool[cardCount].gameObject.SetActive(true);
				Debug.Log("UpdateCards backSprite " + backSprite);
				cardPool[cardCount].UpdateCards(selectedFaceSprites[id], backSprite, id);
				cardCount++;
				if (cardCount % 2 == 0)
				{
					id++;
				}
			}
		}
	}

	private void ShuffleList<T>(List<T> list)
	{
		System.Random random = new System.Random();

		for (int i = totalcards - 1; i > 0; i--)
		{
			int randomIndex = random.Next(0, i);
			T temp = list[randomIndex];
			list[randomIndex] = list[i];
			list[i] = temp;
		}
	}

	private void AddCardsOnBoard()
	{
		gridLayout.cellSize = new Vector2(parentRect.rect.width / cols, parentRect.rect.height / rows);
		for (int i = 0; i < totalcards; i++)
		{
			cardPool[i].transform.SetParent(transform, false);
		}
	}

	private void CardSelected(Card _card)
    {
		OnCardSelected?.Invoke(_card);
    }

	public void CorrectPair(Card _card1, Card _card2)
    {
		_card1.HideCard();
		_card2.HideCard();
	}

	public void InCorrectPair(Card _card1, Card _card2)
	{
		_card1.ResetCard();
		_card2.ResetCard();
	}
}
