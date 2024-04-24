using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Board board;

    private Queue<Card> selectedCards = new Queue<Card>();

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
        board.Init(_row, _col);
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

            if(card1.Id == card2.Id)
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
