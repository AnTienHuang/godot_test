using Godot;
using System;

public partial class Game : Node
{
    [Export]
    public PackedScene EnemyScene { get; set; }

    private int _score;

    public void GameOver()
    {
        GetNode<AudioStreamPlayer>("Music").Stop();
        GetNode<AudioStreamPlayer>("DeathSound").Play();
        GetNode<Timer>("EnemyTimer").Stop();
        GetNode<Timer>("ScoreTimer").Stop();
        GetNode<HUD>("HUD").ShowGameOver();
    }

    public void NewGame()
    {
        _score = 0;
        GetTree().CallGroup("AllEnemy", Node.MethodName.QueueFree);
        GetNode<AudioStreamPlayer>("Music").Play();
        var player = GetNode<Player>("Player");
        var startPosition = GetNode<Marker2D>("StartPosition");
        
        player.Start(startPosition.Position);
        GetNode<Timer>("StartTimer").Start();

        var hud = GetNode<HUD>("HUD");
        hud.UpdateScore(_score);
        hud.ShowMessage("Get Ready!");
    }
    
    private void OnScoreTimerTimeout()
    {
        _score++;
        GetNode<HUD>("HUD").UpdateScore(_score);
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
        direction += (float)GD.RandRange(-Mathf.Pi / 6, Mathf.Pi / 6);
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
