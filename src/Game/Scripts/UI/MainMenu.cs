using Godot;
using GodotUtilities;

namespace CardGameV1.UI;

[Scene]
public partial class MainMenu : Control
{
    [Node]
    private Button continueButton = null!;
    [Node]
    private Button newRunButton = null!;
    [Node]
    private Button exitButton = null!;

    public override void _Ready()
    {
        GetTree().Paused = false;
    }

    public override void _EnterTree()
    {
        continueButton.Pressed += OnContinuePressed;
        newRunButton.Pressed += OnNewRunPressed;
        exitButton.Pressed += OnExitPressed;
    }

    public override void _ExitTree()
    {
        continueButton.Pressed -= OnContinuePressed;
        newRunButton.Pressed -= OnNewRunPressed;
        exitButton.Pressed -= OnExitPressed;
    }

    private void OnContinuePressed()
    {
        GD.Print("continue run");
    }

    private void OnNewRunPressed()
    {
        GD.Print("new run");
    }

    private void OnExitPressed() => GetTree().Quit();

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}