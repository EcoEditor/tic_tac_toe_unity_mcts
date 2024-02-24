using System;
using UnityEngine;

public class BoardMarks : MonoBehaviour
{
    private Board _board;
    private SpriteRenderer _spriteRenderer;
    private int _x;
    private int _y;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void AssignValue(Board board, int x, int y)
    {
        _board = board; 
        _x = x;
        _y = y;
    }

    public void Click()
    {
        // the human player is the only one who interacts and always = 1
        if (_board.IsEmptyPosition(_x, _y))
        {
            _board.PerformMove(1,_x,_y);
        }
    }

    public void UpdateView(Sprite playerMark)
    {
        _spriteRenderer.sprite = playerMark;
    }

    public int X => _x;
    public int Y => _y;
}
