namespace CardGameV1.StatusSystem.UI;

[SceneTree]
public partial class StatusUI : Control
{
    private Status? _status;

    public Status Status
    {
        get => _status!;
        set
        {
            if (_status != null)
            {
                _status.Changed -= OnStatusChanged;
            }

            _status = value;
            _status.Changed += OnStatusChanged;
            UpdateContent(_status);
            OnStatusChanged();
        }
    }

    #region lifecycle

    public override void _ExitTree()
    {
        if (_status != null)
        {
            _status.Changed -= OnStatusChanged;
        }
    }

    #endregion

    private void OnStatusChanged()
    {
        if (_status == null)
            return;

        Duration.Text = _status.Duration.ToString();
        Stacks.Text = _status.Stacks.ToString();
    }

    private void UpdateContent(Status status)
    {
        Icon.Texture = status.Icon;
        Duration.Visible = status.Duration > 0;
        Stacks.Visible = status.Stacks != 0;
        CustomMinimumSize = Icon.Size;

        if (Duration.Visible)
        {
            CustomMinimumSize = Duration.Size + Duration.Position;
        }
        else if (Stacks.Visible)
        {
            CustomMinimumSize = Stacks.Size + Stacks.Position;
        }
    }
}