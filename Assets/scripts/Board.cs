using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
	public ulong yellow_pos, red_pos;
    public bool turn;

    public Board()
    {
        red_pos = 0;
	    yellow_pos = 0;
        turn = false;
    }

	public Board(ulong yp, ulong rp, bool t)
    {
        red_pos = rp;
        yellow_pos = yp;
        turn = t;
    }
}

