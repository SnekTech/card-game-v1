using CardGameV1.Constants;
using GodotUtilities;

namespace CardGameV1.StatusSystem.UI;

public partial class StatusContainer : GridContainer
{
    public event Action? Clicked;

    public override void _GuiInput(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed(InputActions.LeftMouse))
        {
            Clicked?.Invoke();
        }
    }

    public void AddStatusUI(Status status)
    {
        var newStatusUI = SceneFactory.Instantiate<StatusUI>();
        AddChild(newStatusUI);
        newStatusUI.Status = status;
    }

    public void RemoveStatusUI(string statusId)
    {
        var toRemove = this.GetChildrenOfType<StatusUI>().FirstOrDefault(statusUI => statusUI.Status.Id == statusId);
        toRemove?.QueueFree();
    }
}