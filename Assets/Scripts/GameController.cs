using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.SerializableAttribute]
public class CellArray
{
    public Cell[] m_Cells;
}

/// <summary>
/// 
/// </summary>
public class GameController : MonoBehaviour
{
    private enum CELLSTATUS{
        NONE = -1,
        WALL = 0,
        PLAYER1 = 1,
        PLAYER2 = 2,
    };
    private CELLSTATUS[,] m_BoardStatus;
    private enum TURNPHASE
    {
        NONE = -1,
        SELECTKING = 0,
        SELECTPIECE,
        SELECTMOVEMENT,
    };
    private TURNPHASE m_Phase;
    private int m_OwnKingIndex = -1;
    private int m_OpponentKingIndex = -1;
    private int m_PlayTurnCount;
    private List<Cell> m_MovableCellList = new List<Cell>();
    private int m_SelectedPieceIndex = -1;
    private enum HANDTURN
    {
        PLAYER1TURN = 1,
        PLAYER2TURN = 2,
    };
    private HANDTURN m_CurrentTurn;
    private bool[,] m_Checked;
    private Queue<string> m_CommandQueue;

    [UnityEngine.SerializeField]
    public bool m_IsDebug = false;
    public bool m_IsGameStarting;
    public Button m_SelectButton;
    public PieceController[] m_Player1Pieces;
    public PieceController[] m_Player2Pieces;
    public CellArray[] m_Board;
    public GameObject m_DialogPrefab;

