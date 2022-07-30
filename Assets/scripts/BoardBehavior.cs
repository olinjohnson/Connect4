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
    public GameObject textContainer;

    public GameObject winningText;
    public GameObject losingText;
    public GameObject drawText;
    public TextMeshProUGUI statsText;

    private Game game;
    private Board board;

    void Start()
    {
        game = new Game();
        board = new Board();
        StartAIGame();
    }

    void StartAIGame()
    {
        StartCoroutine(MoveCycle());
    }

    IEnumerator MoveCycle()
    {
        while(game.DetectWin(board) == 0)
        {
            if(!board.turn)
            {
                // Player Move
                arrowContainer.SetActive(true);
                textContainer.SetActive(true);
                yield return new WaitUntil(() => board.turn);
                arrowContainer.SetActive(false);
                textContainer.SetActive(false);
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

    public bool MakePlayerMove(int position)
    {
        if (game.ValidMove(board, position))
        {
            board = game.MakeMove(board, position);
            Instantiate(yellowCoinPrefab, new Vector3(coinCoordsX, coinCoordsY, coinCoordsZ[position]), Quaternion.identity);
            return true;
        }
        else
        {
            return false;
        }
    }
}
