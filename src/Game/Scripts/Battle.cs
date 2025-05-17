using CardGameV1.CustomResources;
using CardGameV1.EventBus;
using CardGameV1.TurnManagement;
using CardGameV1.UI;
using Godot;
using GodotUtilities;

namespace CardGameV1;

[Scene]
public partial class Battle : Node2D
{
    [Export]
    private CharacterStats characterStats = null!;
    
    [Node]
    private BattleUI battleUI = null!;

    [Node]
    private PlayerHandler playerHandler = null!;

    private readonly PlayerEventBus playerEventBus = EventBusOwner.PlayerEventBus;

    public override void _Ready()
    {
        /*
         * normally we do this on every Run, to keep our
         * health, gold and deck between battles
         */
        var newStats = characterStats.CreateInstance();
        battleUI.CharacterStats = newStats;

        playerEventBus.PlayerTurnEnded += playerHandler.EndTurn;
        playerEventBus.PlayerHandDiscarded += playerHandler.StartTurn; // temporary
        
        StartBattle(newStats);
    }

    private void StartBattle(CharacterStats stats)
    {
        playerHandler.StartBattle(stats);
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}