using Godot;
using GodotUtilities;

namespace CardGameV1.StatusSystem.UI;

[Scene]
public partial class StatusUI : Control
{
    [Node]
    private TextureRect icon = null!;
    [Node]
    private Label duration = null!;
    [Node]
    private Label stacks = null!;

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

        if (_status.CanExpire && _status.Duration <= 0)
        {
            QueueFree();
        }

        if (_status.StackType == StackType.Intensity && _status.Stacks == 0)
        {
            QueueFree();
        }

        duration.Text = _status.Duration.ToString();
        stacks.Text = _status.Stacks.ToString();
    }

    private void UpdateContent(Status status)
    {
        icon.Texture = status.Icon;
        duration.Visible = status.StackType == StackType.Duration;
        stacks.Visible = status.StackType == StackType.Intensity;
        CustomMinimumSize = icon.Size;

        if (duration.Visible)
        {
            CustomMinimumSize = duration.Size + duration.Position;
        }
        else if (stacks.Visible)
        {
            CustomMinimumSize = stacks.Size + stacks.Position;
        }
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}