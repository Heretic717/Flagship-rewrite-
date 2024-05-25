using Godot;
using System;

public partial class muzzle_flashes : Sprite2D
{
	int lifetime = 15;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		lifetime -= 1;
		if (lifetime >= 0)
			QueueFree();
	}
}
