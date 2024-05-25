using Godot;
using System;
using System.Collections;
using System.Diagnostics;

public partial class player_char_script : Area2D
{
	float velocity = 0f; //velocity
	float acceleration = 0f; //real acceleration
	float radialAcceleration = 0f;
	float radialVelocity = 0f;
	float rotationSpeed;
	float accel = 0f; // base value for acceleration
	public static Vector2 speed = new(0, 0); // speed on x and y axis
	GpuParticles2D thrusterMain;
	GpuParticles2D thruster1;
	GpuParticles2D thruster2;
	GpuParticles2D thruster3;
	GpuParticles2D thruster4;
	GpuParticles2D thruster5;
	GpuParticles2D thruster6;
	GpuParticles2D thruster7;
	GpuParticles2D thruster8;
	Area2D Attack_Orbit;

	PackedScene death = GD.Load<PackedScene>("res://Effects/Explosion_dead.tscn");
	string[] shipPaths = new string[] { "res://Scenes/base_ship_assembly.tscn", "res://Scenes/star_ship_assembly.tscn" };
	string[] shapePaths = new string[] { "res://Scenes/char_base_collider.tscn", "res://Scenes/char_star_collider.tscn" };
	PackedScene ship;
	PackedScene collider;
	Node2D playerShip;
	CollisionPolygon2D playerCollider;
	
	AudioStreamPlayer2D explode;
	AudioStreamPlayer2D thrust1;
	AudioStreamPlayer2D thrust2;
	AudioStreamPlayer2D thrust3;

	public float health = 100;
	public float maxHealth = 100;

	public override void _Ready()
	{
		ship = GD.Load<PackedScene>(shipPaths[UserVariables.LoadedShip]);
		collider = GD.Load<PackedScene>(shapePaths[UserVariables.LoadedShip]);
		playerShip = ship.Instantiate<Node2D>();
		playerCollider = collider.Instantiate<CollisionPolygon2D>();
		AddChild(playerShip);
		AddChild(playerCollider);

		Attack_Orbit = GetChild<Area2D>(0);

		AreaEntered += (Area2D body) => _on_Hit(body);

		explode = GetNode("/root/Audio/Sfx").GetChild<AudioStreamPlayer2D>(0);
		thrust1 = GetNode("/root/Audio/Sfx").GetChild<AudioStreamPlayer2D>(4);
		thrust2 = GetNode("/root/Audio/Sfx").GetChild<AudioStreamPlayer2D>(4);
		thrust3 = GetNode("/root/Audio/Sfx").GetChild<AudioStreamPlayer2D>(4);

		switch (UserVariables.LoadedShip)
		{ 
			case 0:
				{
					thrusterMain = GetChild<Node2D>(1).GetChild<Sprite2D>(0).GetChild<Node2D>(0).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
					thruster1 = GetChild<Node2D>(1).GetChild<Sprite2D>(0).GetChild<Node2D>(1).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
					thruster2 = GetChild<Node2D>(1).GetChild<Sprite2D>(0).GetChild<Node2D>(2).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
					thruster3 = GetChild<Node2D>(1).GetChild<Sprite2D>(0).GetChild<Node2D>(3).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
					thruster4 = GetChild<Node2D>(1).GetChild<Sprite2D>(0).GetChild<Node2D>(4).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);

					accel = 20f;

					maxHealth = 200;
					health = maxHealth;
				}
				break;
			case 1:
				{
					thruster1 = GetChild<Node2D>(1).GetChild<Sprite2D>(0).GetChild<Node2D>(0).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
					thruster2 = GetChild<Node2D>(1).GetChild<Sprite2D>(0).GetChild<Node2D>(1).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
					thruster3 = GetChild<Node2D>(1).GetChild<Sprite2D>(0).GetChild<Node2D>(2).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
					thruster4 = GetChild<Node2D>(1).GetChild<Sprite2D>(0).GetChild<Node2D>(3).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
					thruster5 = GetChild<Node2D>(1).GetChild<Sprite2D>(0).GetChild<Node2D>(4).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
					thruster6 = GetChild<Node2D>(1).GetChild<Sprite2D>(0).GetChild<Node2D>(5).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
					thruster7 = GetChild<Node2D>(1).GetChild<Sprite2D>(0).GetChild<Node2D>(6).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);
					thruster8 = GetChild<Node2D>(1).GetChild<Sprite2D>(0).GetChild<Node2D>(7).GetChild<Node2D>(0).GetChild<GpuParticles2D>(0);

					AreaEntered += (Area2D body) => _on_Hit(body);

					accel = 10f;

					maxHealth = 300;
					health = maxHealth;
				}
				break;

