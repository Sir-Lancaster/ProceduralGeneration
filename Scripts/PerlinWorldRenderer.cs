using Godot;

public partial class PerlinWorldRenderer : BaseWorldRenderer
{
    private float[,] _noiseMap;

    public float DeepWaterThreshold = 0.3f;
    public float ShallowWaterThreshold = 0.4f;
    public float BeachThreshold = 0.45f;
    public float GrassThreshold = 0.6f;
    public float MountainThreshold = 0.7f;

    public PerlinWorldRenderer()
    {
        _cellSize = 4;
    }

    public void SetNoiseMap(float[,] noiseMap)
    {
        _noiseMap = noiseMap;
        QueueRedraw();
    }

    public override void _Draw()
    {
        if (_noiseMap == null) return;
        DrawGrid(_noiseMap.GetLength(0), _noiseMap.GetLength(1), (x, y) =>
            GetColorFromHeight(_noiseMap[x, y]));
    }

    private Color GetColorFromHeight(float height)
    {
        float normalized = (height + 1f) / 2f;

        if (normalized < DeepWaterThreshold) return Colors.DarkBlue;
        else if (normalized < ShallowWaterThreshold) return Colors.Blue;
        else if (normalized < BeachThreshold) return Colors.SandyBrown;
        else if (normalized < GrassThreshold) return Colors.Green;
        else if (normalized < MountainThreshold) return Colors.Brown;
        else return Colors.White;
    }
}