using CardGameV1.Character;
using CardGameV1.EventBus;
using Godot;
using GodotUtilities;

namespace CardGameV1.TurnManagement;

[Scene]
public partial class EnemyHandler : Node2D
{
    private static readonly EnemyEventBus EventBus = EventBusOwner.EnemyEventBus;

    public override void _Ready()
    {
        EventBus.EnemyActionCompleted += OnEnemyActionCompleted;
    }

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
        if (GetChildCount() == 0)
            return;

        var firstEnemy = GetChild<Enemy>(0);
        firstEnemy.DoTurn();
    }

    private void OnEnemyActionCompleted(Enemy enemy)
    {
        var enemyIndex = enemy.GetIndex();
        var isLastEnemy = enemyIndex == GetChildCount() - 1;
        if (isLastEnemy)
        {
            EventBus.EmitEnemyTurnEnded();
            return;
        }

        var nextEnemy = GetChild<Enemy>(enemyIndex + 1);
        nextEnemy.DoTurn();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}