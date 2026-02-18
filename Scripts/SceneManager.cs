using Godot;

public partial class SceneManager : Node
{
    private PackedScene _mainMenuScene;
    private PackedScene _cellularAutomataScene;
    private PackedScene _perlinNoiseScene;
    private PackedScene _waveCollapseScene;

    public override void _Ready()
    {
        // Preload scenes
        _mainMenuScene = GD.Load<PackedScene>("res://Scenes/MainMenu.tscn");
        _cellularAutomataScene = GD.Load<PackedScene>("res://Scenes/CellularAutomataDemo.tscn");
        _perlinNoiseScene = GD.Load<PackedScene>("res://Scenes/PerlinNoiseDemo.tscn");
        _waveCollapseScene = GD.Load<PackedScene>("res://Scenes/WaveFunctionDemo.tscn");
    }

    public void GoToMainMenu()
    {
        GetTree().ChangeSceneToPacked(_mainMenuScene);
    }

    public void GoToCellularAutomataDemo()
    {
        GetTree().ChangeSceneToPacked(_cellularAutomataScene);
    }

    public void GoToPerlinNoiseDemo()
    {
        GetTree().ChangeSceneToPacked(_perlinNoiseScene);
    }

    public void GoToWaveFunctionDemo()
    {
        GetTree().ChangeSceneToPacked(_waveCollapseScene);
    }
}
