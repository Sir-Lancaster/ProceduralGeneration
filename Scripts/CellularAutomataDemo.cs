using Godot;
using System;

public partial class CellularAutomataDemo : Node2D
{
	// Nodes.
	private WorldRenderer _worldRenderer;
	private Label _fillProbLabel;
	private Label _smoothIterLabel;
	private Label _birthLimLabel;
	private Label _deathLimLabel;

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
		// Get Nodes
		_worldRenderer = GetNode<WorldRenderer>("WorldRenderer");
		_fillProbLabel = GetNode<Label>("CanvasLayer/UIPanel/FillProbLabel");
		_smoothIterLabel = GetNode<Label>("CanvasLayer/UIPanel/SmoothIterLabel");
		_birthLimLabel = GetNode<Label>("CanvasLayer/UIPanel/BirthLimLabel");
		_deathLimLabel = GetNode<Label>("CanvasLayer/UIPanel/DeathLimLabel");
		
		// Set the label text to display the current slider value.
		_fillProbLabel.Text = $"Fill Probability: {_fillProbability:F2}";
		_smoothIterLabel.Text = $"Smoothing Iterations: {_smoothingIterations}";
		_birthLimLabel.Text = $"Birth Limit: {_birthLimit}";
		_deathLimLabel.Text = $"Death Limit: {_deathLimit}";

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

	// Slider Functions.
	private void _on_fill_prob_slider_value_changed(float value)
	{
		_fillProbability = value;
		_fillProbLabel.Text = $"Fill Probability: {value:F2}";
	}
	
	private void _on_smooth_iter_slider_value_changed(int value)
	{
		_smoothingIterations = value;
		_smoothIterLabel.Text = $"Smoothing Iterations: {value}";
	}

	private void _on_birth_lim_slider_value_changed(int value)
	{
		_birthLimit = value;
		_birthLimLabel.Text = $"Birth Limit: {value}";
	}

	private void _on_death_lim_slider_value_changed(int value)
	{
		_deathLimit = value;
		_deathLimLabel.Text = $"Death Limit: {value}";
	}

	// Button Functions.
	private void _on_generate_button_pressed()
	{
		GenerateWorld();
	}

	private void _on_back_to_menu_button_pressed()
	{
		GetNode<SceneManager>("/root/SceneManager").GoToMainMenu();
	}
}
