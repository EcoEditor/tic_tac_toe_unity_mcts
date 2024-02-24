using System;
using System.Collections.Generic;
using tic_tac_toe_unity_mcts;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Board : MonoBehaviour
{
    public Action<int> PlayerTurn;
    private const int ROWS = 3;
    private const int COLUMNS = 3;

    [SerializeField] private GameObject linePrefab;
    [SerializeField] private BoardMarks boardMarkPrefabRef;
    [SerializeField] private float offset = 10f;
    [SerializeField] private Sprite[] markSprites;

    private MonteCarloTreeSearch _mcts;
    private tic_tac_toe_unity_mcts.Board _board;
    private int[,] _boardValues;
    private float _markPrefabScale;
    
    private HashSet<BoardMarks> _boardMarks;

    private void Awake()
    {
        _board = new tic_tac_toe_unity_mcts.Board();
        _mcts = new MonteCarloTreeSearch();
        _boardMarks = new HashSet<BoardMarks>();
        
        InitializeBoard();
        DrawBoard();
    }

    private void InitializeBoard()
    {
        _boardValues = new int[ROWS, COLUMNS];

        for (int i = 0; i < ROWS; i++)
        {
            for (int j = 0; j < COLUMNS; j++)
            {
                _boardValues[i, j] = 0;
            }
        }
    }

    private void DrawBoard()
    {
        // total of 2x2 lines 
        for (int i = 0; i < ROWS - 1; i++)
        {
            for (int j = 0; j < COLUMNS - 1; j++)
            {
                var position = new Vector2();
                var rotation = new Vector3();

                if (i % 2 == 0 && j % 2 == 0)
                {
                    position = new Vector2(2, (offset * 0.5f));
                    rotation = new Vector3(0f, 0f, 90f);
                } else if (i % 2 == 0 && j % 2 != 0)
                {
                    position = new Vector2(2, -(offset * 0.5f));
                    rotation = new Vector3(0f, 0f, 90f);
                } else if (i % 2 != 0 && j % 2 == 0)
                {
                    position = new Vector2(0, 0);
                } else
                {
                    position = new Vector2(offset, 0);
                }

                var line = Instantiate(linePrefab, transform);
                line.transform.position = position;
                line.transform.eulerAngles = rotation;
            }
        }

        var initialPosition = new Vector2(-2, 4);
        for (int i = 0; i < ROWS; i++)
        {
            for (int j = 0; j < COLUMNS; j++)
            {
                var mark = Instantiate(boardMarkPrefabRef, transform);
                mark.AssignValue(this, i,j);
                var position = new Vector2(initialPosition.x + (j * (initialPosition.y)), initialPosition.y - (initialPosition.y * i));
                mark.transform.position = position;
                _boardMarks.Add(mark);
            }

            _markPrefabScale = boardMarkPrefabRef.transform.localScale.x;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log($"Mouse position { position}");
            var boardValue = GetMarkByPosition(position);
            boardValue.Click();
        }
    }

    public void PerformMove(int player, int x, int y)
    {
        // Update ai board
        var p = new Position(x, y);
        _board.PerformMove(player, p);
        _boardValues[x, y] = player;
        GetMarkByValue(x,y).UpdateView(markSprites[player-1]);
        PlayerTurn?.Invoke(player);
    }
    
    public bool IsEmptyPosition(int row, int col) {
        return _boardValues[row, col] == 0;
    }

    private BoardMarks GetMarkByPosition(Vector2 position)
    {
        foreach (var mark in _boardMarks)
        {
            var distance = Vector2.Distance(position, mark.transform.position);
            if (distance <= _markPrefabScale)
            {
                return mark;
            }
        }

        return null;
    }

    private BoardMarks GetMarkByValue(int x, int y)
    {
        foreach (var mark in _boardMarks)
        {
            if (mark.X == x && mark.Y == y)
            {
                return mark;
            }
        }

        return null;
    }

    public tic_tac_toe_unity_mcts.Board MctsBoard => _board;
    public MonteCarloTreeSearch MCTS => _mcts;
}