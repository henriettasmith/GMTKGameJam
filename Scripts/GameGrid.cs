using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameGrid : TileMap
{
    [Export]
    public int Width;
    [Export]
    public int Height;
    [Export]
    public Curve progressCurve;

    public Node2D player;
    public TileMap EnemyHighlights;
    public TileMap PlayerHighlights;
    public Camera2D camera;
    public Control ClickPanel;
    public Label ScoreText;
    public Label InstructionText;

    public int score = 0;
    public bool paused = true;
    public Knight knight;
    public Vector2[] validPositions;
    public List<Piece> Enemies = new List<Piece>();
    
    public float EnemyTimer = 0f;
    public PackedScene PiecesFactoryScene = (PackedScene)ResourceLoader.Load("res://Pieces.tscn");
    public Node PiecesFactory;
    public Node parent;
    public int enemyCount = 4;
    public bool dead = false;
    public float DeathTimer = 2f;

    public override void _Ready()
    {
        GD.Randomize();

        player = (Node2D)GetNode("../BlackKnight");
        EnemyHighlights = (TileMap)GetNode("EnemyHighlights");
        PlayerHighlights = (TileMap)GetNode("PlayerHighlights");
        camera = (Camera2D)GetNode("../Camera2D");
        parent = GetNode("..");
        ClickPanel = (Control)GetNode("../ClickPanel");
        ScoreText = (Label)GetNode("../ScoreText");
        InstructionText = (Label)GetNode("../InstructionText");
        PiecesFactory = PiecesFactoryScene.Instance();

        knight = new Knight();
        knight.position = WorldToMap(player.Position);
        validPositions = knight.targets();

        while(Enemies.Count < enemyCount)
            SpawnEnemy();

        UpdateMap();
    }

    //y:0-9
    //x:12-23
    //12
    //Pawn -> Bishop -> Knight -> Pawn2 -> King -> Rook -> Bishop2 -> Knight2 -> Queen -> King2 -> Rook2 -> Queen2

    public void SpawnEnemy()
    {
        List<string> l = new List<string>() {"Pawn2", "Bishop", "Knight", "Pawn2", "King", "Pawn", "Rook", "Bishop2", "Knight2", "Queen", "Pawn2", "King2", "Rook2", "Queen2"};
        float maximum = progressCurve.Interpolate((float)score/100f);
        int id = (int)GD.RandRange(0, maximum);

        Vector2 pos;
        do
        {
            if(l[id] == "Pawn" || l[id] == "Pawn2")
            {
                pos = new Vector2(11, (int)GD.RandRange(0, 10));
            }
            else
            {
                pos = new Vector2((int)GD.RandRange(4, 11), (int)GD.RandRange(0, 10));
            }
        } while(isPieceAt(pos) != null);

        AddEnemy(l[id], pos, !l[id].EndsWith("2"));
    }

    public void AddEnemy(string type, Vector2 pos, bool turnLocked = false)
    {
        Piece q = (Piece)PiecesFactory.GetNode(type).Duplicate();
        if(turnLocked == false)
        {
            TextureProgress tp = (TextureProgress)q.GetNode("TextureProgress");
            q.textureProgress = tp;
        }
        AddChild(q);
        q.position = pos;
        q.turnLocked = turnLocked;
        q.targetMove = q.position;
        Enemies.Add(q);
    }
    
    public override void _Process(float delta)
    {
        if(paused)
        {
            if(dead)
            {
                DeathTimer -= delta;
                if(DeathTimer <= 0)
                {
                    DeathTimer = 0;
                    if(Input.IsActionPressed("LeftClick"))
                    {
                        GetTree().ReloadCurrentScene();
                    }
                }
            }
            else if(Input.IsActionPressed("LeftClick"))
            {
                paused = false;
                ClickPanel.Visible = false;
            }
            return;
        }

        if(Input.IsActionPressed("LeftClick"))
        {
            Vector2 mouseWorldPos = camera.ToGlobal(GetViewport().GetMousePosition());
            Vector2 tilePos = WorldToMap(mouseWorldPos);

            if(WithinMap(tilePos) && validPositions.Contains(tilePos))
            {
                knight.position = tilePos;
                validPositions = knight.targets();

                for(int i = 0; i < Enemies.Count; ++i)
                {
                    Piece p = Enemies[i];
                    p.awake = true;
                    if(p.position == knight.position)
                    {
                        score += p.Score();
                        Enemies.RemoveAt(i);
                        --i;
                        p.Free();
                        continue;
                    }
                    if(p.turnLocked)
                    {
                        p.position = p.targetMove;
                        p.targetMove = MovePiece(p);
                    }
                }
                while(Enemies.Count < enemyCount)
                    SpawnEnemy();
            }
        }
        UpdateEnemies(delta);
        UpdateMap();
    }

    public bool WithinMap(Vector2 check)
    {
        return (check.x < 0 || check.x > Width || check.y < 0 || check.y > Height) == false;
    }

    public void UpdateMap()
    {
        PlayerHighlights.Clear();
        EnemyHighlights.Clear();

        player.Position = MapToWorld(knight.position) + CellSize/2;

        ScoreText.Text = score.ToString("D6");

        for(int i = 0; i < validPositions.Length; ++i)
        {
            if(WithinMap(validPositions[i]))
                PlayerHighlights.SetCellv(validPositions[i], 2);
        }

        foreach(Piece p in Enemies)
        {
            p.Position = MapToWorld(p.position) + CellSize/2;
            if(p.awake)
            {
                Color c = p.SelfModulate;
                c.a = 1;
                p.SelfModulate = c;
            }
            if(p.targetMove == p.position)
            {
                continue;
            }
            if(p.turnLocked == false)
            {
                p.textureProgress.Value = p.EnemyTimer / p.EnemyTimerMax * 100f;
            }
            EnemyHighlights.SetCellv(p.targetMove, 3);
        }
    }

    public Piece isPieceAt(Vector2 pos)
    {
        if(knight.position == pos)
            return knight;
        foreach(Piece p in Enemies)
        {
            if(p.position == pos)
                return p;
        }
        return null;
    }

    public void UpdateEnemies(float delta)
    {
        foreach(Piece p in Enemies)
        {
            if(p.awake && p.turnLocked == false)
            {
                p.EnemyTimer += delta;
                if(p.EnemyTimer >= p.EnemyTimerMax)
                {
                    p.EnemyTimer -= p.EnemyTimerMax;
                    p.position = p.targetMove;
                    p.targetMove = MovePiece(p);
                }
            }
        }
    }

    public void Die(string reason)
    {
        dead = true;
        paused = true;
        ClickPanel.Visible = true;
        ((Label)ClickPanel.GetNode("Label2")).Text = reason + "\n Click to restart";
    }

    public Vector2 MovePiece(Piece p)
    {

        if(p.id() == PIECE.PAWN && p.position.x == 0)
        {
            Die("A Pawn reached the end");
            return p.position;
        }

        List<Vector2> accessablePositions = AccessablePositions(p);

        if(accessablePositions.Count == 0)
            return p.position;

        Vector2 ret = accessablePositions[0];

        for(int i = 1; i < accessablePositions.Count; ++i)
        {
            if(ret.DistanceTo(knight.position) > accessablePositions[i].DistanceTo(knight.position))
                ret = accessablePositions[i];
        }

        if(p.id() == PIECE.PAWN)
        {
            Pawn pa = (Pawn)p;
            pa.moved = true;
        }
        else
        {
            if(p.position == knight.position)
            {
                Die("You were captured");
            }
        }

        return ret;
    }

    public List<Vector2> AccessablePositions(Piece p)
    {
        Vector2[] validPositions = p.targets();
        List<Vector2> accessablePositions = new List<Vector2>();

        for(int i = 0; i < validPositions.Length; ++i)
        {
            if(WithinMap(validPositions[i]) == false)
                continue;

            Piece p2 = isPieceAt(validPositions[i]);
            
            if(p2 == knight && p.id() == PIECE.PAWN)
                continue;
                
            if(p2 == null || p2 == knight)
                accessablePositions.Add(validPositions[i]);
        }

        return accessablePositions;
    }
}
