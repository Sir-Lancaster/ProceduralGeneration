using Godot;

public partial class PerlinWorldRenderer : Node2D
{
	private float[,] _noiseMap;
	private int _cellSize = 4;

	public void SetNoiseMap(float[,] noiseMap)
	{
		_noiseMap = noiseMap;
		QueueRedraw();
	}

    public override void _Draw()
    {
        if (_noiseMap == null) return;

		int width = _noiseMap.GetLength(0);
		int height = _noiseMap.GetLength(1);

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				float noiseValue = _noiseMap[x, y];

				// Map values to colors.
				Color color = GetColorFromHeight(noiseValue);

				DrawRect(new Rect2(x * _cellSize, y * _cellSize, _cellSize, _cellSize), color);
			}
		}
    }

	private static Color GetColorFromHeight(float height)
	{
		// Normalize from [-1, 1] to [0, 1].
		float normalized = (height + 1f) / 2f;

		// Map to terrain colors.
		if (normalized < 0.3f) 
			return Colors.DarkBlue;		// Deep Water
		else if (normalized < 0.4f)
            return Colors.Blue;          // Shallow water
        else if (normalized < 0.45f)
            return Colors.SandyBrown;    // Beach
        else if (normalized < 0.6f)
            return Colors.Green;         // Grass
        else if (normalized < 0.7f)
            return Colors.DarkGreen;     // Forest
        else if (normalized < 0.8f)
            return Colors.Gray;          // Mountain
        else
            return Colors.White;         // Snow peaks 
	}

}
