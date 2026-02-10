using Godot;

public abstract partial class BaseWorldRenderer : Node2D
{
    protected int _cellSize;

    protected void DrawGrid(int width, int height, System.Func<int, int, Color> getColor)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = getColor(x, y);
                DrawRect(new Rect2(x * _cellSize, y * _cellSize, _cellSize, _cellSize), color);
            }
        }
    }
}