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
	private string[] selectedFaceSpriteAddresses;
	private RectTransform parentRect;
	private GridLayoutGroup gridLayout;

	public int Rows { get { return rows; } }
	public int Cols { get { return cols; } }
	public Card[] Cards { get { return cardPool.GetRange(0, totalcards).ToArray(); } }

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

		FetchAssetsFromAddressables();
	}

	public void Init(int _rows, int _cols, CardInfo[] cardInfos)
	{
		rows = _rows;
		cols = _cols;
		totalcards = cardInfos.Length;

		FetchAssetsFromAddressables(cardInfos);
	}

	private void FetchAssetsFromAddressables()
    {
		totalFaceSprites = Mathf.CeilToInt((float)(rows * cols) / 2f);
		int randomFrontSpriteIndex = Random.Range(0, faceSpritesAddress.Length - totalFaceSprites);
		selectedFaceSpriteAddresses = new string[totalFaceSprites];
		Addressables.LoadAssetAsync<Sprite>(backSpriteAddress).Completed += OnBackSpriteComplete;
		for (int i = 0, j = randomFrontSpriteIndex; i < totalFaceSprites; i++)
		{
			selectedFaceSpriteAddresses[i] = faceSpritesAddress[j];
			j++;
		}
		UpdateCards();
	}

	private void FetchAssetsFromAddressables(CardInfo[] cardInfos)
	{
		totalFaceSprites = cardInfos.Length;
		selectedFaceSpriteAddresses = new string[totalFaceSprites];
		Addressables.LoadAssetAsync<Sprite>(backSpriteAddress).Completed += OnBackSpriteComplete;
		for (int i = 0; i < totalFaceSprites; i++)
		{
			selectedFaceSpriteAddresses[i] = cardInfos[i].frontSprite;
		}
		UpdateCards(cardInfos);
	}

	private void OnBackSpriteComplete(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<Sprite> obj)
	{
		backSprite = obj.Result;
		ShuffleList<Card>(cardPool);
		AddCardsOnBoard();
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
				cardPool[cardCount].UpdateData(selectedFaceSpriteAddresses[id], id);
				cardCount++;
				if (cardCount % 2 == 0)
				{
					id++;
				}
			}
		}
	}

	private void UpdateCards(CardInfo[] cardInfos)
	{
		if (cardPool.Count < totalcards)
		{
			CreateCards(totalcards - cardPool.Count);
		}
		else if (cardPool.Count > totalcards)
		{
			int extraCards = cardPool.Count - totalcards;
			HideCards(cardPool.Count - extraCards);
		}
		for (int i = 0; i < cardInfos.Length; i++)
		{
			cardPool[i].gameObject.SetActive(true);
			cardPool[i].UpdateData(cardInfos[i].frontSprite, cardInfos[i].id, (CardState)cardInfos[i].state);
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
			Debug.Log("backSprite " + backSprite);
			cardPool[i].UpdateView(backSprite);
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

	public Card[] GetCurrentCards()
    {
		List<Card> currentCards = new List<Card>(cardPool.GetRange(0, totalcards));
		return currentCards.ToArray();
	}
}
