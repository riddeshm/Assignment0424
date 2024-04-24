using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private Canvas menuCanvas;

    private Queue<Card> selectedCards = new Queue<Card>();

    private void Start()
    {
        board.OnCardSelected += CardSelected;
    }

    private void OnDestroy()
    {
        board.OnCardSelected -= CardSelected;
    }

    public void InitBoard2x2()
    {
        board.Init(2, 2);
        DisableMenu();
    }
    public void InitBoard2x3()
    {
        board.Init(2, 3);
        DisableMenu();
    }
    public void InitBoard4x4()
    {
        board.Init(4, 4);
        DisableMenu();
    }
    public void InitBoard5x4()
    {
        board.Init(5, 4);
        DisableMenu();
    }

    private void DisableMenu()
    {
        menuCanvas.enabled = false;
        menuCanvas.gameObject.SetActive(false);
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
