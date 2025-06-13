using CardGameV1.EventBus;
using Godot;
using GodotUtilities;

namespace CardGameV1.Map;

[Scene]
public partial class MapScene : Control
{
    [Node]
    private Button backButton = null!;

    private readonly MapGenerator _mapGenerator = new();

    public override void _Ready()
    {
        _mapGenerator.GenerateMap();
    }

    public override void _EnterTree()
    {
        backButton.Pressed += OnBackButtonPressed;
    }

    public override void _ExitTree()
    {
        backButton.Pressed -= OnBackButtonPressed;
    }

    private void OnBackButtonPressed() => EventBusOwner.Events.EmitMapExited();

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}