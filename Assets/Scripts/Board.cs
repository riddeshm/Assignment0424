using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
	[SerializeField] private Sprite backSprite;
	[SerializeField] private Sprite[] faceSprites;
	[SerializeField] private GameObject inputFieldPrefab;
	private int rows;
	private int cols;
	private Card[] cards;
	

	private Sprite[] selectedFaceSprites;
	private RectTransform parentRect;
	private GridLayoutGroup gridLayout;

	private void Awake()
	{
		parentRect = gameObject.GetComponent<RectTransform>();
		gridLayout = gameObject.GetComponent<GridLayoutGroup>();
		
	}

	public void Init(int _rows, int _cols)
    {
		rows = _rows;
		cols = _cols;
		int totalFaceSprites = (rows * cols) / 2;
		int randomFrontSpriteIndex = Random.Range(0, faceSprites.Length - totalFaceSprites);
		selectedFaceSprites = new Sprite[totalFaceSprites];
		cards = new Card[rows * cols];


		for (int i = 0, j = randomFrontSpriteIndex; i < totalFaceSprites; i++)
        {
			selectedFaceSprites[i] = faceSprites[j];
			j++;
		}

		CreateCards();
		ShuffleArray<Card>(cards);
		AddCardsOnBoard();
	}

	private void CreateCards()
    {
		gridLayout.cellSize = new Vector2(parentRect.rect.width / cols, parentRect.rect.height / rows);

		int cardCount = 0;
		int id = 0;
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				GameObject cardGo = Instantiate(inputFieldPrefab);
				Card card = cardGo.GetComponent<Card>();
				card.UpdateCards(selectedFaceSprites[id], backSprite, id);
				cards[cardCount] = card;
				cardCount++;
				if (cardCount % 2 == 0)
				{
					id++;
				}
			}
		}
	}

	private void ShuffleArray<T>(T[] array)
	{
		System.Random random = new System.Random();

		for (int i = array.Length - 1; i > 0; i--)
		{
			int randomIndex = random.Next(0, i);
			T temp = array[randomIndex];
			array[randomIndex] = array[i];
			array[i] = temp;
		}
	}

	private void AddCardsOnBoard()
	{
		for (int i = 0; i < cards.Length; i++)
		{
			cards[i].transform.SetParent(transform, false);
		}
	}
}
