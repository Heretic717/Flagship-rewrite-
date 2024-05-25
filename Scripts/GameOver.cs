using Godot;
using System;

public partial class GameOver : Control
{
	public override void _Ready()
	{
	  GetChild<VBoxContainer>(1).GetChild<HBoxContainer>(1).GetChild<Label>(1).Text = UserVariables.Score.ToString();
	}

	public void LoadGame()
	{
		UserVariables.Score = 0;
		GetTree().ChangeSceneToFile("res://Scenes/world.tscn");
	}

	public void _on_restart_pressed()
	{
		LoadGame();
	}

	public void _on_quit_pressed()
	{
		GetTree().Quit();
	}
}
