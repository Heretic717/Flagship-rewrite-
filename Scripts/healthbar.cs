using Godot;
using System;

public partial class healthbar : TextureProgressBar
{
	player_char_script player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetParent().GetParent().GetParent().GetParent().GetChild<player_char_script>(1);

		MaxValue = player.MaxHealth;
		Value = player.Health;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Value != player.Health)
			TickingHealth((float)delta);
	}

	private void TickingHealth(float delta)
	{
		if (MaxValue != player.MaxHealth)
		{
			MaxValue = player.MaxHealth;
		}

		if (Value > player.Health)
		{
			Value -= 1f;
		} else if (Value < player.Health)
		{
			Value += 1f;
		}
	}
}
