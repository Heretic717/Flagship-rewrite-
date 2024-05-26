using Godot;
using System;

public partial class HudScoreLabel : Label
{
	public override void _Process(double delta)
	{
		Text = UserVariables.Score.ToString();
	}
}
