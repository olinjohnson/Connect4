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


        int[] scores = new int[7];
        int least = 1000;
        for (int i = 0; i < 7; i++)
        {
            if (game.ValidMove(b, i))
            {
                int node = Run(game.MakeMove(b, i), depth, 1, -1000, 1000) + position_offsets[i];
                least = Math.Min(least, node);
                scores[i] = node;
                Debug.Log(node);
            }
        }


        watch.Stop();
        var elapsed = watch.ElapsedMilliseconds;
        Debug.Log("Nodes searched: " + nodes_searched + " Time(ms): " + elapsed);

        return Array.IndexOf(scores, least);
    }

    public int Run(Board ghostMove, int depth, int currentDepth, int alpha, int beta)
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
                int best = -1000;
                for (int i = 0; i < 7; i++)
                {
                    if (game.ValidMove(ghostMove, i))
                    {
                        int childVal = Run(game.MakeMove(ghostMove, i), depth, currentDepth + 1, alpha, beta);
                        best = Math.Max(best, childVal);
                        alpha = Math.Max(alpha, childVal);
                        if (beta <= alpha) { break; }
                    }
                }
                return best;
            }
            else
            {
                int least = 1000;
                for (int i = 0; i < 7; i++)
                {
                    if (game.ValidMove(ghostMove, i))
                    {
                        int childVal = Run(game.MakeMove(ghostMove, i), depth, currentDepth + 1, alpha, beta);
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