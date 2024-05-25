using Godot;
using System;

public partial class turret : Node2D
{
	PackedScene projectile = (PackedScene)GD.Load("res://Scenes/projectile.tscn");
	PackedScene[] muzzelFlare = new PackedScene[3]{ (PackedScene)GD.Load("res://Scenes/muzzle_flashes_1.tscn"), (PackedScene)GD.Load("res://Scenes/muzzle_flashes_2.tscn"), (PackedScene)GD.Load("res://Scenes/muzzle_flashes_3.tscn") };
	private float startRot = 0f;
	Timer timer;
	private float rOF = .15f;
	private bool canFire = false;

	private bool barrel1 = true;
	private bool barrel2 = false;
	Vector2 CursorPos;
	float missRadius = 10f;

	AudioStreamPlayer2D shoot;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		startRot = this.Rotation;
		timer = this.GetChild<Timer>(0, false);
		timer.WaitTime = rOF;
		timer.Start();
		timer.Timeout += () => canFire = true;

		shoot = GetNode("/root/Sfx").GetChild<AudioStreamPlayer2D>(3);
		shoot.MaxPolyphony = 10;
	}

	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		CursorPos = GetGlobalMousePosition();
		GetChild<Sprite2D>(1).LookAt(CursorPos);
		GetChild<Sprite2D>(1).GetChild<Node2D>(0).LookAt(CursorPos);

		if (Input.IsMouseButtonPressed(MouseButton.Left) && canFire) {
			Fire(forcedMissRadius(GD.Randf()));
		}
	}
	private Vector2 forcedMissRadius(float random)
	{
		Vector2 kill_cirlce_center = CursorPos;
		float angle = random * (float)Math.PI * 2f;
		float x = kill_cirlce_center.X + (float)Math.Cos(angle) * missRadius;
		float y = kill_cirlce_center.Y + (float)Math.Sin(angle) * missRadius;

		return new Vector2(x, y);
	}

	private void Fire(Vector2 miss)
	{
		int flareIndex = GD.RandRange(0, 2);
		projectile_logic proj = projectile.Instantiate<projectile_logic>();
		Sprite2D flare = muzzelFlare[flareIndex].Instantiate<Sprite2D>();
		GetTree().Root.AddChild(proj);
		GetTree().Root.AddChild(flare);

		shoot.Play();
		shoot.GlobalPosition = GlobalPosition;

		flare.GlobalRotation = GetChild<Sprite2D>(1).GlobalRotation;
		flare.Scale = new(.35f, .35f);
		if (barrel1)
		{
			proj.GlobalPosition = GetChild<Sprite2D>(1).GetChild<Node2D>(0).GlobalPosition;
			flare.GlobalPosition = GetChild<Sprite2D>(1).GetChild<Node2D>(0).GlobalPosition + new Vector2(4, 0).Rotated(GetChild<Sprite2D>(1).GlobalRotation);
			timer.Stop();
			timer.WaitTime = rOF;
			timer.Start();
		}
		else if (barrel2)
		{
			proj.GlobalPosition = GetChild<Sprite2D>(1).GetChild<Node2D>(1).GlobalPosition;
			flare.GlobalPosition = GetChild<Sprite2D>(1).GetChild<Node2D>(1).GlobalPosition + new Vector2(4, 0).Rotated(GetChild<Sprite2D>(1).GlobalRotation);
			timer.Stop();
			timer.WaitTime = rOF;
			timer.Start();
		}
		barrel1 = !barrel1;
		barrel2 = !barrel2;
		canFire = false;

		flare.LookAt(miss);
		proj.LookAt(miss);
		proj.velocity = (miss - GlobalPosition).Normalized() + player_char_script.speed.Rotated(proj.GlobalRotation) * .1f;
	}
}


