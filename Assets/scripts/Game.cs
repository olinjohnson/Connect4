using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game {

    public bool turn = false;

    public int DetectWin(Board b)
    {
        int[] p_directions = {6, 5, 1, 7};
        foreach(int d in p_directions)
        {
            // Could be vectorized
            ulong y_pair = b.yellow_pos & (b.yellow_pos >> d);
            ulong r_pair = b.red_pos & (b.red_pos >> d);
            if ((y_pair & (y_pair >> (2 * d))) != 0)
            {
                return 1;
            }
            if((r_pair & (r_pair >> (2 * d))) != 0)
            {
                return 2;
            }
        }

        // Check if the board is full and it's a draw
        for(int i = 0; i < 7; i++)
        {
            if(ValidMove(b, i))
            {
                return 0;
            }
        }

        return 3;
    }

    public bool ValidMove(Board b, int col)
    {
        // Retrieve column in question
        ulong pieces = b.red_pos | b.yellow_pos;
        ulong column = (((ulong)0x3F << (6 * col)) & pieces) >> (6 * col);
        // Check if column is full
        if(column < 0x20)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void makeMove(Board b, int col)
    {
        // Retrieve column in question
        ulong pieces = b.red_pos | b.yellow_pos;
        ulong selected_col = (((ulong)0x3F << (6 * col)) & pieces) >> (6 * col);
        // Add piece via dilation
        ulong selected_piece = selected_col ^ (selected_col << 1);
        selected_piece ^= 1;
        // Update board with new piece
        selected_piece = selected_piece << (6 * col);

        if (turn)
        {
            b.red_pos |= selected_piece;
        }
        else
        {
            b.yellow_pos |= selected_piece;
        }
    }

    public int AIMove(Board b)
    {
        Minimax algo = new Minimax(b, this, 6);
        int move = Random.Range(0, 7);
        while(!ValidMove(b, move))
        {
            move = Random.Range(0, 7);
        }
        return move;
    }

}