				default:
				{

				} 
				break;
		}
		

	}

	public override void _PhysicsProcess(double delta)
	{

		if (health <= 0)
		{
			On_Death();
		}

		switch (UserVariables.LoadedShip)
		{
			case 0:
			{
					BaseShipMove((float) delta);
			} break;
			case 1:
			{
					StarShipMove((float)delta);
			}break;
			default:
			{

			}break;
		}
	}

	private void _on_Hit(Area2D body)
	{
		String bodyGroup = body.GetGroups()[0];
		switch (bodyGroup)
		{
			case "enemyprojectilesmall":
				{
					health -= 10;
				} break;
				default:
				{

				}break;
		}
		body.QueueFree();
	}

	private async void On_Death()
	{
		explode.Play();
		explode.GlobalPosition = GlobalPosition;
		GpuParticles2D deathExplosion = death.Instantiate<GpuParticles2D>();
		GetTree().Root.AddChild(deathExplosion);
		deathExplosion.GlobalPosition = GlobalPosition;

		// stop the game loop and display the game over screen
		await ToSignal(GetTree().CreateTimer(2.0), SceneTreeTimer.SignalName.Timeout);

		GetTree().ChangeSceneToFile("res://Scenes/GameOver.tscn");
	}

	private void BaseShipMove(float delta)
	{
		//put a max and min on acceleration to prevent extreme speed or rubberbanding on deceleration
		if (acceleration > accel * 10)
			acceleration = accel * 10;
		else if (acceleration < 0)
			acceleration = 0;
		if (radialAcceleration > accel * 5)
			radialAcceleration = accel * 5;
		else if (radialAcceleration < 0)
			radialAcceleration = 0;

		//set velocity	
		velocity += acceleration * (float)delta;
		radialVelocity += radialAcceleration * (float)delta;


		// calculate directional speed based on which key was pressed
		if (Input.IsActionPressed("MovementUp"))
		{
			acceleration += accel;
			speed.X += velocity * (float)delta + acceleration * (float)delta * (float)delta * .5f;
			thrusterMain.Emitting = true;
			thruster1.Emitting = true;
			thruster2.Emitting = true;
			thrust1.GlobalPosition = thrusterMain.GlobalPosition;
			if (!thrust1.Playing)
			{
				thrust1.Play();
			}
		}
		if (Input.IsActionPressed("MovementDown"))
		{
			speed.X *= .8f;
			thrusterMain.Emitting = false;
			thruster1.Emitting = false;
			thruster2.Emitting = false;
			if (thrust1.Playing)
			{
				thrust1.Stop();
			}
		}
		if (Input.IsActionPressed("MovementLeft"))
		{
			radialAcceleration += accel;
			rotationSpeed += (-radialVelocity * (float)delta + radialAcceleration * (float)delta * (float)delta * .5f) / 180 * 3.14f;
			rotationSpeed *= .8f;
			thruster4.Emitting = true;
			thrust2.GlobalPosition = thruster4.GlobalPosition;
			if (!thrust3.Playing)
			{
				thrust2.Play();
			}

		}
		if (Input.IsActionPressed("MovementRight"))
		{
			radialAcceleration += accel;
			rotationSpeed += (radialVelocity * (float)delta + radialAcceleration * (float)delta * (float)delta * .5f) / 180 * 3.14f;
			rotationSpeed *= .8f;
			thruster3.Emitting = true;
			thrust3.GlobalPosition = thruster3.GlobalPosition;
			if (!thrust3.Playing)
			{
				thrust3.Play();
			}
		}

		if (Input.IsActionJustReleased("MovementUp"))
		{
			thrusterMain.Emitting = false;
			thruster1.Emitting = false;
			thruster2.Emitting = false;
			thrust1.Stop();
		}
		if (Input.IsActionJustReleased("MovementLeft"))
		{
			thruster4.Emitting = false;
			thrust3.Stop();
		}
		if (Input.IsActionJustReleased("MovementRight"))
		{
			thruster3.Emitting = false;
			thrust2.Stop();
		}


		// set new position and rotationSpeed based on current speed
		Rotate(rotationSpeed);
		Position += new Vector2(speed.X, speed.Y).Rotated(Rotation);


		// friction for smooth accel/decel
		if (speed.Y != 0)
		{
			speed.Y *= .95f;
		}
		if (speed.X != 0)
		{
			speed.X *= .95f;
		}
		if (rotationSpeed != 0)
		{
			rotationSpeed *= .95f;
		}

		acceleration -= velocity * 5f;
		radialAcceleration -= radialVelocity * 5f;
	}

	private void StarShipMove(float delta)
	{
		//put a max and min on acceleration to prevent extreme speed or rubberbanding on deceleration
		if (acceleration > accel * 10)
			acceleration = accel * 10;
		else if (acceleration < 0)
			acceleration = 0;

		//set velocity	
		velocity += acceleration * (float)delta;

		// calculate directional speed based on which key was pressed
		if (Input.IsActionPressed("MovementUp"))
		{
			if (!thrust1.Playing)
				thrust1.Play();
			thrust1.GlobalPosition = thruster7.GlobalPosition;
			thruster7.Emitting = true;
			thruster6.Emitting = true;
			thruster8.Emitting = true;
			acceleration += accel;
			speed.Y -= velocity * (float)delta + acceleration * (float)delta * (float)delta * .5f;
			if (Input.IsActionPressed("MovementLeft"))
			{
				speed.Y *= .975f;
				acceleration -= accel;
				thruster8.Emitting = false;

			}
			if (Input.IsActionPressed("MovementRight"))
			{
				speed.Y *= .975f;
				acceleration -= accel;
				thruster6.Emitting = false;
			}
		}
		if (Input.IsActionPressed("MovementDown"))
		{
			if (!thrust1.Playing)
				thrust1.Play();
			thrust1.GlobalPosition = thruster3.GlobalPosition;
			thruster2.Emitting = true;
			thruster3.Emitting = true;
			thruster4.Emitting = true;
			acceleration += accel;
			speed.Y += velocity * (float)delta + acceleration * (float)delta * (float)delta * .5f;
			if (Input.IsActionPressed("MovementLeft"))
			{
				speed.Y *= .975f;
				acceleration -= accel;
				thruster2.Emitting = false;

			}
			if (Input.IsActionPressed("MovementRight"))
			{
				speed.Y *= .975f;
				acceleration -= accel;
				thruster4.Emitting = false;
			}
		}
		if (Input.IsActionPressed("MovementLeft"))
		{
			if (!thrust1.Playing)
				thrust1.Play();
			thrust1.GlobalPosition = thruster5.GlobalPosition;
			thruster5.Emitting = true;
			thruster6.Emitting = true;
			thruster4.Emitting = true;
			acceleration += accel;
			speed.X -= velocity * (float)delta + acceleration * (float)delta * (float)delta * .5f;
			if (Input.IsActionPressed("MovementUp"))
			{
				speed.X *= .975f;
				acceleration -= accel;
				thruster4.Emitting = false;

			}
			if (Input.IsActionPressed("MovementDown"))
			{
				speed.X *= .975f;
				acceleration -= accel;
				thruster6.Emitting = false;

			}
		}
		if (Input.IsActionPressed("MovementRight"))
		{
			if (!thrust1.Playing)
				thrust1.Play();
			thrust1.GlobalPosition = thruster1.GlobalPosition;
			thruster1.Emitting = true;
			thruster8.Emitting = true;
			thruster2.Emitting = true;
			acceleration += accel;
			speed.X += velocity * (float)delta + acceleration * (float)delta * (float)delta * .5f;
			if (Input.IsActionPressed("MovementUp"))
			{
				speed.X *= .975f;
				acceleration -= accel;
				thruster2.Emitting = false;

			}
			if (Input.IsActionPressed("MovementDown"))
			{
				speed.X *= .975f;
				acceleration -= accel;
				thruster8.Emitting = false;

			}
		}

		if (Input.IsActionJustReleased("MovementUp"))
		{
			thrust1.Stop();
			thruster6.Emitting = false;
			thruster7.Emitting = false;
			thruster8.Emitting = false;
		}
		if (Input.IsActionJustReleased("MovementDown"))
		{
			thrust1.Stop();
			thruster2.Emitting = false;
			thruster3.Emitting = false;
			thruster4.Emitting = false;
		}
		if (Input.IsActionJustReleased("MovementLeft"))
		{
			thrust1.Stop();
			thruster4.Emitting = false;
			thruster5.Emitting = false;
			thruster6.Emitting = false;
		}
		if (Input.IsActionJustReleased("MovementRight"))
		{
			thrust1.Stop();
			thruster8.Emitting = false;
			thruster1.Emitting = false;
			thruster2.Emitting = false;
		}

		// set new position based on current directional speed
		Position += new Vector2(speed.X, speed.Y);


		// friction for smooth accel/decel
		if (speed.Y != 0)
		{
			speed.Y *= .95f;
		}
		if (speed.X != 0)
		{
			speed.X *= .95f;
		}
		acceleration -= velocity * 5f;
	}


	}
