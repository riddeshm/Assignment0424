using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public event Action OnGameComplete;
    public event Action<int, int> OnScoreUpdated;

    [SerializeField] private Board board;

    private Queue<Card> selectedCards = new Queue<Card>();

    public int Rows { get { return board.Rows; } }
    public int Cols { get { return board.Cols; } }
    public Card[] Cards { get { return board.Cards; } }

    public int matchCount = 0;

    private int score = 0;
    private int streak = 0;

    private int correctPoints = 10;
    private int incorrectPoints = 5;

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
        Init();
        board.Init(_row, _col);
    }

    public void LoadGame(int _row, int _col, CardInfo[] cardInfos)
    {
        Init();
        board.Init(_row, _col, cardInfos);
    }

    private void Init()
    {
        score = 0;
        streak = 0;
        matchCount = 0;
        selectedCards.Clear();
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
                matchCount++;
                streak++;
                score += (correctPoints * streak);
                board.CorrectPair(card1, card2);
            }
            else
            {
                if(score >= incorrectPoints)
                {
                    score -= incorrectPoints;
                }
                streak = 0;
                board.InCorrectPair(card1, card2);
            }
        }
        OnScoreUpdated?.Invoke(score, streak);
        CheckWin();
    }

    private void CheckWin()
    {
        if(matchCount >= Mathf.CeilToInt((float)(Rows * Cols) / 2f))
        {
            OnGameComplete?.Invoke();
        }
    }
}
