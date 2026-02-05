using Godot;

public partial class WorldRenderer : Node2D
{
    private bool[,] _grid;
    private int _cellsize = 10;

    public void SetGrid(bool[,] grid)
    {
        _grid = grid;
        QueueRedraw();
    }

    public override void _Draw()
    {
        if (_grid == null) return;
        
        int width = _grid.GetLength(0);
        int height = _grid.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y <height; y++)
            {
                Color color = _grid[x, y] ? Colors.White : Colors.Black;
                DrawRect(new Rect2(x * _cellsize, y * _cellsize, _cellsize, _cellsize), color);
            }
        }
    }
}
