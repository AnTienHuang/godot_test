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

        var player = GetNode<player>("Player");
        var startPosition = GetNode<Marker2D>("StartPosition");
        
        player.Start(startPosition.Position);
        GetNode<Timer>("StartTimer").Start();
    }
}
