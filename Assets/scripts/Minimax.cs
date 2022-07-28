using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimax
{
    public Board board;
    public Game game;

    public uint nodes_searched;
    public int[] position_offsets = { 3, 2, 1, 0, 1, 2, 3 };

    public Minimax(Board b, Game g)
    {
        board = b;
        game = g;
    }

    public int FindIdealMove(Board b, int depth)
    {
        nodes_searched = 0;
        var watch = System.Diagnostics.Stopwatch.StartNew();

        int?[] scores = new int?[7];
        for (int i = 0; i < 7; i++)
        {
            if (game.ValidMove(b, i))
            {
                scores[i] = Run(game.MakeMove(b, i), depth, 1) + position_offsets[i];
            }
        }

        watch.Stop();
        var elapsed = watch.ElapsedMilliseconds;
        Debug.Log("Nodes searched: " + nodes_searched + " Time(ms): " + elapsed);

        return Array.IndexOf(scores, game.MinElem(scores));
    }

    public int? Run(Board ghostMove, int depth, int currentDepth)
    {
        nodes_searched++;
        // Evaluate position
        int potential_win = Evaluate(ghostMove, currentDepth);
        // Check terminal state
        if (potential_win != 0 || depth == currentDepth)
        {
            return potential_win;
        }
        // Evaluate child nodes
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
            // If it's a draw
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

    public int Evaluate(Board b, int currentDepth)
    {
        int[] p_directions = { 6, 5, 1, 7 };
        foreach (int d in p_directions)
        {
            // Could be vectorized
            ulong y_pair = b.yellow_pos & (b.yellow_pos >> d);
            ulong r_pair = b.red_pos & (b.red_pos >> d);
            if ((y_pair & (y_pair >> (2 * d))) != 0)
            {
                return 100 - currentDepth;
            }
            if ((r_pair & (r_pair >> (2 * d))) != 0)
            {
                return -100 + currentDepth;
            }
        }
        return 0;
    }
}
