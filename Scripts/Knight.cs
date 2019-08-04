using Godot;
using System;

public class Knight: Piece
{
    public override Vector2[] targets()
    {
        return new Vector2[]
        {
            position + new Vector2(2,-1),
            position + new Vector2(2,1),
            position + new Vector2(-2,-1),
            position + new Vector2(-2,1),
            position + new Vector2(-1,-2),
            position + new Vector2(1,-2),
            position + new Vector2(-1,2),
            position + new Vector2(1,2)
        };
    }

    public override PIECE id()
    {
        return PIECE.KNIGHT;
    }

    public override int Score()
    {
        return 3;
    }
}
