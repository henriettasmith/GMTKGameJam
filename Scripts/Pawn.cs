using Godot;
using System;

public class Pawn: Piece
{
    public bool moved = false;
    public override Vector2[] targets()
    {
        if(moved)
        {
            return new Vector2[]
            {
                position + new Vector2(-1,0)
            };
        }
        else
        {
            return new Vector2[]
            {
                position + new Vector2(-1,0),
                position + new Vector2(-2,0)
            };
        }
    }

    public override PIECE id()
    {
        return PIECE.PAWN;
    }

    public override int Score()
    {
        return 1;
    }
}

