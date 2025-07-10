namespace CardGameV1.UI.BattleUIComponents;

[SceneTree]
public partial class RedFlash : CanvasLayer
{
    private const float FlashAlpha = 0.2f;
    private const float FlashDuration = 0.1f;

    public override void _EnterTree()
    {
        EventBus.EventBusOwner.PlayerEvents.PlayerHit += OnPlayerHit;
    }

    public override void _ExitTree()
    {
        EventBus.EventBusOwner.PlayerEvents.PlayerHit -= OnPlayerHit;
    }

    private void OnPlayerHit()
    {
        SetColorRectAlpha(FlashAlpha);
        ResetAfterDelay().Fire();
        return;

        async Task ResetAfterDelay()
        {
            await SnekUtility.DelayGd(FlashDuration);
            SetColorRectAlpha(0);
        }
    }

    private void SetColorRectAlpha(float alpha) => ColorRect.Color = ColorRect.Color with { A = alpha };
}