    // Start is called before the first frame update
    void Start()
    {
        m_SelectButton.interactable = false;
        m_SelectButton.onClick.AddListener(OnClickSelectButton);
        InitializeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (m_IsGameStarting)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();
                switch (m_CurrentTurn)
                {
                    case HANDTURN.PLAYER1TURN:
                        switch (m_Phase)
                        {
                            case TURNPHASE.NONE:
                                break;

                            case TURNPHASE.SELECTPIECE:
                                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("Piece1"))
                                {
                                    PieceController targetPiece = hit.collider.gameObject.GetComponent<PieceController>();
                                    ShowMovableCells(targetPiece);
                                    m_SelectedPieceIndex = Array.IndexOf(m_Player1Pieces, targetPiece);
                                    m_Phase = TURNPHASE.SELECTMOVEMENT;
                                }
                                break;

                            case TURNPHASE.SELECTMOVEMENT:
                                if (Physics.Raycast(ray, out hit))
                                {
                                    switch (hit.collider.gameObject.tag)
                                    {
                                        case "Piece1":
                                            foreach (Cell cell in m_MovableCellList)
                                            {
                                                cell.HideMovableMark();
                                            }
                                            m_MovableCellList.Clear();
                                            PieceController targetPiece = hit.collider.gameObject.GetComponent<PieceController>();
                                            ShowMovableCells(targetPiece);
                                            m_SelectedPieceIndex = Array.IndexOf(m_Player1Pieces, targetPiece);
                                            break;
                                        case "Cell":
                                            Cell targetCell = hit.collider.gameObject.GetComponent<Cell>();
                                            if (!m_MovableCellList.Contains(targetCell))
                                            {
                                                break;
                                            }
                                            MovePiece(targetCell);
                                            foreach (Cell cell in m_MovableCellList)
                                            {
                                                cell.HideMovableMark();
                                            }
                                            m_MovableCellList.Clear();
                                            m_PlayTurnCount++;
                                            PieceController king = m_Player2Pieces[m_OpponentKingIndex];
                                            if (CheckSurrounded(CELLSTATUS.PLAYER2, king.m_PositionX + 1, king.m_PositionY + 1))
                                            {
                                                GameOver();
                                            }
                                            ClearChecker();
                                            m_Phase = TURNPHASE.SELECTPIECE;
                                            m_CurrentTurn = HANDTURN.PLAYER2TURN;
                                            break;
                                    }
                                }
                                break;
                        }
                        break;

                    case HANDTURN.PLAYER2TURN:
                        if (m_IsDebug)
                        {
                            switch (m_Phase)
                            {
                                case TURNPHASE.SELECTPIECE:
                                    if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("Piece2"))
                                    {
                                        PieceController targetPiece = hit.collider.gameObject.GetComponent<PieceController>();
                                        ShowMovableCells(targetPiece);
                                        m_SelectedPieceIndex = Array.IndexOf(m_Player2Pieces, targetPiece);
                                        m_Phase = TURNPHASE.SELECTMOVEMENT;
                                    }
                                    break;

                                case TURNPHASE.SELECTMOVEMENT:
                                    if (Physics.Raycast(ray, out hit))
                                    {
                                        Debug.Log(hit.collider.gameObject.tag);
                                        switch (hit.collider.gameObject.tag)
                                        {
                                            case "Piece2":
                                                foreach (Cell cell in m_MovableCellList)
                                                {
                                                    cell.HideMovableMark();
                                                }
                                                m_MovableCellList.Clear();
                                                PieceController targetPiece = hit.collider.gameObject.GetComponent<PieceController>();
                                                ShowMovableCells(targetPiece);
                                                m_SelectedPieceIndex = Array.IndexOf(m_Player2Pieces, targetPiece);
                                                break;
                                            case "Cell":
                                                Cell targetCell = hit.collider.gameObject.GetComponent<Cell>();
                                                if (!m_MovableCellList.Contains(targetCell))
                                                {
                                                    break;
                                                }
                                                MovePiece(targetCell);
                                                m_Phase = TURNPHASE.SELECTPIECE;
                                                foreach (Cell cell in m_MovableCellList)
                                                {
                                                    cell.HideMovableMark();
                                                }
                                                m_MovableCellList.Clear();
                                                m_PlayTurnCount++;
                                                m_CurrentTurn = HANDTURN.PLAYER1TURN;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                }

            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("Piece1"))
                {
                    if (m_OwnKingIndex != -1)
                    {
                        m_Player1Pieces[m_OwnKingIndex].ResetKing();
                    }
                    PieceController target = hit.collider.gameObject.GetComponent<PieceController>();
                    target.OnChooseAsKingWithCone();
                    m_OwnKingIndex = Array.IndexOf(m_Player1Pieces, target);
                    m_SelectButton.interactable = true;
                }
            }
        }
        else
        {
            if (m_IsGameStarting)
            {
                switch (m_CurrentTurn)
                {
                    case HANDTURN.PLAYER1TURN:
                        break;

                    case HANDTURN.PLAYER2TURN:
                        //if (!m_IsDebug) {
                        //    m_SelectedPieceIndex = UnityEngine.Random.Range(0, 6);
                        //    List<Cell> cells = GetMovableCells(m_Player2Pieces[m_SelectedPieceIndex]);
                        //    while (cells.Count == 0)
                        //    {
                        //        m_SelectedPieceIndex = UnityEngine.Random.Range(0, 6);
                        //        cells = GetMovableCells(m_Player2Pieces[m_SelectedPieceIndex]);
                        //    }
                        //    int randomSelectIndex = UnityEngine.Random.Range(0, cells.Count);
                        //    MovePiece(cells[randomSelectIndex]);
                        //    PieceController king = m_Player1Pieces[m_OwnKingIndex];
                        //    if (CheckSurrounded(CELLSTATUS.PLAYER1, king.m_PositionX + 1, king.m_PositionY + 1)) {
                        //        GameOver();
                        //    }
                        //    ClearChecker();
                        //    m_Phase = TURNPHASE.SELECTPIECE;
                        //    m_CurrentTurn = HANDTURN.PLAYER1TURN;
                        //}
                        if (!m_IsDebug) {
                            switch (m_Phase)
                            {
                                case TURNPHASE.SELECTPIECE:
                                    m_SelectedPieceIndex = UnityEngine.Random.Range(0, 6);
                                    m_MovableCellList = GetMovableCells(m_Player2Pieces[m_SelectedPieceIndex]);
                                    while (m_MovableCellList.Count == 0)
                                    {
                                        m_SelectedPieceIndex = UnityEngine.Random.Range(0, 6);
                                        m_MovableCellList = GetMovableCells(m_Player2Pieces[m_SelectedPieceIndex]);
                                    }
                                    //m_SelectedPieceIndex = UnityEngine.Random.Range(0, m_MovableCellList.Count);
                                    m_Phase = TURNPHASE.SELECTMOVEMENT;
                                    break;

                                case TURNPHASE.SELECTMOVEMENT:
                                    int randomSelectIndex = UnityEngine.Random.Range(0, m_MovableCellList.Count);
                                    MovePiece(m_MovableCellList[randomSelectIndex]);
                                    PieceController king = m_Player1Pieces[m_OwnKingIndex];
                                    if (CheckSurrounded(CELLSTATUS.PLAYER1, king.m_PositionX + 1, king.m_PositionY + 1))
                                    {
                                        GameOver();
                                    }
                                    ClearChecker();
                                    m_MovableCellList.Clear();
                                    m_Phase = TURNPHASE.SELECTPIECE;
                                    m_CurrentTurn = HANDTURN.PLAYER1TURN;
                                    break;
                            }
                        }
                        break;
                }
            }
        }

