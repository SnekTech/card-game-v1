using CardGameV1.EffectSystem;
using CardGameV1.EventBus;
using CardGameV1.StatusSystem.UI;

namespace CardGameV1.StatusSystem;

public class StatusHandler
{
    public StatusHandler(ITarget target, StatusContainer statusUIContainer)
    {
        _target = target;
        _statusUIContainer = statusUIContainer;
        _statusUIContainer.Clicked += OnStatusUIContainerClicked;
    }

    private const float StatusApplyInterval = 0.25f;

    private readonly ITarget _target;
    private readonly StatusContainer _statusUIContainer;

    private readonly Dictionary<string, Status> _statusById = new();

    public void AddStatus(Status status)
    {
        _statusById.TryGetValue(status.Id, out var sameStatus);
        if (sameStatus is null)
        {
            _statusById.Add(status.Id, status);
            status.Init(_target);
            _statusUIContainer.AddStatusUI(status);
            return;
        }

        sameStatus.StackUp(status);
    }

    public async Task ApplyStatusesByType(StatusType type, CancellationToken cancellationToken)
    {
        if (type == StatusType.EventBased)
            return;

        cancellationToken.ThrowIfCancellationRequested();
        foreach (var status in _statusById.Values.Where(s => s.Type == type).ToList())
        {
            await status.ApplyStatusAsync(_target, cancellationToken);
            status.Consume();

            if (status.IsExpired)
            {
                _statusUIContainer.RemoveStatusUI(status.Id);
                _statusById.Remove(status.Id);
            }

            await SnekUtility.DelayGd(StatusApplyInterval, cancellationToken);
        }
    }

    public void OnDispose()
    {
        _statusUIContainer.Clicked -= OnStatusUIContainerClicked;
    }

    private void OnStatusUIContainerClicked() => EventBusOwner.Events.EmitStatusTooltipRequested(_statusById.Values);
}