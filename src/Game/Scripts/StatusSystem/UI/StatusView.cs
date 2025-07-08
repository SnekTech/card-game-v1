using CardGameV1.Constants;
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
        _.BackButton.Pressed += OnBackButtonPressed;
    }

    public override void _ExitTree()
    {
        _.BackButton.Pressed -= OnBackButtonPressed;
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed(InputActions.UICancel) && Visible)
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

    public void ShowView(List<Status> statusList)
    {
        foreach (var status in statusList)
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
    
    private void OnBackButtonPressed() => HideView();
}