        if (Input.anyKeyDown)
        {
            if (!m_IsGameStarting) {
                if (Input.GetKey("b"))
                {
                    Debug.Log("b");
                    m_CommandQueue.Enqueue("b");
                }
                else if (Input.GetKey("d"))
                {
                    Debug.Log("d");
                    m_CommandQueue.Enqueue("d");
                }
                else if (Input.GetKey("e"))
                {
                    Debug.Log("e");
                    m_CommandQueue.Enqueue("e");
                }
                else if(Input.GetKey("g"))
                {
                    Debug.Log("g");
                    m_CommandQueue.Enqueue("g");
                }
                else if(Input.GetKey("u"))
                {
                    Debug.Log("u");
                    m_CommandQueue.Enqueue("u");
                }
                else if (Input.GetKey("return"))
                {
                    Debug.Log("Enter Command");
                    string command = "";
                    foreach (string c in m_CommandQueue)
                    {
                        command += c;
                    }
                    ConfirmCommand(command);
                    m_CommandQueue.Clear();
                }
            }
        }
    }

    /// <summary>
    /// 盤面を初期化するメソッド。
    /// </summary>
    private void InitializeGame()
    {
        CELLSTATUS none = CELLSTATUS.NONE;
        CELLSTATUS wall = CELLSTATUS.WALL;
        CELLSTATUS player1 = CELLSTATUS.PLAYER1;
        CELLSTATUS player2 = CELLSTATUS.PLAYER2;
        m_BoardStatus = new CELLSTATUS[8, 8] {
            { wall, wall   , wall   , wall   , wall   , wall   , wall   , wall },
            { wall, player2, player2, player2, player2, player2, player2, wall },
            { wall, none   , none   , none   , none   , none   , none   , wall },
            { wall, none   , none   , none   , none   , none   , none   , wall },
            { wall, none   , none   , none   , none   , none   , none   , wall },
            { wall, none   , none   , none   , none   , none   , none   , wall },
            { wall, player1, player1, player1, player1, player1, player1, wall },
            { wall, wall   , wall   , wall   , wall   , wall   , wall   , wall }
        };
        m_PlayTurnCount = 1;
        m_IsGameStarting = false;
        m_Checked = new bool[6, 6] {
            { false, false, false, false, false, false },
            { false, false, false, false, false, false },
            { false, false, false, false, false, false },
            { false, false, false, false, false, false },
            { false, false, false, false, false, false },
            { false, false, false, false, false, false }
        };
        m_Phase = TURNPHASE.NONE;
        m_CommandQueue = new Queue<string>();
    }

    /// <summary>
    /// 王将決定ボタンを押下したときによばれるメソッド。
    /// </summary>
    public void OnClickSelectButton()
    {
        if (!m_IsGameStarting)
        {
            m_IsGameStarting = true;
            m_SelectButton.interactable = false;
            GameObject.Find("Button").SetActive(false);
            GameObject dialog = (GameObject)Instantiate(m_DialogPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            dialog.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
            //m_CurrentTurn = (HANDTURN)UnityEngine.Random.Range(1, 3);
            m_CurrentTurn = (HANDTURN)2;
            dialog.GetComponent<GameStartDialogContoller>().SetDialogMessage((int)m_CurrentTurn);
            m_OpponentKingIndex = UnityEngine.Random.Range(0, 6);
            if (m_IsDebug) {
                m_Player2Pieces[m_OpponentKingIndex].OnChooseAsKingWithCone();
            }
            else
            {
                m_Player2Pieces[m_OpponentKingIndex].OnChooseAsKing();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnSelectedKing()
    {
        m_Phase = TURNPHASE.SELECTPIECE;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private List<Cell> GetMovableCells(PieceController target)
    {
        List<Cell> ret = new List<Cell>();

        int indent = 1;
        Cell cell = m_Board[target.m_PositionY].m_Cells[target.m_PositionX];

        int x = target.m_PositionX + indent;
        int y = target.m_PositionY + indent;
        // 右方向に走査
        for (int i = x + 1; i <= 6; i++)
        {
            switch (m_BoardStatus[y, i])
            {
                case CELLSTATUS.NONE:
                    Cell c = m_Board[target.m_PositionY].m_Cells[i - 1];
                    ret.Add(c);
                    break;

                case CELLSTATUS.PLAYER1:
                case CELLSTATUS.PLAYER2:
                case CELLSTATUS.WALL:
                    i = 7;
                    break;
            }
        }

        // 左方向に走査
        for (int i = x - 1; i >= 1; i--)
        {
            switch (m_BoardStatus[y, i])
            {
                case CELLSTATUS.NONE:
                    Cell c = m_Board[target.m_PositionY].m_Cells[i - 1];
                    ret.Add(c);
                    break;

                case CELLSTATUS.PLAYER1:
                case CELLSTATUS.PLAYER2:
                case CELLSTATUS.WALL:
                    i = -1;
                    break;
            }
        }

        // 下方向に走査
        for (int i = y + 1; i <= 6; i++)
        {
            switch (m_BoardStatus[i, x])
            {
                case CELLSTATUS.NONE:
                    Cell c = m_Board[i - 1].m_Cells[target.m_PositionX];
                    ret.Add(c);
                    break;

                case CELLSTATUS.PLAYER1:
                case CELLSTATUS.PLAYER2:
                case CELLSTATUS.WALL:
                    i = 7;
                    break;
            }
        }

        // 上方向に走査
        for (int i = y - 1; i >= 1; i--)
        {
            switch (m_BoardStatus[i, x])
            {
                case CELLSTATUS.NONE:
                    Cell c = m_Board[i - 1].m_Cells[target.m_PositionX];
                    ret.Add(c);
                    break;

                case CELLSTATUS.PLAYER1:
                case CELLSTATUS.PLAYER2:
                case CELLSTATUS.WALL:
                    i = -1;
                    break;
            }
        }

        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    private void ShowMovableCells(PieceController target)
    {
        m_MovableCellList.AddRange(GetMovableCells(target));
        foreach (Cell cell in m_MovableCellList)
        {
            cell.DisplayMovableMark();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetCell"></param>
    private void MovePiece(Cell targetCell)
    {
        Vector3 startPos;
        Vector3 targetPos = targetCell.transform.localPosition;
        float delX, delY;
        PieceController targetPiece;
        switch (m_CurrentTurn)
        {
            case HANDTURN.PLAYER1TURN:
                targetPiece = m_Player1Pieces[m_SelectedPieceIndex];
                m_BoardStatus[targetCell.m_PositionY + 1, targetCell.m_PositionX + 1] = CELLSTATUS.PLAYER1;
                break;

            case HANDTURN.PLAYER2TURN:
                targetPiece = m_Player2Pieces[m_SelectedPieceIndex];
                m_BoardStatus[targetCell.m_PositionY + 1, targetCell.m_PositionX + 1] = CELLSTATUS.PLAYER2;
                break;
            default:
                return;
        }
        startPos = targetPiece.transform.localPosition;
        delX = targetPos.x - startPos.x;
        delY = targetPos.y - startPos.y;
        targetPiece.transform.Translate(delX, delY, 0.0f);
        m_BoardStatus[targetPiece.m_PositionY + 1, targetPiece.m_PositionX + 1] = CELLSTATUS.NONE;
        targetPiece.m_PositionX = targetCell.m_PositionX;
        targetPiece.m_PositionY = targetCell.m_PositionY;
        Debug.Log(m_CurrentTurn + " (" + startPos.x + ", " + startPos.y + ") -> (" + targetPos.x + ", " + targetPos.y + ")");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool CheckSurrounded(CELLSTATUS piece, int x, int y)
    {
        bool ret;

        if (m_Checked[y - 1, x - 1] == true)
        {
            return true;
        }

        m_Checked[y - 1, x - 1] = true;

        if (m_BoardStatus[y, x] == CELLSTATUS.NONE)
        {
            return false;
        }

        if (m_BoardStatus[y, x] == piece)
        {
            if (m_BoardStatus[y, x - 1] != CELLSTATUS.WALL)
            {
                ret = CheckSurrounded(piece, x - 1, y);
                if (ret == false)
                {
                    return false;
                }
            }
            if (m_BoardStatus[y - 1, x] != CELLSTATUS.WALL)
            {
                ret = CheckSurrounded(piece, x, y - 1);
                if (ret == false)
                {
                    return false;
                }
            }
            if (m_BoardStatus[y, x + 1] != CELLSTATUS.WALL)
            {
                ret = CheckSurrounded(piece, x + 1, y);
                if (ret == false)
                {
                    return false;
                }
            }
            if (m_BoardStatus[y + 1, x] != CELLSTATUS.WALL)
            {
                ret = CheckSurrounded(piece, x, y + 1);
                if (ret == false)
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    private void ClearChecker()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                m_Checked[i, j] = false;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void GameOver()
    {
        switch (m_CurrentTurn) {
            case HANDTURN.PLAYER1TURN:
                Debug.Log("Player1 Win");
                break;

            case HANDTURN.PLAYER2TURN:
                Debug.Log("Player2 Win");
                break;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ConfirmCommand(string commandString)
    {
        switch (commandString)
        {
            case "debug":
                SwitchDebugMode();
                break;

            default:
                break;
        }
    }

    private void SwitchDebugMode()
    {
        m_IsDebug = !m_IsDebug;
    }
}
