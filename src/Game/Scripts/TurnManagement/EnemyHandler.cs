using System.Threading.Tasks;
using CardGameV1.Character;
using CardGameV1.EventBus;
using Godot;
using GodotUtilities;

namespace CardGameV1.TurnManagement;

public partial class EnemyHandler : Node2D
{
    private static readonly EnemyEvents EnemyEvents = EventBusOwner.EnemyEvents;

    public void ResetEnemyActions()
    {
        foreach (var child in GetChildren())
        {
            if (child is not Enemy enemy)
                continue;

            enemy.CurrentAction = null;
            enemy.UpdateAction();
        }
    }

    public void StartTurn()
    {
        EnemiesDoTurnAsync().Fire();
    }

    private async Task EnemiesDoTurnAsync()
    {
        foreach (var enemy in this.GetChildrenOfType<Enemy>())
        {
            await enemy.DoTurnAsync();
        }

        EnemyEvents.EmitEnemyTurnEnded();
    }
}