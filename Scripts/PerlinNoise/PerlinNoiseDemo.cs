using Godot;

public partial class PerlinNoiseDemo : Node2D
{
    private PerlinWorldRenderer _worldRenderer;
    private Label _widthLabel;
    private Label _heightLabel;
    private Label _octavesLabel;
    private Label _deepWaterLabel;
    private Label _shallowWaterLabel;
    private Label _beachLabel;
    private Label _grassLabel;
    private Label _mountainLabel;

    private int _width = 200;
    private int _height = 200;
    private int _octaves = 4;
    private float _persistence = 0.9f;
    private float _lacunarity = 2.0f;
    private float _scale = 0.01f;
    private int _seed = 0;
    private bool _useFbm = true;

    private float _deepWaterThreshold = -0.5f;
    private float _shallowWaterThreshold = -0.2f;
    private float _beachThreshold = 0.0f;
    private float _grassThreshold = 0.5f;
    private float _mountainThreshold = 0.8f;

    public override void _Ready()
    {
        _worldRenderer = GetNode<PerlinWorldRenderer>("PerlinWorldRenderer");
        _widthLabel = GetNode<Label>("CanvasLayer/UIPanel/WidthLabel");
        _heightLabel = GetNode<Label>("CanvasLayer/UIPanel/HeightLabel");
        _octavesLabel = GetNode<Label>("CanvasLayer/UIPanel/OctavesLabel");
        _deepWaterLabel = GetNode<Label>("CanvasLayer/UIPanel/DeepWaterLabel");
        _shallowWaterLabel = GetNode<Label>("CanvasLayer/UIPanel/ShallowWaterLabel");
        _beachLabel = GetNode<Label>("CanvasLayer/UIPanel/BeachLabel");
        _grassLabel = GetNode<Label>("CanvasLayer/UIPanel/GrassLabel");
        _mountainLabel = GetNode<Label>("CanvasLayer/UIPanel/MountainLabel");

        // Connect signals from UI scene
        GetNode<HSlider>("CanvasLayer/UIPanel/WidthSlider").ValueChanged += _on_width_slider_value_changed;
        GetNode<HSlider>("CanvasLayer/UIPanel/HeightSlider").ValueChanged += _on_height_slider_value_changed;
        GetNode<CheckBox>("CanvasLayer/UIPanel/UseFbmCheckbox").Toggled += _on_use_fbm_checkbox_toggled;
        GetNode<HSlider>("CanvasLayer/UIPanel/OctavesSlider").ValueChanged += _on_octaves_slider_value_changed;
        GetNode<LineEdit>("CanvasLayer/UIPanel/SeedInput").TextChanged += _on_seed_input_text_changed;
        GetNode<HSlider>("CanvasLayer/UIPanel/DeepWaterSlider").ValueChanged += _on_deep_water_slider_value_changed;
        GetNode<HSlider>("CanvasLayer/UIPanel/ShallowWaterSlider").ValueChanged += _on_shallow_water_slider_value_changed;
        GetNode<HSlider>("CanvasLayer/UIPanel/BeachSlider").ValueChanged += _on_beach_slider_value_changed;
        GetNode<HSlider>("CanvasLayer/UIPanel/GrassSlider").ValueChanged += _on_grass_slider_value_changed;
        GetNode<HSlider>("CanvasLayer/UIPanel/MountainSlider").ValueChanged += _on_mountain_slider_value_changed;
        GetNode<Button>("CanvasLayer/UIPanel/GenerateButton").Pressed += _on_generate_button_pressed;
        GetNode<Button>("CanvasLayer/UIPanel/BackToMenuButton").Pressed += _on_back_to_menu_button_pressed;

        _widthLabel.Text = $"Chunk Width: {_width}";
        _heightLabel.Text = $"Chunk Height: {_height}";
        _octavesLabel.Text = $"Num Octaves: {_octaves}";
        _deepWaterLabel.Text = $"Deep Water Threshold: {_deepWaterThreshold}";
        _shallowWaterLabel.Text = $"Shallow Water Threshold: {_shallowWaterThreshold}";
        _beachLabel.Text = $"Beach Threshold: {_beachThreshold}";
        _grassLabel.Text = $"Grass: {_grassThreshold}";
        _mountainLabel.Text = $"Mountain Threshold: {_mountainThreshold}";

        ApplyThresholds();
        GenerateWorld();
    }

    private void ApplyThresholds()
    {
        _worldRenderer.DeepWaterThreshold = (_deepWaterThreshold + 1f) / 2f;
        _worldRenderer.ShallowWaterThreshold = (_shallowWaterThreshold + 1f) / 2f;
        _worldRenderer.BeachThreshold = (_beachThreshold + 1f) / 2f;
        _worldRenderer.GrassThreshold = (_grassThreshold + 1f) / 2f;
        _worldRenderer.MountainThreshold = (_mountainThreshold + 1f) / 2f;
    }

    private void GenerateWorld()
    {
        var seedInput = GetNode<LineEdit>("CanvasLayer/UIPanel/SeedInput");
        if (int.TryParse(seedInput.Text, out int parsedSeed))
            _seed = parsedSeed;

        int seed = _seed == 0 ? (int)GD.Randi() : _seed;
        int[] permTable = PerlinNoise.GeneratePermutationTable(seed);
        float[,] noiseMap = new float[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (_useFbm)
                {
                    noiseMap[x, y] = PerlinNoise.FractalNoise2D(
                        x, y, permTable, _octaves, _persistence, _lacunarity, _scale);
                }
                else
                {
                    noiseMap[x, y] = PerlinNoise.PerlinNoise2D(x * _scale, y * _scale, permTable);
                }
            }
        }

        ApplyThresholds();
        _worldRenderer.SetNoiseMap(noiseMap);
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

    private void _on_use_fbm_checkbox_toggled(bool toggled)
    {
        _useFbm = toggled;
    }

    private void _on_octaves_slider_value_changed(double value)
    {
        _octaves = (int)value;
        _octavesLabel.Text = $"Num Octaves: {_octaves}";
    }

    private void _on_seed_input_text_changed(string text)
    {
        if (int.TryParse(text, out int value))
            _seed = value;
    }

    private void _on_deep_water_slider_value_changed(double value)
    {
        _deepWaterThreshold = (float)value;
        _deepWaterLabel.Text = $"Deep Water Threshold: {value:F2}";
    }

    private void _on_shallow_water_slider_value_changed(double value)
    {
        _shallowWaterThreshold = (float)value;
        _shallowWaterLabel.Text = $"Shallow Water Threshold: {value:F2}";
    }

    private void _on_beach_slider_value_changed(double value)
    {
        _beachThreshold = (float)value;
        _beachLabel.Text = $"Beach Threshold: {value:F2}";
    }

    private void _on_grass_slider_value_changed(double value)
    {
        _grassThreshold = (float)value;
        _grassLabel.Text = $"Grass: {value:F2}";
    }

    private void _on_mountain_slider_value_changed(double value)
    {
        _mountainThreshold = (float)value;
        _mountainLabel.Text = $"Mountain Threshold: {value:F2}";
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
