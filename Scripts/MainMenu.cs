using Godot;
using System;

public partial class MainMenu : Node2D
{
	void _on_texture_button_pressed()
	{
		GetChild<Control>(1).GetChild<VBoxContainer>(0).GetChild<BoxContainer>(3).Visible = false;
		GetChild<Control>(1).GetChild<VBoxContainer>(0).GetChild<TextureButton>(4).Visible = false;
		GetChild<Control>(1).GetChild<VBoxContainer>(0).GetChild<Container>(2).Visible = false;
		GetChild<Control>(1).GetChild<VBoxContainer>(0).GetChild<BoxContainer>(5).Visible = true;
	}

	void LoadGame()
	{
		UserVariables.Score = 0;

		GetTree().ChangeSceneToFile("res://world.tscn");
	}

	void _on_cruiser_button_pressed()
	{
		UserVariables.LoadedShip = 0;

		LoadGame();
	}
	void _on_star_ship_button_pressed()
	{
		UserVariables.LoadedShip = 1;
		LoadGame();
	}

	void _on_quit_pressed()
	{
		GetTree().Quit();
	}

	void _on_audio_stream_player_finished()
	{
		GetChild<AudioStreamPlayer>(6).Play();
	}
}
