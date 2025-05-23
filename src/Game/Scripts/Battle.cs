using CardGameV1.Autoload;
using CardGameV1.Character;
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
    private AudioStream music = null!;

    [Export]
    private CharacterStats characterStats = null!;

    [Node]
    private BattleUI battleUI = null!;

    [Node]
    private Player player = null!;

    [Node]
    private PlayerHandler playerHandler = null!;

    [Node]
    private EnemyHandler enemyHandler = null!;

    private static readonly PlayerEventBus PlayerEvents = EventBusOwner.PlayerEvents;
    private static readonly EnemyEventBus EnemyEvents = EventBusOwner.EnemyEvents;
    private static readonly BattleEventBus BattleEvents = EventBusOwner.BattleEvents;

    public override void _Ready()
    {
        /*
         * normally we do this on every Run, to keep our
         * health, gold and deck between battles
         */
        var newStats = characterStats.CreateInstance();
        battleUI.CharacterStats = newStats;
        player.CharacterStats = newStats;


        StartBattle(newStats);
    }

    public override void _EnterTree()
    {
        enemyHandler.ChildOrderChanged += OnEnemiesChildOrderChanged;

        EnemyEvents.EnemyTurnEnded += OnEnemyTurnEnded;

        PlayerEvents.PlayerTurnEnded += playerHandler.EndTurn;
        PlayerEvents.PlayerHandDiscarded += enemyHandler.StartTurn;
        PlayerEvents.PlayerDied += OnPlayerDied;
    }

    public override void _ExitTree()
    {
        enemyHandler.ChildOrderChanged -= OnEnemiesChildOrderChanged;

        EnemyEvents.EnemyTurnEnded -= OnEnemyTurnEnded;

        PlayerEvents.PlayerTurnEnded -= playerHandler.EndTurn;
        PlayerEvents.PlayerHandDiscarded -= enemyHandler.StartTurn;
        PlayerEvents.PlayerDied -= OnPlayerDied;
    }

    private void StartBattle(CharacterStats stats)
    {
        GetTree().Paused = false;
        SoundManager.MusicPlayer.Play(music, true);
        enemyHandler.ResetEnemyActions();
        playerHandler.StartBattle(stats);
    }

    private void OnEnemiesChildOrderChanged()
    {
        if (enemyHandler.GetChildCount() == 0)
        {
            BattleEvents.EmitBattleOverScreenRequested("Victorious!", BattleOverPanel.PanelType.Win);
        }
    }

    private void OnPlayerDied()
    {
        BattleEvents.EmitBattleOverScreenRequested("Game Over!", BattleOverPanel.PanelType.Lose);
    }

    private void OnEnemyTurnEnded()
    {
        playerHandler.StartTurn();
        enemyHandler.ResetEnemyActions();
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}