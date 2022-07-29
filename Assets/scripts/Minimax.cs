using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimax
{
    public Board board;
    public Game game;

    public uint nodes_searched;
    public float[] position_offsets = { 0.3f, 0.2f, 0.1f, 0f, 0.1f, 0.2f, 0.3f };

    public Minimax(Board b, Game g)
    {
        board = b;
        game = g;
    }

    public int FindIdealMove(Board b, int depth)
    {
        nodes_searched = 0;
        var watch = System.Diagnostics.Stopwatch.StartNew();

        float[] scores = new float[7];
        float least = 1000;
        for (int i = 0; i < 7; i++)
        {
            if (game.ValidMove(b, i))
            {
                float node = Run(game.MakeMove(b, i), depth, 1, -1000, 1000) + position_offsets[i];
                least = Math.Min(least, node);
                scores[i] = node;
            }
        }


        watch.Stop();
        var elapsed = watch.ElapsedMilliseconds;
        Debug.Log("Nodes searched: " + nodes_searched + " Time(ms): " + elapsed);

        return Array.IndexOf(scores, least);
    }


    public float Run(Board ghostMove, int depth, int currentDepth, float alpha, float beta)
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
            if(!ghostMove.turn)
            {
                float best = -1000;
                for (int i = 0; i < 7; i++)
                {
                    if (game.ValidMove(ghostMove, i))
                    {
                        float childVal = Run(game.MakeMove(ghostMove, i), depth, currentDepth + 1, alpha, beta) + position_offsets[i];
                        best = Math.Max(best, childVal);
                        alpha = Math.Max(alpha, best);
                        if (beta <= alpha) { break; }
                    }
                }
                return best;
            }
            else
            {
                float least = 1000;
                for (int i = 0; i < 7; i++)
                {
                    if (game.ValidMove(ghostMove, i))
                    {
                        float childVal = Run(game.MakeMove(ghostMove, i), depth, currentDepth + 1, alpha, beta) + position_offsets[i];
                        least = Math.Min(least, childVal);
                        beta = Math.Min(beta, least);
                        if (beta <= alpha) { break; }
                    }
                }
                return least;
            }
        }
    }

    public int Evaluate(Board b, int currentDepth)
    {
        int[] p_directions = { 7, 6, 1, 8 };
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
