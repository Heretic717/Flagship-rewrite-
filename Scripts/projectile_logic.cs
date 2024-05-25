using Godot;
using System;

public partial class projectile_logic : Area2D
{

	public Vector2 velocity = new(0, 0);
	float speed = 500f;
	Vector2 startingPos;
	public float range = .35f;
	float totalMoved;
	Timer timer;

	PackedScene fizzleParticles = (PackedScene)GD.Load("res://Effects/Explosion_miss.tscn");
	PackedScene hitParticles = (PackedScene)GD.Load("res://Effects/Explosion_hit.tscn");

	AudioStreamPlayer2D hitSound;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		startingPos = Position;
		timer = GetChild<Timer>(2);
		timer.WaitTime = range + GD.RandRange(-.05, .05);
		timer.Start();
		timer.Timeout += () => _on_Fizzle();

		AreaEntered += (Area2D body) => _on_Hit();
		hitSound = GetNode("/root/Sfx").GetChild<AudioStreamPlayer2D>(2);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);;
		GlobalPosition += velocity * new Vector2((float)delta * speed, (float)delta * speed);
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public override void _Process(double delta)
	{

	}

	private void _on_Fizzle()
	{
		//spawn fizzle particles here
		GpuParticles2D fizzle = fizzleParticles.Instantiate<GpuParticles2D>();
		GetTree().Root.AddChild(fizzle);
		fizzle.GlobalPosition = GlobalPosition;
		QueueFree();
	}
	private void _on_Hit()
	{
		hitSound.Play();
		hitSound.GlobalPosition = GlobalPosition;
		//spawn projectile hit particles here
		GpuParticles2D hit = hitParticles.Instantiate<GpuParticles2D>();
		GetTree().Root.AddChild(hit);
		hit.GlobalPosition = GlobalPosition;
		GetNode<Node>("/root/UserVariables").Set("Score", GetNode<Node>("/root/UserVariables").Get("Score").As<int>() + 1);
		QueueFree();
	}

}
