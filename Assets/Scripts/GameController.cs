using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Board board;

    private void Start()
    {
        board.Init(4, 4);
    }
}
