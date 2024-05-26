using Godot;
using System;

public partial class worldStuff : Node2D
{
	PackedScene enemyFighter = (PackedScene)GD.Load("res://Scenes/enemy_fighter.tscn");
	Area2D player;
	PathFollow2D spawnPath;

	float spawnRate = 8f;
	Timer enemyTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetChild<Area2D>(1);
		
		spawnPath = GetChild<Area2D>(1).GetChild<Camera2D>(1).GetChild<Path2D>(0).GetChild<PathFollow2D>(0);

		enemyTimer = GetChild<Timer>(3);
		enemyTimer.WaitTime = spawnRate;
		enemyTimer.Start();
		enemyTimer.Timeout += () => spawnEnemy();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void spawnEnemy() 
	{
		
		enemyTimer.Stop();
		enemyTimer.WaitTime -= 0.001f;
		enemyTimer.Start();
		GD.Randomize();
		int enemyNum = GD.RandRange(1, 4);

		for (int i = 0; i < enemyNum; i++)
		{
			GD.Print("spawned fighters");
			spawnPath.ProgressRatio = GD.Randf();
			enemy_fighter_AI enemyFighterInstance = enemyFighter.Instantiate<enemy_fighter_AI>();
			AddChild(enemyFighterInstance);
			enemyFighterInstance.GlobalPosition = spawnPath.GetChild<Node2D>(0).GlobalPosition;
			enemyFighterInstance.player = player;
		}
	}
}
