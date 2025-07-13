using CardGameV1.EffectSystem;
using CardGameV1.EventBus;
using CardGameV1.StatusSystem.UI;

namespace CardGameV1.StatusSystem;

public class StatusHandler
{
    public StatusHandler(ITarget target, StatusContainer container)
    {
        _target = target;
        _container = container;
        _container.Clicked += OnContainerClicked;
    }

    private const float StatusApplyInterval = 0.25f;

    private readonly ITarget _target;
    private readonly StatusContainer _container;

    private readonly List<Status> _statusList = [];

    public void AddStatus(Status status)
    {
        var sameStatus = GetStatus(status.Id);
        if (sameStatus == null)
        {
            _statusList.Add(status);
            var newStatusUI = SceneFactory.Instantiate<StatusUI>();
            _container.AddChild(newStatusUI);
            newStatusUI.Status = status;
            newStatusUI.Status.Init(_target);
            return;
        }

        switch (status)
        {
            case { CanExpire: false, IsStackable: false }:
                return;
            case { CanExpire: true, StackType: StackType.Duration }:
                sameStatus.Duration += status.Duration;
                return;
            case { StackType: StackType.Intensity }:
                sameStatus.Stacks += status.Stacks;
                break;
        }
    }

    public async Task ApplyStatusesByType(StatusType type, CancellationToken cancellationToken)
    {
        if (type == StatusType.EventBased)
            return;

        cancellationToken.ThrowIfCancellationRequested();
        foreach (var status in _statusList.Where(s => s.Type == type).ToList())
        {
            await status.ApplyStatusAsync(_target, cancellationToken);
            if (status.StackType == StackType.Duration)
            {
                status.Duration--;
            }

            if (status.IsExpired())
            {
                _container.RemoveStatusUI(status.Id);
                _statusList.Remove(status);
            }

            await SnekUtility.DelayGd(StatusApplyInterval, cancellationToken);
        }
    }

    public void OnDispose()
    {
        _container.Clicked -= OnContainerClicked;
    }

    private Status? GetStatus(string statusId) => _statusList.FirstOrDefault(status => status.Id == statusId);

    private void OnContainerClicked() => EventBusOwner.Events.EmitStatusTooltipRequested(_statusList);
}