using CardGameV1.EventBus;
using GTweens.Easings;
using GTweensGodot.Extensions;

namespace CardGameV1.UI.BattleUIComponents;

[SceneTree]
public partial class Tooltip : PanelContainer
{
    private const float FadeSeconds = 0.2f;

    private bool _isVisible;
    private static readonly CardEvents CardEvents = EventBusOwner.CardEvents;

    private CancellationTokenSource _cts = new();

    public override void _Ready()
    {
        this.SetModulateAlpha(0);
        Hide();
    }

    public override void _EnterTree()
    {
        CardEvents.CardTooltipRequested += ShowTooltip;
        CardEvents.TooltipHideRequested += HideTooltip;
    }

    public override void _ExitTree()
    {
        CardEvents.CardTooltipRequested -= ShowTooltip;
        CardEvents.TooltipHideRequested -= HideTooltip;
        _cts.Cancel();
    }

    private void ShowTooltip(Texture2D icon, string text)
    {
        _isVisible = true;
        CancelAndResetRunningAnimation();

        TooltipIcon.Texture = icon;
        TooltipTextLabel.Text = text;

        PlayShowAnimationAsync(_cts.Token).Fire();
    }

    private void HideTooltip()
    {
        _isVisible = false;
        CancelAndResetRunningAnimation();

        PlayHideAnimationAsync(_cts.Token).Fire();
    }

    private async Task PlayShowAnimationAsync(CancellationToken cancellationToken)
    {
        Show();
        await this.TweenModulate(Colors.White, FadeSeconds)
            .SetEasing(Easing.OutCubic)
            .PlayAsync(cancellationToken);
    }
    
    private async Task PlayHideAnimationAsync(CancellationToken cancellationToken)
    {
        await SnekUtility.DelayGd(FadeSeconds, cancellationToken);
        if (_isVisible)
            return;
        
        await this.TweenModulate(Colors.Transparent, FadeSeconds)
            .SetEasing(Easing.OutCubic)
            .PlayAsync(cancellationToken);
        Hide();
    }

    private void CancelAndResetRunningAnimation()
    {
        _cts.Cancel();
        _cts.Dispose();
        _cts = new CancellationTokenSource();
    }
}