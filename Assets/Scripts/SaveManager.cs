using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string rowsKey = "Rows";
    private const string colsKey = "Cols";
    private const string cardInfosKey = "CardInfos";

    private CardInfo[] cardInfos;
    private int rows;
    private int cols;

    public CardInfo[] CardInfos { get { return cardInfos; } }
    public int Rows { get { return rows; } }
    public int Cols { get { return cols; } }

    public void SaveGame(Card[] cards, int _row, int _col)
    {
        rows = _row;
        cols = _col;
        int totalCards = cards.Length;
        cardInfos = new CardInfo[totalCards];
        for (int i = 0; i < totalCards; i++)
        {
            cardInfos[i] = new CardInfo();
            cardInfos[i].frontSprite = cards[i].FrontSpriteAddress;
            cardInfos[i].id = cards[i].GetId();
            cardInfos[i].state = (int)cards[i].CurrentState;
        }
        string cardsArrayJson = JsonHelper.ToJson(cardInfos);

        PlayerPrefs.SetInt(rowsKey, rows);
        PlayerPrefs.SetInt(colsKey, cols);
        PlayerPrefs.SetString(cardInfosKey, cardsArrayJson);
    }

    public bool LoadGame()
    {
        if(PlayerPrefs.HasKey(rowsKey))
        {
            rows = PlayerPrefs.GetInt(rowsKey);
        }
        else
        {
            return false;
        }
        if (PlayerPrefs.HasKey(colsKey))
        {
            cols = PlayerPrefs.GetInt(colsKey);
        }
        else
        {
            return false;
        }
        if (PlayerPrefs.HasKey(cardInfosKey))
        {
            string newJson = PlayerPrefs.GetString(cardInfosKey);
            cardInfos = new CardInfo[rows * cols];

            cardInfos = JsonHelper.FromJson<CardInfo>(newJson);
        }
        else
        {
            return false;
        }
        return true;
    }
}

[System.Serializable]
public class CardInfo
{
    public string frontSprite;
    public int id;
    public int state;
}
