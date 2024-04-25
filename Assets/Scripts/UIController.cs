using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] private Canvas menuCanvas;
    [SerializeField] SaveManager saveManager;
    [SerializeField] GameObject saveButton;
    [SerializeField] GameObject winPopup;

    private void Awake()
    {
        gameController.OnGameComplete += GameComplete;
    }

    private void OnDestroy()
    {
        gameController.OnGameComplete -= GameComplete;
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
        saveButton.SetActive(true);
        winPopup.SetActive(false);
    }

    private void GameComplete()
    {
        saveButton.SetActive(false);
        winPopup.SetActive(true);
    }
}
