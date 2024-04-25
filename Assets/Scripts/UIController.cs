using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private Canvas menuCanvas;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private Button saveButton;
    [SerializeField] private GameObject winPopup;
    [SerializeField] private TextMeshProUGUI scoreText;

    private StringBuilder builder = new StringBuilder(50);

    private void Awake()
    {
        gameController.OnGameComplete += GameComplete;
        gameController.OnScoreUpdated += UpdateScore;
    }

    private void OnDestroy()
    {
        gameController.OnGameComplete -= GameComplete;
        gameController.OnScoreUpdated -= UpdateScore;
    }

    public void InitBoard2x2()
    {
        gameController.InitBoard(2, 2);
        DisableMenu();
    }
    public void InitBoard2x3()
    {
        gameController.InitBoard(2, 3);
        DisableMenu();
    }
    public void InitBoard4x4()
    {
        gameController.InitBoard(4, 4);
        DisableMenu();
    }
    public void InitBoard5x4()
    {
        gameController.InitBoard(5, 4);
        DisableMenu();
    }

    public void SaveGame()
    {
        saveManager.SaveGame(gameController.Cards, gameController.Rows, gameController.Cols);
    }

    public void LoadGame()
    {
        if(saveManager.LoadGame())
        {
            gameController.LoadGame(saveManager.Rows, saveManager.Cols, saveManager.CardInfos);
            DisableMenu();
        }
        else
        {
            Debug.LogError("Game was not saved, hence could not load");
        }
    }

    public void EnableMenu()
    {
        menuCanvas.gameObject.SetActive(true);
        menuCanvas.enabled = true;
    }

    private void DisableMenu()
    {
        menuCanvas.enabled = false;
        menuCanvas.gameObject.SetActive(false);
        saveButton.interactable = true;
        winPopup.SetActive(false);
        scoreText.text = "Score : 0 Streak : 0";
    }

    private void GameComplete()
    {
        saveButton.interactable = false;
        winPopup.SetActive(true);
    }

    private void UpdateScore(int score, int streak)
    {
        builder.Length = 0;
        builder.Append("Score : ");
        builder.Append(score);
        builder.Append(" Streak : ");
        builder.Append(streak);
        scoreText.text = builder.ToString();
    }
}
