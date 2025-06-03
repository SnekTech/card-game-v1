using CardGameV1.EventBus;
using Godot;
using GodotUtilities;

namespace CardGameV1.UI.BattleUIComponents;

[Scene]
public partial class BattleOverPanel : Panel
{
    [Node]
    private Label label = null!;

    [Node]
    private Button continueButton = null!;

    [Node]
    private Button restartButton = null!;

    private static readonly BattleEvents BattleEvents = EventBusOwner.BattleEvents;

    public override void _Ready()
    {
        WireNodes();
    }

    public override void _EnterTree()
    {
        continueButton.Pressed += OnContinuePressed;
        restartButton.Pressed += OnRestartPressed;
        BattleEvents.BattleOverScreenRequested += ShowScreen;
    }

    public override void _ExitTree()
    {
        continueButton.Pressed -= OnContinuePressed;
        restartButton.Pressed -= OnRestartPressed;
        BattleEvents.BattleOverScreenRequested -= ShowScreen;
    }

    private void ShowScreen(string text, PanelType type)
    {
        label.Text = text;
        continueButton.Visible = type == PanelType.Win;
        restartButton.Visible = type == PanelType.Lose;
        Show();
        GetTree().Paused = true;
    }

    private void OnContinuePressed() => BattleEvents.EmitWon();

    private void OnRestartPressed() => GetTree().ReloadCurrentScene();

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }

    public enum PanelType
    {
        Win,
        Lose
    }
}