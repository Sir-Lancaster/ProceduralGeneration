using Godot;
using System;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Callback method for cellular automata button
	private void _on_cellular_automata_button_pressed()
	{
		GetTree().Root.GetNode<SceneManager>("SceneManager").GoToCellularAutomataDemo();
	}
}
