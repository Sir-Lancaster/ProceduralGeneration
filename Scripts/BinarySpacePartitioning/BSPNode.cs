using Godot;

public class BSPNode
{
    public Rect2I Region { get; set; }
    public Rect2I? Room { get; set; }
    public Vector2I? CorridorStart { get; set;}
    public Vector2I? CorridorEnd { get; set; }
    public BSPNode Left { get; set; }
    public BSPNode Right { get; set; }
    public bool IsLeaf => Left == null && Right == null;

    public BSPNode(Rect2I region)
    {
        Region = region;
    }
}