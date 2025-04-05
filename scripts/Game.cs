using Godot;
using System;

public partial class Game : Node
{
    [Export]
    public PackedScene EnemyScene { get; set; }

    private int _score;

    public void GameOver()
    {
        GetNode<Timer>("EnemyTimer").Stop();
        GetNode<Timer>("ScoreTimer").Stop();
    }

    public void NewGame()
    {
        _score = 0;

        var player = GetNode<Player>("Player");
        var startPosition = GetNode<Marker2D>("StartPosition");
        
        player.Start(startPosition.Position);
        GetNode<Timer>("StartTimer").Start();
    }

    private void OnScoreTimerTimeout()
    {
        _score++;
    }

    private void OnStartTimerTimeout()
    {
        GetNode<Timer>("EnemyTimer").Start();
        GetNode<Timer>("ScoreTimer").Start();
    }

    private void OnEnemyTimerTimeout()
    {
        // Create a new instance of the enemy scene 
        Enemy enemy = EnemyScene.Instantiate<Enemy>();

        // Set enemy to a random location on Path2D
        var enemySpawnLocation = GetNode<PathFollow2D>("EnemyPath/EnemySpawnLocation");
        enemy.Position = enemySpawnLocation.Position;
        enemySpawnLocation.ProgressRatio = GD.Randf();
        
        // Set enemy's direction (Perpendicular to the path direction)
        float direction = enemySpawnLocation.Rotation + Mathf.Pi / 2;
        // Add randomness to the direction
        direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        enemy.Rotation = direction;

        // Set velocity
        var velocity = new Vector2((float)GD.RandRange(250.0, 500.0), 0);
        enemy.LinearVelocity = velocity.Rotated(direction);

        // Spawn the enemy
        AddChild(enemy);
    }

    public override void _Ready()
    {
        // NewGame();
        Console.Write("game ready");
    }
}
