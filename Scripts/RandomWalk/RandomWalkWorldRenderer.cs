using Godot;
public partial class RandomWalkWorldRenderer : BaseWorldRenderer
{
    private RandomWalk _randomWalk;

    public void Initialize(RandomWalk walk)
    {
        _randomWalk = walk;
        QueueRedraw();   
    }

    public override void _Draw()
    {
        if (_randomWalk?.Rooms == null) return;

        foreach (RWRoom room in _randomWalk.Rooms)
        {
            Rect2 pixelRoom = RoomToPixel(room.Position);

            // Draw room.
            DrawRect(pixelRoom, Colors.White);

            // Draw hallways.
            foreach (Rect2I hallway in room.Hallways)
            {
                DrawRect(hallway, Colors.Red);
            }
        }
    }

    private Rect2 RoomToPixel(Vector2I position)
    {
        int cellSize = RandomWalk.ROOM_SIZE + RandomWalk.HALLWAY_WIDTH;
        return new Rect2(
            position.X * cellSize,
            position.Y * cellSize,
            RandomWalk.ROOM_SIZE,
            RandomWalk.ROOM_SIZE
        );
    }
}
