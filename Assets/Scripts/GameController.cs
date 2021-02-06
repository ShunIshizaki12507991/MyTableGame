using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private int[,] board;

    private enum cellConds : int{
        Player1 = 1,
        Player2 = 2,
        None = 0
    };

    // Start is called before the first frame update
    void Start()
    {
        InitializeGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeGame()
    {
        int Player1 = (int)cellConds.Player1;
        int Player2 = (int)cellConds.Player2;
        int None = (int)cellConds.None;
        board = new int[6, 6] {
            {Player1, Player1, Player1, Player1, Player1, Player1},
            {None, None, None, None, None, None},
            {None, None, None, None, None, None},
            {None, None, None, None, None, None},
            {None, None, None, None, None, None},
            {Player2, Player2, Player2, Player2, Player2, Player2}
        };
    }
}
