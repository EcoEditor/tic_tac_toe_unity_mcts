using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private const int HUMAN_PLAYER = 1;
    private const int AI_PLAYER = 2;
    
    [SerializeField] private Board boardRef;
    [SerializeField] private GameObject restartButton;

    private int curPlayer;

    private void Awake()
    {
        boardRef = Instantiate(boardRef);
        boardRef.PlayerTurn += PlayerTurn;
    }

    private void OnDestroy()
    {
        boardRef.PlayerTurn -= PlayerTurn;
    }

    private void PlayerTurn(int turn)
    {
        if (boardRef.MctsBoard.CheckStatus() != -1)
        {
            EndGame();
            return;
        }
        
        curPlayer = 3 - turn;

        if (curPlayer == AI_PLAYER)
        {
            var board = boardRef.MCTS.FindNextMove(boardRef.MctsBoard, curPlayer);
            var lastMove = board.LastPosition;
            boardRef.PerformMove(curPlayer, lastMove.X, lastMove.Y);
        } else
        {
            Debug.Log("Waiting for human player to play");
        }
    }

    private void EndGame()
    {
        var status = boardRef.MctsBoard.CheckStatus();
        if (status == HUMAN_PLAYER)
        {
            Debug.Log("PLAYER OF TYPE HUMAN HAS WON!");
        } else if (status == AI_PLAYER)
        {
            Debug.Log("PLAYER OF TYPE AI HAS WON!");
        } else if (status == 0)
        {
            Debug.Log("GAME DRAW.");
        } else if (status == -1)
        {
            Debug.Log("OOOOOOOOHHHHHH, game in progress but it reached end game....");
        }

        restartButton.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}