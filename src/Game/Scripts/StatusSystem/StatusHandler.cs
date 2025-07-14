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

    private readonly List<Status> _statusList = [];

    public void AddStatus(Status status)
    {
        var sameStatus = GetStatus(status.Id);
        if (sameStatus is null)
        {
            _statusList.Add(status);
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
        foreach (var status in _statusList.Where(s => s.Type == type).ToList())
        {
            await status.ApplyStatusAsync(_target, cancellationToken);
            status.Consume();

            if (status.IsExpired)
            {
                _statusUIContainer.RemoveStatusUI(status.Id);
                _statusList.Remove(status);
            }

            await SnekUtility.DelayGd(StatusApplyInterval, cancellationToken);
        }
    }

    public void OnDispose()
    {
        _statusUIContainer.Clicked -= OnStatusUIContainerClicked;
    }

    private Status? GetStatus(string statusId) => _statusList.FirstOrDefault(status => status.Id == statusId);

    private void OnStatusUIContainerClicked() => EventBusOwner.Events.EmitStatusTooltipRequested(_statusList);
}