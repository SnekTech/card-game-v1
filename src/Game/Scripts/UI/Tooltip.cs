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
    private bool _isVisible;

    public override void _Ready()
    {
        var eventBus = EventBus.EventBusOwner.CardEventBus;
        eventBus.CardTooltipRequested += ShowTooltip;
        eventBus.TooltipHideRequested += HideTooltip;
        
        this.SetModulateAlpha(0);
        Hide();
    }

    private void ShowTooltip(Texture2D icon, string text)
    {
        _isVisible = true;
        _tween?.KillIfValid();

        tooltipIcon.Texture = icon;
        tooltipText.Text = text;

        ShowAnimation();
    }

    private void HideTooltip()
    {
        _isVisible = false;
        _tween?.KillIfValid();

        var timer = this.CreateSceneTreeTimer(FadeSeconds);
        timer.Timeout += HideAnimation;
    }

    private void ShowAnimation()
    {
        _tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
        _tween.TweenCallback(Callable.From(Show));
        _tween.TweenProperty(this, "modulate", Colors.White, FadeSeconds);
    }

    private void HideAnimation()
    {
        if (_isVisible)
            return;
        
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