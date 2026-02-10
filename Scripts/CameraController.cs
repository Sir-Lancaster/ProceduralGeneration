using Godot;

public partial class CameraController : Camera2D
{
    [Export] 
    public float ZoomSpeed = 0.1f;
    [Export] 
    public float MinZoom = 0.1f;
    [Export] 
    public float MaxZoom = 3.0f;

    private bool _isDragging = false;
    private Vector2 _dragStart;

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.Middle)
            {
                if (mouseButton.Pressed)
                {
                    _isDragging = true;
                    _dragStart = GetGlobalMousePosition();
                }
                else
                {
                    _isDragging = false;
                }
            }
            else if (mouseButton.ButtonIndex == MouseButton.WheelUp)
            {
                ZoomCamera(ZoomSpeed);
            }
            else if (mouseButton.ButtonIndex == MouseButton.WheelDown)
            {
                ZoomCamera(-ZoomSpeed);
            }
        }

        if (@event is InputEventMouseMotion && _isDragging)
        {
            Vector2 currentMousePos = GetGlobalMousePosition();
            GlobalPosition += _dragStart - currentMousePos;
        }
    }

    private void ZoomCamera(float delta)
    {
        Vector2 newZoom = Zoom + Vector2.One * delta;
        newZoom.X = Mathf.Clamp(newZoom.X, MinZoom, MaxZoom);
        newZoom.Y = Mathf.Clamp(newZoom.Y, MinZoom, MaxZoom);
        Zoom = newZoom;
    }
}