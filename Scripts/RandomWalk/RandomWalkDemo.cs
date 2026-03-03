using Godot;

public partial class RandomWalkDemo : Node2D
{
	private RandomWalk _randomWalk;

    // Nodes and Labels.
    private RandomWalkWorldRenderer _randomWalkWorldRenderer;
    private Label _minStepsLabel;
    private Label _maxStepsLabel;
    private Label _stepChanceLabel;
    private Label _branchChanceLabel;

    // Default values.
    private int _minSteps = 10;
    private int _maxSteps = 30;
    private float _stepChance = 1.00f;
    private bool _allowLoops = true;
    private bool _allowBranches = true;
    private bool _allowBranchesToConnect = false;
    private float _branchChance = 0.30f;
    private int _seed = 0;

    public override void _Ready()
    {
        _randomWalkWorldRenderer = GetNode<RandomWalkWorldRenderer>("RandomWalkWorldRenderer");
        _minStepsLabel = GetNode<Label>("CanvasLayer/UIPanel/MinStepsLabel");
        _maxStepsLabel = GetNode<Label>("CanvasLayer/UIPanel/MaxStepsLabel");
        _stepChanceLabel = GetNode<Label>("CanvasLayer/UIPanel/StepChanceLabel");
        _branchChanceLabel = GetNode<Label>("CanvasLayer/UIPanel/BranchChanceLabel");

        // Connect signals from UI.
        GetNode<HSlider>("CanvasLayer/UIPanel/MinStepsSlider").ValueChanged += _on_min_steps_slider_value_changed;
        GetNode<HSlider>("CanvasLayer/UIPanel/MaxStepsSlider").ValueChanged += _on_max_steps_slider_value_changed;
        GetNode<HSlider>("CanvasLayer/UIPanel/StepChanceSlider").ValueChanged += _on_step_chance_slider_value_changed;
        GetNode<CheckButton>("CanvasLayer/UIPanel/LoopsGrouping/LoopsCheckButton").Toggled += _on_loops_check_button_toggled_on;
        GetNode<CheckButton>("CanvasLayer/UIPanel/BranchEnableGroup/BranchCheckButton").Toggled += _on_branch_check_button_toggled_on;
        GetNode<CheckButton>("CanvasLayer/UIPanel/BranchConnectGroup/BranchConnectCheckButton").Toggled += _on_branch_connect_check_button_toggled_on;
        GetNode<HSlider>("CanvasLayer/UIPanel/BranchChanceSlider").ValueChanged += _on_branch_chance_slider_value_changed;
        GetNode<Button>("CanvasLayer/UIPanel/GenerateButton").Pressed += _on_generate_button_pressed;
        GetNode<Button>("CanvasLayer/UIPanel/BackToMenuButton").Pressed += _on_back_to_menu_button_pressed;

        // Set initial label text.
        _minStepsLabel.Text = $"Min Steps: {_minSteps}";
        _maxStepsLabel.Text = $"Max Steps: {_maxSteps}";
        _stepChanceLabel.Text = $"Step Chance: {_stepChance:F2}";
        _branchChanceLabel.Text = $"Branch Chance: {_branchChance:F2}";

        GenerateWorld();
    }

    private void _on_min_steps_slider_value_changed(double value)
    {
        _minSteps = (int)value;
        _minStepsLabel.Text = $"Min Steps: {_minSteps}";
    }


    private void _on_max_steps_slider_value_changed(double value)
    {
        _maxSteps = (int)value;
        _maxStepsLabel.Text = $"Max Steps: {_maxSteps}";
    }


    private void _on_step_chance_slider_value_changed(double value)
    {
         _stepChance = (float)value;
         _stepChanceLabel.Text = $"Step Chance: {_stepChance:F2}";
    }


    private void _on_loops_check_button_toggled_on(bool value)
    {
        _allowLoops = value;
    }


    private void _on_branch_check_button_toggled_on(bool value)
    {
         _allowBranches = value;
    }


    private void _on_branch_connect_check_button_toggled_on(bool value)
    {
         _allowBranchesToConnect = value;
    }


    private void _on_branch_chance_slider_value_changed(double value)
    {
         _branchChance = (float)value;
         _branchChanceLabel.Text = $"Branch Chance: {_branchChance:F2}";
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


    private void GenerateWorld()
    {
        _randomWalk = new RandomWalk();
        _randomWalk.Generate(_minSteps, _maxSteps, _seed, _stepChance, _branchChance, _allowLoops, _allowBranches, _allowBranchesToConnect);
        _randomWalkWorldRenderer.Initialize(_randomWalk);
    }
}
