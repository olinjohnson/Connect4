using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoardBehavior : MonoBehaviour
{
    private float[] coinCoordsZ = { 10.173f, 8.859f, 7.583f, 6.271f, 4.966f, 3.668f, 2.37f};
    private float coinCoordsX = 5.184f;
    private float coinCoordsY = 18.28f;

    public GameObject yellowCoinPrefab;
    public GameObject redCoinPrefab;
    public GameObject arrowContainer;

    public GameObject AITextContainer;
    public GameObject PlayerTextContainer;
    public TextMeshProUGUI PlayerTurnText;

    public GameObject winningText;
    public GameObject losingText;
    public GameObject drawText;
    public TextMeshProUGUI statsText;

    public Game game;
    public Board board;

    void Start()
    {
        game = new Game();
        board = new Board();
        if(PlayerPrefs.AIOpponent)
        {
            StartAIGame();
        }
        else
        {
            StartPlayerGame();
        }
    }

    void StartAIGame()
    {
        AITextContainer.SetActive(true);
        StartCoroutine(AIMoveCycle());
    }

    void StartPlayerGame()
    {
        PlayerTextContainer.SetActive(true);
        StartCoroutine(PlayerMoveCycle());
    }

    IEnumerator PlayerMoveCycle()
    {
        while(game.DetectWin(board) == 0)
        {
            if(!board.turn)
            {
                // Player 1 Move
                PlayerTurnText.text = "Player 1's turn";
                arrowContainer.SetActive(true);
                yield return new WaitUntil(() => board.turn);
                arrowContainer.SetActive(false);
                yield return new WaitForSeconds(1);
            }
            else
            {
                // Player 2 Move
                PlayerTurnText.text = "Player 2's turn";
                arrowContainer.SetActive(true);
                yield return new WaitUntil(() => !board.turn);
                arrowContainer.SetActive(false);
                yield return new WaitForSeconds(1);
            }
        }

        GameObject.Find("TurnInfo").SetActive(false);
        PlayerTurnText.enabled = true;
        // Check what time of endgame
        if (game.DetectWin(board) == 1)
        {
            PlayerTurnText.text = "Player 1 wins!";
        }
        else if (game.DetectWin(board) == 2)
        {
            PlayerTurnText.text = "Player 2 wins!";
        }
        else
        {
            PlayerTurnText.text = "It's a draw!";
        }
    }

    IEnumerator AIMoveCycle()
    {
        while(game.DetectWin(board) == 0)
        {
            if(!board.turn)
            {
                // Player Move
                arrowContainer.SetActive(true);
                AITextContainer.SetActive(true);
                yield return new WaitUntil(() => board.turn);
                arrowContainer.SetActive(false);
                AITextContainer.SetActive(false);
                yield return new WaitForSeconds(1);
            }
            else
            {
                // Ai Move
                int aicoord = game.AIMove(board, PlayerPrefs.searchDepth);
                board = game.MakeMove(board, aicoord);
                Instantiate(redCoinPrefab, new Vector3(coinCoordsX, coinCoordsY, coinCoordsZ[aicoord]), Quaternion.identity);
                if(PlayerPrefs.showStats)
                {
                    statsText.text = "Nodes searched: " + PlayerPrefs.nodesSearched + "      Time spent: " + PlayerPrefs.timeSearched + " ms";
                }
                yield return new WaitForSeconds(1);
            }
        }

        // Check what time of endgame
        if (game.DetectWin(board) == 1)
        {
            winningText.SetActive(true);
        }
        else if(game.DetectWin(board) == 2)
        {
            losingText.SetActive(true);
        }
        else
        {
            drawText.SetActive(true);
        }
    }

    public bool MakePlayerMove(int position, GameObject coinPrefab)
    {
        if (game.ValidMove(board, position))
        {
            board = game.MakeMove(board, position);
            Instantiate(coinPrefab, new Vector3(coinCoordsX, coinCoordsY, coinCoordsZ[position]), Quaternion.identity);
            return true;
        }
        else
        {
            return false;
        }
    }
}
