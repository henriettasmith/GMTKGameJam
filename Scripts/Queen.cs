using Godot;
using System;


public class Queen: Piece
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
            position + new Vector2(3,0),

            position + new Vector2(-1,-1),
            position + new Vector2(-2,-2),
            position + new Vector2(-3,-3),

            position + new Vector2(-1,1),
            position + new Vector2(-2,2),
            position + new Vector2(-3,3),

            position + new Vector2(1,-1),
            position + new Vector2(2,-2),
            position + new Vector2(3,-3),

            position + new Vector2(1,1),
            position + new Vector2(2,2),
            position + new Vector2(3,3)
        };
    }

    public override PIECE id()
    {
        return PIECE.QUEEN;
    }

    public override int Score()
    {
        return 9;
    }
}

