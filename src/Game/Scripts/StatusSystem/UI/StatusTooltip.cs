namespace CardGameV1.StatusSystem.UI;

[SceneTree]
public partial class StatusTooltip : HBoxContainer
{
    private Status _status = null!;

    public Status Status
    {
        set
        {
            _status = value;
            Update(_status);
        }
    }

    private void Update(Status status)
    {
        _.Icon.Texture = status.Icon;
        _.Label.Text = status.Tooltip;
    }
}