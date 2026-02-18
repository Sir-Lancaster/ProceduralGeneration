using Godot;

public abstract partial class BaseWorldRenderer : Node2D
{
    protected int _cellSize;
    protected bool _useTileMap = false;
    protected TileMapLayer _tileMapLayer;

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

    // New method for TileMap rendering.
    protected void RenderToTileMap(int width, int height, System.Func<int, int, Vector2I> getAtlasCoords)
    {
        if (_tileMapLayer == null) return;

        _tileMapLayer.Clear();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2I atlasCoords = getAtlasCoords(x, y);
                _tileMapLayer.SetCell(new Vector2I(x, y), 0, atlasCoords);
            }
        }
    }
}