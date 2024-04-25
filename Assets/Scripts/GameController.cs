using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Board board;

    private Queue<Card> selectedCards = new Queue<Card>();

    public int Rows { get { return board.Rows; } }
    public int Cols { get { return board.Cols; } }
    public Card[] Cards { get { return board.Cards; } }

    private void Start()
    {
        board.OnCardSelected += CardSelected;
    }

    private void OnDestroy()
    {
        board.OnCardSelected -= CardSelected;
    }

    public void InitBoard(int _row, int _col)
    {
        selectedCards.Clear();
        board.Init(_row, _col);
    }

    public void LoadGame(int _row, int _col, CardInfo[] cardInfos)
    {
        selectedCards.Clear();
        board.Init(_row, _col, cardInfos);
    }

    private void CardSelected(Card _card)
    {
        selectedCards.Enqueue(_card);
        ValidatePair();
    }

    private void ValidatePair()
    {
        while(selectedCards.Count > 1)
        {
            Card card1 = selectedCards.Dequeue();
            Card card2 = selectedCards.Dequeue();

            if(card1.GetId() == card2.GetId())
            {
                board.CorrectPair(card1, card2);
            }
            else
            {
                board.InCorrectPair(card1, card2);
            }
        }
    }
}
