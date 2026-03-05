using System;
using Godot;

public partial class BSPDemo : Node2D
{
    private BinarySpace _bsp;
    
    // Nodes and Labels.
    private BSPWorldRenderer _bspWorldRenderer;
    private Label _heightLable;
    private Label _widthLabel;
    private Label _minDepthLabel;
    private Label _maxDepthLabel;
    private Label _splitChanceLabel;

    // Input default values.
    private int _width = 10000;
    private int _height = 10000;
    private int _minDepth = 10;
    private int _maxDepth = 15;
    private float _splitChance = 0.8f;
    private int _seed = 0;

    public override void _Ready()
    {
        _bspWorldRenderer = GetNode<BSPWorldRenderer>("BSPWorldRenderer");
        _heightLable = GetNode<Label>("CanvasLayer/UIPanel/HeightLabel");
        _widthLabel = GetNode<Label>("CanvasLayer/UIPanel/WidthLabel");
        _minDepthLabel = GetNode<Label>("CanvasLayer/UIPanel/MinDepthLabel");
        _maxDepthLabel = GetNode<Label>("CanvasLayer/UIPanel/MaxDepthLabel");
        _splitChanceLabel = GetNode<Label>("CanvasLayer/UIPanel/SplitChanceLabel");

        // Connect signals from UI.
        GetNode<HSlider>("CanvasLayer/UIPanel/HeightSlider").ValueChanged += _on_height_slider_value_changed;
        GetNode<HSlider>("CanvasLayer/UIPanel/WidthSlider").ValueChanged += _on_width_slider_value_changed;
        GetNode<HSlider>("CanvasLayer/UIPanel/MinDepthSlider").ValueChanged += _on_min_depth_slider_value_changed;
        GetNode<HSlider>("CanvasLayer/UIPanel/MaxDepthSlider").ValueChanged += _on_max_depth_slider_value_changed;
        GetNode<HSlider>("CanvasLayer/UIPanel/SplitChanceSlider").ValueChanged += _on_split_chance_slider_value_changed;
        GetNode<LineEdit>("CanvasLayer/UIPanel/SeedInput").TextChanged += _on_seed_input_text_changed;
        GetNode<Button>("CanvasLayer/UIPanel/GenerateButton").Pressed += _on_generate_button_pressed;
        GetNode<Button>("CanvasLayer/UIPanel/MenuButton").Pressed += _on_menu_button_pressed;

        // Set initial label text.
        _heightLable.Text = $"Chunk Height: {_height}";
        _widthLabel.Text = $"Chunk Width: {_width}";
        _minDepthLabel.Text = $"Min Depth: {_minDepth}";
        _maxDepthLabel.Text = $"Max Depth: {_maxDepth}";
        _splitChanceLabel.Text = $"Split Chance: {_splitChance:F2}";

        GenerateWorld();
    }
    
    private void GenerateWorld()
    {
        _bsp = new BinarySpace();
        int seed = _seed == 0 ? new Random().Next() : _seed;
        _bsp.Generate(_maxDepth, _minDepth, _width, _height, seed, _splitChance);
        _bspWorldRenderer.Initialize(_bsp, 1);
    }

    private void _on_height_slider_value_changed(double value)
    {
        _height = (int)value;
        _heightLable.Text = $"Chunk Height: {_height}";
    }
    
    private void _on_width_slider_value_changed(double value)
    {
        _width = (int)value;
        _widthLabel.Text = $"Chunk Width: {_width}";
    }

    private void _on_min_depth_slider_value_changed(double value)
    {
        _minDepth = (int)value;
        _minDepthLabel.Text = $"Min Depth: {_minDepth}";
    }

    private void _on_max_depth_slider_value_changed(double value)
    {
        _maxDepth = (int)value;
        _maxDepthLabel.Text = $"Max Depth: {_maxDepth}";
    }

    private void _on_split_chance_slider_value_changed(double value)
    {
        _splitChance = (float)value;
        _splitChanceLabel.Text = $"Split Chance: {_splitChance:F2}";
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

    private void _on_menu_button_pressed()
    {
        GetNode<SceneManager>("/root/SceneManager").GoToMainMenu();
    }
}