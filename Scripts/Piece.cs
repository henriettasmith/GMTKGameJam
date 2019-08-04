using Godot;
using System;

public enum PIECE {KNIGHT, PAWN, BISHOP, ROOK, KING, QUEEN}

public abstract class Piece : Node2D
{
    public Vector2 position;
    [Export]
    public bool turnLocked;
    public float EnemyTimer = 0f;
    public float EnemyTimerMax = 1f;
    public bool awake = false;
    public TextureProgress textureProgress;
    public Vector2 targetMove;
    public abstract Vector2[] targets();
    public abstract PIECE id();
    public abstract int Score();
}
