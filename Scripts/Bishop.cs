using Godot;
using System;

public class Bishop: Piece
{
    public override Vector2[] targets()
    {
        return new Vector2[]
        {
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
        return PIECE.BISHOP;
    }

    public override int Score()
    {
        return 3;
    }
}
