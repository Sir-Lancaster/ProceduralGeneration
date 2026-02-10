using Godot;

public partial class AutomataWorldRenderer : BaseWorldRenderer
{
    private bool[,] _grid;

    public AutomataWorldRenderer()
    {
        _cellSize = 10;
    }

    public void SetGrid(bool[,] grid)
    {
        _grid = grid;
        QueueRedraw();
    }

    public override void _Draw()
    {
        if (_grid == null) return;
        DrawGrid(_grid.GetLength(0), _grid.GetLength(1), (x, y) =>
            _grid[x, y] ? Colors.White : Colors.Black);
    }
}
