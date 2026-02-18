using Godot;

public partial class WaveFunctionDemo : Node2D
{
    private WaveFunctionCollapse _wfc;
    private WaveWorldRenderer _renderer;

    private HSlider _widthSlider;
    private HSlider _heightSlider;
    private OptionButton _weightSetButton;
    private LineEdit _seedInput;
    private Button _generateButton;
    private Button _backButton;
    private Label _widthLabel;
    private Label _heightLabel;

    private const int CellSize = 16;

    private static readonly float[][] WeightPresets = new float[][]
    {
        new float[] { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
        new float[] { 0,0,0,0,0,5,3,3,3,3,1,4,1,1,4,1 },
        new float[] { 0,0,0,0,0,0,0,0,0,0,1,0,1,1,0,1 },
        new float[] { 1,0,0,0,0,10,2,2,2,2,1,1,1,1,1,1 },
    };

    public override void _Ready()
    {
        var ui = GetNode<Control>("CanvasLayer/UIPanel");
        _widthSlider     = ui.GetNode<HSlider>("WidthSlider");
        _heightSlider    = ui.GetNode<HSlider>("HeightSlider");
        _weightSetButton = ui.GetNode<OptionButton>("OptionButton");
        _seedInput       = ui.GetNode<LineEdit>("TextEdit");
        _generateButton  = ui.GetNode<Button>("GenerateButton");
        _backButton      = ui.GetNode<Button>("BackToMenuButton");
        _widthLabel      = ui.GetNode<Label>("Width");
        _heightLabel     = ui.GetNode<Label>("Height");

        _weightSetButton.Clear();
        _weightSetButton.AddItem("All Tiles", 0);
        _weightSetButton.AddItem("Prefer Roads", 1);
        _weightSetButton.AddItem("Only Corners", 2);
        _weightSetButton.AddItem("Prefer 4-Way", 3);

        // Connect signals
        _generateButton.Pressed += OnGeneratePressed;
        _backButton.Pressed += OnBackPressed;
        _widthSlider.ValueChanged += OnWidthSliderChanged;
        _heightSlider.ValueChanged += OnHeightSliderChanged;

        // Set initial label values
        _widthLabel.Text  = $"Chunk Width: {(int)_widthSlider.Value}";
        _heightLabel.Text = $"Chunk Height: {(int)_heightSlider.Value}";

        _renderer = GetNode<WaveWorldRenderer>("WaveWorldRenderer");

        OnGeneratePressed();
    }

    private void OnWidthSliderChanged(double value)
    {
        _widthLabel.Text = $"Chunk Width: {(int)value}";
    }

    private void OnHeightSliderChanged(double value)
    {
        _heightLabel.Text = $"Chunk Height: {(int)value}";
    }

    private void OnGeneratePressed()
    {
        int width  = (int)_widthSlider.Value;
        int height = (int)_heightSlider.Value;

        if (int.TryParse(_seedInput.Text, out int seed) && seed != 0)
            GD.Seed((ulong)seed);
        else
            GD.Randomize();

        float[] weights = WeightPresets[_weightSetButton.Selected];

        _wfc = new WaveFunctionCollapse(width, height);
        _renderer.Initialize(_wfc, width, height, CellSize);
        _renderer.Regenerate(weights);
    }

    private void OnBackPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
    }
}
