using Godot;
using System;

public partial class UserVariables : Node2D
{
	private static int loadedShip;
	private static int score;

	public static int LoadedShip
	{
		get { return loadedShip; }
		set { loadedShip = value; }
	}

	public static int Score
	{
		get { return score; }
		set { score = value; }
	}
}
