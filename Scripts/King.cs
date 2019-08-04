using Godot;
using System;

public class King: Piece
{
    public override Vector2[] targets()
    {
        return new Vector2[]
        {
            position + new Vector2(0,-1),
            position + new Vector2(0,1),
            position + new Vector2(-1,-1),
            position + new Vector2(-1,1),
            position + new Vector2(-1,0),
            position + new Vector2(1,0),
            position + new Vector2(1,-1),
            position + new Vector2(1,1)
        };
    }

    public override PIECE id()
    {
        return PIECE.KING;
    }

    public override int Score()
    {
        return 4;
    }
}
