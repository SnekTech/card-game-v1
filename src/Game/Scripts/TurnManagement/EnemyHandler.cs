using System.Threading.Tasks;
using CardGameV1.Character;
using CardGameV1.CustomResources;
using CardGameV1.EventBus;
using CardGameV1.MyExtensions;
using Godot;
using GodotUtilities;

namespace CardGameV1.TurnManagement;

public partial class EnemyHandler : Node2D
{
    private static readonly EnemyEvents EnemyEvents = EventBusOwner.EnemyEvents;

    public void ResetEnemyActions()
    {
        foreach (var enemy in this.GetChildrenOfType<Enemy>())
        {
            enemy.CurrentAction = null;
            enemy.UpdateAction();
        }
    }

    public void StartTurn()
    {
        EnemiesDoTurnAsync().Fire();
    }

    public void SetupEnemies(BattleStats battleStats)
    {
        this.ClearChildren();

        var enemiesPacked = SnekUtility.LoadScene(battleStats.EnemiesScenePath).Instantiate<Node2D>();
        foreach (var enemy in enemiesPacked.GetChildrenOfType<Enemy>())
        {
            enemy.Owner = null;
            enemy.Reparent(this);
        }

        enemiesPacked.QueueFree();
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