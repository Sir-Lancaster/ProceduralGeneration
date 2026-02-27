using Godot;

public partial class MainMenu : Control
{
	private void _on_cellular_automata_button_pressed()
	{
		GetTree().Root.GetNode<SceneManager>("SceneManager").GoToCellularAutomataDemo();
	}

	private void _on_perlin_noise_button_pressed()
	{
		GetTree().Root.GetNode<SceneManager>("SceneManager").GoToPerlinNoiseDemo();
	}
	
	private void _on_wave_function_nav_pressed()
	{
		GetTree().Root.GetNode<SceneManager>("SceneManager").GoToWaveFunctionDemo();
	}
	
	private void _on_bsp_nav_pressed()
	{
		GetTree().Root.GetNode<SceneManager>("SceneManager").GoToBSPDemo();
	}
}
