using Godot;
using System;

public partial class CellularAutomataDemo : Node2D
{
    // Nodes.
    private AutomataWorldRenderer _automataWorldRenderer;
    private Label _widthLabel;
    private Label _heightLabel;
    private Label _fillProbLabel;
    private Label _smoothIterLabel;
    private Label _birthLimLabel;
    private Label _deathLimLabel;
    private Label _seedLabel;

    // Slider parameters.
    private int _width = 200;
    private int _height = 200;
    private float _fillProbability = 0.45f;
    private int _smoothingIterations = 5;
    private int _birthLimit = 4;
    private int _deathLimit = 3;
    private int _seed = 0;

    public override void _Ready()
    {
        _automataWorldRenderer = GetNode<AutomataWorldRenderer>("AutomataWorldRenderer");
        _widthLabel = GetNode<Label>("CanvasLayer/UIPanel/WidthLabel");
        _heightLabel = GetNode<Label>("CanvasLayer/UIPanel/HeightLabel");
        _fillProbLabel = GetNode<Label>("CanvasLayer/UIPanel/FillProbLabel");
        _smoothIterLabel = GetNode<Label>("CanvasLayer/UIPanel/SmoothIterLabel");
        _birthLimLabel = GetNode<Label>("CanvasLayer/UIPanel/BirthLimLabel");
        _deathLimLabel = GetNode<Label>("CanvasLayer/UIPanel/DeathLimLabel");
        _seedLabel = GetNode<Label>("CanvasLayer/UIPanel/SeedLabel");

        // Connect signals from UI scene
        GetNode<HSlider>("CanvasLayer/UIPanel/WidthSlider").ValueChanged += _on_width_slider_value_changed;
        GetNode<HSlider>("CanvasLayer/UIPanel/HeightSlider").ValueChanged += _on_height_slider_value_changed;
        GetNode<HSlider>("CanvasLayer/UIPanel/FillProbSlider").ValueChanged += _on_fill_prob_slider_value_changed;
        GetNode<HSlider>("CanvasLayer/UIPanel/SmoothIterSlider").ValueChanged += _on_smooth_iter_slider_value_changed;
        GetNode<HSlider>("CanvasLayer/UIPanel/BirthLimSlider").ValueChanged += _on_birth_lim_slider_value_changed;
        GetNode<HSlider>("CanvasLayer/UIPanel/DeathLimSlider").ValueChanged += _on_death_lim_slider_value_changed;
        GetNode<LineEdit>("CanvasLayer/UIPanel/SeedInput").TextChanged += _on_seed_input_text_changed;
        GetNode<Button>("CanvasLayer/UIPanel/GenerateButton").Pressed += _on_generate_button_pressed;
        GetNode<Button>("CanvasLayer/UIPanel/BackToMenuButton").Pressed += _on_back_to_menu_button_pressed;

        _widthLabel.Text = $"Chunk Width: {_width}";
        _heightLabel.Text = $"Chunk Height: {_height}";
        _fillProbLabel.Text = $"Initial Density: {_fillProbability:F2}";
        _smoothIterLabel.Text = $"Num Steps: {_smoothingIterations}";
        _birthLimLabel.Text = $"Birth Limit: {_birthLimit}";
        _deathLimLabel.Text = $"Death Limit: {_deathLimit}";
        _seedLabel.Text = "Seed (0 for random)";

        GenerateWorld();
    }

    private void GenerateWorld()
    {
        var seedInput = GetNode<LineEdit>("CanvasLayer/UIPanel/SeedInput");
        if (int.TryParse(seedInput.Text, out int parsedSeed))
            _seed = parsedSeed;

        int seed = _seed == 0 ? -1 : _seed;
        bool[,] grid = CellularAutomata.Generate(
            _width, _height, _fillProbability, _smoothingIterations, _birthLimit, _deathLimit, seed
        );
        _automataWorldRenderer.SetGrid(grid);
    }

    private void _on_width_slider_value_changed(double value)
    {
        _width = (int)value;
        _widthLabel.Text = $"Chunk Width: {_width}";
    }

    private void _on_height_slider_value_changed(double value)
    {
        _height = (int)value;
        _heightLabel.Text = $"Chunk Height: {_height}";
    }

    private void _on_fill_prob_slider_value_changed(double value)
    {
        _fillProbability = (float)value;
        _fillProbLabel.Text = $"Initial Density: {value:F2}";
    }

    private void _on_smooth_iter_slider_value_changed(double value)
    {
        _smoothingIterations = (int)value;
        _smoothIterLabel.Text = $"Num Steps: {_smoothingIterations}";
    }

    private void _on_birth_lim_slider_value_changed(double value)
    {
        _birthLimit = (int)value;
        _birthLimLabel.Text = $"Birth Limit: {_birthLimit}";
    }

    private void _on_death_lim_slider_value_changed(double value)
    {
        _deathLimit = (int)value;
        _deathLimLabel.Text = $"Death Limit: {_deathLimit}";
    }

    private void _on_seed_input_text_changed(string text)
    {
        if (int.TryParse(text, out int value))
            _seed = value;
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
