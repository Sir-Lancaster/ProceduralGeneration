using Godot;

public partial class PerlinNoiseDemo : Node2D
{
	// Nodes
    private PerlinWorldRenderer _worldRenderer;
	private Label _octavesLabel;
	private Label _persistenceLabel;
	private Label _lacunarityLabel;
	private Label _scaleLabel;
	private Label _seedLabel;
    
    private int _width = 200;
    private int _height = 200;
    private float _scale = 0.05f;
    private int _octaves = 6;
    private float _persistence = 0.5f;
    private float _lacunarity = 2.0f;
    private int _seed = 42;
    
    public override void _Ready()
    {
        // Get all nodes.
        _worldRenderer = GetNode<PerlinWorldRenderer>("PerlinWorldRenderer");
        _octavesLabel = GetNode<Label>("CanvasLayer/UIPanel/OctavesLabel");
        _persistenceLabel = GetNode<Label>("CanvasLayer/UIPanel/PersistenceLabel");  // Changed {} to ()
        _lacunarityLabel = GetNode<Label>("CanvasLayer/UIPanel/LacunarityLabel");
        _scaleLabel = GetNode<Label>("CanvasLayer/UIPanel/ScaleLabel");
		_seedLabel = GetNode<Label>("CanvasLayer/UIPanel/SeedLabel");

		// Change the labels display text
		_octavesLabel.Text = $"Octaves: {_octaves}";
		_persistenceLabel.Text = $"Persistence: {_persistence:F2}";
		_lacunarityLabel.Text = $"Lacunarity: {_lacunarity:F2}";
		_scaleLabel.Text = $"Scale: {_scale:F2}";
		_seedLabel.Text = $"Seed: {_seed}";
        GenerateWorld();
    }

    private void GenerateWorld()
    {
        int[] permTable = PerlinNoise.GeneratePermutationTable(_seed);
        float[,] noiseMap = new float[_width, _height];
        
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                // Use fractal noise instead of basic Perlin.
                noiseMap[x, y] = PerlinNoise.FractalNoise2D(
                    x, y, 
                    permTable, 
                    _octaves, 
                    _persistence, 
                    _lacunarity, 
                    _scale
                );
            }
        }
        
        _worldRenderer.SetNoiseMap(noiseMap);
    }

    // Slider and button callbacks.
    private void _on_octaves_slider_value_changed(int value)
    {
        _octaves = value;
		_octavesLabel.Text = $"Octaves: {value}";
    }

    private void _on_persistence_slider_value_changed(float value)
    {
        _persistence = value;
		_persistenceLabel.Text = $"Persistence: {value}";
    }

    private void _on_lacunarity_slider_value_changed(float value)
    {
        _lacunarity = value;
		_lacunarityLabel.Text = $"Lacunarity: {value}";
    }

    private void _on_scale_slider_value_changed(float value)
    {
        _scale = value;
		_scaleLabel.Text = $"Scale: {value}";
    }

	private void _on_seed_slider_value_changed(int value)
	{
		_seed = value;
		_seedLabel.Text = $"Seed: {value}";
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
