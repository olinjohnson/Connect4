using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimax
{
    public Board board;
    public Game game;

    public Minimax(Board b, Game g)
    {
        board = b;
        game = g;
    }

    public int FindIdealMove(Board b, int depth)
    {
        int?[] scores = new int?[7];
        for (int i = 0; i < 7; i++)
        {
            if (game.ValidMove(b, i))
            {
                scores[i] = Run(game.MakeMove(b, i), depth, 1);
            }
        }

        return Array.IndexOf(scores, game.MinElem(scores));
    }

    public int? Run(Board ghostMove, int depth, int currentDepth)
    {
        int potential_win = Evaluate(ghostMove);
        if (potential_win != 0 || depth == currentDepth)
        {
            return potential_win;
        }
        else
        {
            int?[] scores = new int?[7];
            for (int i = 0; i < 7; i++)
            {
                if (game.ValidMove(ghostMove, i))
                {
                    scores[i] = Run(game.MakeMove(ghostMove, i), depth, currentDepth + 1);
                }
            }
            if(!(scores.Length == 0) && !(scores == null))
            {
                if (ghostMove.turn)
                {
                    return game.MinElem(scores);
                }
                else
                {
                    return game.MaxElem(scores);
                }
            }
            else
            {
                return 0;
            }

        }
    }

    public int Evaluate(Board b)
    {
        int[] p_directions = { 6, 5, 1, 7 };
        foreach (int d in p_directions)
        {
            // Could be vectorized
            ulong y_pair = b.yellow_pos & (b.yellow_pos >> d);
            ulong r_pair = b.red_pos & (b.red_pos >> d);
            if ((y_pair & (y_pair >> (2 * d))) != 0)
            {
                return 1;
            }
            if ((r_pair & (r_pair >> (2 * d))) != 0)
            {
                return -1;
            }
        }
        return 0;
    }
}
