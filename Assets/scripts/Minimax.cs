using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimax
{
    public int depth;
    public Board board;
    public Game game;

    public Minimax(Board b, Game g, int d)
    {
        depth = d;
        board = b;
        game = g;
    }

    public int run()
    {
        return 1;
    }
}
