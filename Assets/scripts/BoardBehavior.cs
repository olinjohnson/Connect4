using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            if(!game.turn)
            {
                // Player Move
                arrowContainer.SetActive(true);
                textContainer.SetActive(true);
                yield return new WaitUntil(() => game.turn);
                arrowContainer.SetActive(false);
                textContainer.SetActive(false);
                yield return new WaitForSeconds(1);
            }
            else
            {
                // Ai Move
                int aicoord = game.AIMove(board);
                game.makeMove(board, aicoord);
                Instantiate(redCoinPrefab, new Vector3(coinCoordsX, coinCoordsY, coinCoordsZ[aicoord]), Quaternion.identity);
                game.turn = false;
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
            game.makeMove(board, position);
            Instantiate(yellowCoinPrefab, new Vector3(coinCoordsX, coinCoordsY, coinCoordsZ[position]), Quaternion.identity);
            game.turn = true;
            return true;
        }
        else
        {
            return false;
        }
    }
}
