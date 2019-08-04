using Godot;
using System;

public class Rook: Piece
{
    public override Vector2[] targets()
    {
        return new Vector2[]
        {
            position + new Vector2(-1,0),
            position + new Vector2(-2,0),
            position + new Vector2(-3,0),

            position + new Vector2(0,1),
            position + new Vector2(0,2),
            position + new Vector2(0,3),

            position + new Vector2(0,-1),
            position + new Vector2(0,-2),
            position + new Vector2(0,-3),

            position + new Vector2(1,0),
            position + new Vector2(2,0),
            position + new Vector2(3,0)
        };
    }

    public override PIECE id()
    {
        return PIECE.ROOK;
    }

    public override int Score()
    {
        return 5;
    }
}

