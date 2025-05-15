using Godot;
using GodotUtilities;

namespace CardGameV1.UI;

[Scene]
public partial class Tooltip : PanelContainer
{
    private const float FadeSeconds = 0.2f;

    [Node]
    private TextureRect tooltipIcon = null!;

    [Node]
    private RichTextLabel tooltipText = null!;

    private Tween? _tween;

    public override void _Ready()
    {
        this.SetModulateAlpha(0);
        Hide();
    }

    public void ShowTooltip(Texture2D icon, string text)
    {
        _tween?.KillIfValid();

        tooltipIcon.Texture = icon;
        tooltipText.Text = text;

        ShowAnimation();
    }

    public void HideTooltip()
    {
        _tween?.KillIfValid();

        HideAnimation();
    }

    private void ShowAnimation()
    {
        _tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
        _tween.TweenCallback(Callable.From(Show));
        _tween.TweenProperty(this, "modulate", Colors.White, FadeSeconds);
    }

    private void HideAnimation()
    {
        _tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
        _tween.TweenProperty(this, "modulate", Colors.Transparent, FadeSeconds);
        _tween.TweenCallback(Callable.From(Hide));
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}