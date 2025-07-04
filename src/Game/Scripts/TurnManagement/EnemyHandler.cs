using CardGameV1.Character;
using CardGameV1.CustomResources;
using CardGameV1.EventBus;
using GodotUtilities;

namespace CardGameV1.TurnManagement;

[Scene]
public partial class EnemyHandler : Node2D
{
    [Node]
    private Node2D enemySlots = null!;

    private static readonly EnemyEvents EnemyEvents = EventBusOwner.EnemyEvents;

    private IEnumerable<Enemy> Enemies => this.GetChildrenOfType<Enemy>();

    public bool HasNoEnemyAlive => !Enemies.Any();

    public override void _EnterTree()
    {
        EventBusOwner.PlayerEvents.PlayerHandDrawn += OnPlayerHandDrawn;
    }

    public override void _ExitTree()
    {
        EventBusOwner.PlayerEvents.PlayerHandDrawn -= OnPlayerHandDrawn;
    }

    public void ResetEnemyActions()
    {
        foreach (var enemy in Enemies)
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
        ClearExistingEnemies();

        var slots = enemySlots.GetChildrenOfType<Marker2D>().ToList();
        for (var i = 0; i < slots.Count; i++)
        {
            if (i >= battleStats.Enemies.Count)
            {
                GD.Print(
                    $"{slots.Count} enemy slots available, but only got {battleStats.Enemies.Count} enemies for this battle");
                return;
            }

            var enemy = SceneFactory.Instantiate<Enemy>();
            AddChild(enemy);
            enemy.EnemyStats = battleStats.Enemies[i].Duplicate();
            enemy.GlobalPosition = slots[i].GlobalPosition;
        }

        return;

        void ClearExistingEnemies()
        {
            foreach (var enemy in Enemies)
            {
                enemy.QueueFree();
            }
        }
    }

    private async Task EnemiesDoTurnAsync()
    {
        foreach (var enemy in Enemies)
        {
            await enemy.DoTurnAsync();
        }

        EnemyEvents.EmitEnemyTurnEnded();
    }

    private void OnPlayerHandDrawn()
    {
        foreach (var enemy in Enemies)
        {
            enemy.UpdateIntent();
        }
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}