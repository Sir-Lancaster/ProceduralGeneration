using Godot;
public partial class BSPWorldRenderer : BaseWorldRenderer
{
    private BinarySpace _bsp;

    public void Initialize(BinarySpace bsp, int cellSize)
    {
        _bsp = bsp;
        _cellSize = cellSize;
        QueueRedraw();
    }

    public override void _Draw()
    {
        if (_bsp?.Root == null) return;
        DrawNode(_bsp.Root);
    }

    private void DrawNode(BSPNode node)
    {
        // Draw room if leaf. Room is nullable, so need gaurd.
        if (node.IsLeaf && node.Room.HasValue)
        {
            Rect2 pixelRoom = TileToPixel((Rect2I)node.Room);
            DrawRect(pixelRoom, Colors.Crimson);
        }

        // CorridorStart & CorridorEnd are nullable, need gaurd.
        if (node.CorridorStart.HasValue && node.CorridorEnd.HasValue)
        {
            // Convert vector2I into Vector2's for drawing. Vector2I? requires explicit cast.
            Vector2 start = CorridorToVector2((Vector2I)node.CorridorStart);
            Vector2 end = CorridorToVector2((Vector2I)node.CorridorEnd);

            // Draw corridors.
            DrawLine(start, end, Colors.White);

        }

        // Recurse into children, Gaurd against leaves.
        if (!node.IsLeaf)
        {
            DrawNode(node.Left);
            DrawNode(node.Right);
        }
    }

    // Helper for converting the tiles to pixels.
    private Rect2 TileToPixel(Rect2I rect)
    {
        return new Rect2(
            rect.Position.X * _cellSize,
            rect.Position.Y * _cellSize,
            rect.Size.X * _cellSize,
            rect.Size.Y * _cellSize
        );
    }

    // Helper to convert vector2I to vector2
    private Vector2 CorridorToVector2(Vector2I vect)
    {
        return new Vector2(vect.X * _cellSize, vect.Y * _cellSize);
    }
}