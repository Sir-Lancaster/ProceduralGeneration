using Godot;

public partial class WaveWorldRenderer : BaseWorldRenderer
{
    private WaveFunctionCollapse _wfc;
    private int _width;
    private int _height;
    private bool _generated = false;

    private static readonly Color BgColor = new Color(0.15f, 0.15f, 0.15f);
    private static readonly Color LineColor = new Color(0.9f, 0.9f, 0.9f);

    public void Initialize(WaveFunctionCollapse wfc, int width, int height, int cellSize)
    {
        _wfc = wfc;
        _width = width;
        _height = height;
        _cellSize = cellSize;
    }

    public void Regenerate(float[] weights)
    {
        bool success = _wfc.Generate(weights);
        if (!success)
            GD.PrintErr("WFC: Contradiction detected, generation failed.");

        _generated = success;
        QueueRedraw();
    }

    public override void _Draw()
    {
        if (!_generated) return;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                DrawTile(x, y, _wfc.GetTile(x, y));
            }
        }
    }

    private void DrawTile(int x, int y, int tileId)
    {
        float px = x * _cellSize;
        float py = y * _cellSize;
        float half = _cellSize / 2f;
        float lineWidth = _cellSize / 5f;

        // Draw background
        DrawRect(new Rect2(px, py, _cellSize, _cellSize), BgColor);

        // Center point of this cell
        Vector2 center = new Vector2(px + half, py + half);

        // Edge midpoints
        Vector2 north = new Vector2(px + half, py);
        Vector2 east  = new Vector2(px + _cellSize, py + half);
        Vector2 south = new Vector2(px + half, py + _cellSize);
        Vector2 west  = new Vector2(px, py + half);

        // Draw connections based on tile ID
        // N      E      S      W
        bool n, e, s, w;
        switch (tileId)
        {
            case 0:  n=false; e=false; s=false; w=false; break; // Blank
            case 1:  n=true;  e=false; s=false; w=false; break; // Dead end up
            case 2:  n=false; e=true;  s=false; w=false; break; // Dead end right
            case 3:  n=false; e=false; s=true;  w=false; break; // Dead end down
            case 4:  n=false; e=false; s=false; w=true;  break; // Dead end left
            case 5:  n=true;  e=true;  s=true;  w=true;  break; // Four-way
            case 6:  n=true;  e=true;  s=true;  w=false; break; // T (N,E,S)
            case 7:  n=true;  e=true;  s=false; w=true;  break; // T (N,E,W)
            case 8:  n=true;  e=false; s=true;  w=true;  break; // T (N,S,W)
            case 9:  n=false; e=true;  s=true;  w=true;  break; // T (E,S,W)
            case 10: n=false; e=true;  s=true;  w=false; break; // Elbow (E,S)
            case 11: n=false; e=true;  s=false; w=true;  break; // Straight horizontal
            case 12: n=false; e=false; s=true;  w=true;  break; // Elbow (S,W)
            case 13: n=true;  e=true;  s=false; w=false; break; // Elbow (N,E)
            case 14: n=true;  e=false; s=true;  w=false; break; // Straight vertical
            case 15: n=true;  e=false; s=false; w=true;  break; // Elbow (N,W)
            default: n=false; e=false; s=false; w=false; break;
        }

        // Draw a dot at center if any connections exist
        if (n || e || s || w)
            DrawCircle(center, lineWidth * 0.75f, LineColor);

        if (n) DrawLine(center, north, LineColor, lineWidth);
        if (e) DrawLine(center, east,  LineColor, lineWidth);
        if (s) DrawLine(center, south, LineColor, lineWidth);
        if (w) DrawLine(center, west,  LineColor, lineWidth);
    }
}
