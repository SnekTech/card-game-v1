using CardGameV1.Constants;
using CardGameV1.EventBus;
using CardGameV1.MyExtensions;

namespace CardGameV1.StatusSystem.UI;

[SceneTree]
public partial class StatusView : Control
{
    public override void _Ready()
    {
        StatusTooltipContainer.ClearChildren();
    }

    public override void _EnterTree()
    {
        EventBusOwner.Events.StatusTooltipRequested += OnStatusTooltipRequested;
        _.BackButton.Pressed += OnBackButtonPressed;
    }

    public override void _ExitTree()
    {
        EventBusOwner.Events.StatusTooltipRequested -= OnStatusTooltipRequested;
        _.BackButton.Pressed -= OnBackButtonPressed;
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed(BuiltinInputActions.UICancel) && Visible)
        {
            HideView();
        }
    }

    public override void _GuiInput(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed(InputActions.LeftMouse) && Visible)
        {
            HideView();
        }
    }

    private void ShowView(IEnumerable<Status> statuses)
    {
        foreach (var status in statuses)
        {
            var newStatusTooltip = SceneFactory.Instantiate<StatusTooltip>();
            StatusTooltipContainer.AddChild(newStatusTooltip);
            newStatusTooltip.Status = status;
        }

        Show();
    }

    private void HideView()
    {
        StatusTooltipContainer.ClearChildren();
        Hide();
    }

    private void OnStatusTooltipRequested(IEnumerable<Status> statuses) => ShowView(statuses);
    private void OnBackButtonPressed() => HideView();
}