using System.Collections.Generic;
using Godot;

public class RWRoom
{
    public Vector2I Position { get; set; }
    public List<RWRoom> Neighbors { get; set; }
    public List<Rect2I> Hallways { get; set; }
    
    public RWRoom(Vector2I position)
    {
        Position = position;
        Neighbors = new List<RWRoom>();
        Hallways = new List<Rect2I>();
    }
}