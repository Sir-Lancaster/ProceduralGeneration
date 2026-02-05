using Godot;
using System;

public partial class CellularAutomataDemo : Node2D
{
	private WorldRenderer _worldRenderer;

	// Slider parameters.
	private int _width = 100;
	private int _height = 100;
	private float _fillProbability = 0.45f;
	private int _smoothingIterations = 5;
	private int _birthLimit = 4;
	private int _deathLimit = 3;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_worldRenderer = GetNode<WorldRenderer>("WorldRenderer");
		GenerateWorld();
	}

	private void GenerateWorld()
	{
		bool[,] grid = CellularAutomata.Generate(
			_width,
			_height,
			_fillProbability,
			_smoothingIterations,
			_birthLimit,
			_deathLimit
		);

		_worldRenderer.SetGrid(grid);
	}

	private void _on_generate_button_pressed()
	{
		GenerateWorld();
	}

	private void _on_back_to_menu_button_pressed()
	{
		GetNode<SceneManager>("/root/SceneManager").GoToMainMenu();
	}
}
