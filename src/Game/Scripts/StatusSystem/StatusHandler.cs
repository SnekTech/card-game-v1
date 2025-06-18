using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardGameV1.EffectSystem;
using CardGameV1.StatusSystem.UI;
using Godot;
using GodotUtilities;

namespace CardGameV1.StatusSystem;

public partial class StatusHandler : GridContainer
{
    [Export]
    private Node2D statusOwner = null!;

    private const float StatusApplyInterval = 0.25f;

    private ITarget Target => (ITarget)statusOwner;

    public void AddStatus(Status status)
    {
        var sameStatus = GetStatus(status.Id);
        if (sameStatus == null)
        {
            var newStatusUI = SceneFactory.Instantiate<StatusUI>();
            AddChild(newStatusUI);
            newStatusUI.Status = status;
            newStatusUI.Status.Init(Target);
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

    private Status? GetStatus(string statusId) => GetAllStatuses().FirstOrDefault(status => status.Id == statusId);

    private IEnumerable<Status> GetAllStatuses() =>
        this.GetChildrenOfType<StatusUI>().Select(statusUI => statusUI.Status);

    public async Task ApplyStatusesByType(StatusType type)
    {
        if (type == StatusType.EventBased)
            return;

        foreach (var status in GetAllStatuses().Where(sts => sts.Type == type))
        {
            await status.ApplyStatusAsync(Target);
            await SnekUtility.DelayGd(StatusApplyInterval);
        }
    }
}