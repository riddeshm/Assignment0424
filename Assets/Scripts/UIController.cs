using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] private Canvas menuCanvas;

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

    public void EnableMenu()
    {
        menuCanvas.gameObject.SetActive(true);
        menuCanvas.enabled = true;
    }

    private void DisableMenu()
    {
        menuCanvas.enabled = false;
        menuCanvas.gameObject.SetActive(false);
    }
}
