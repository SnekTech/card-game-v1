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

    private static readonly PlayerEventBus PlayerEventBus = EventBusOwner.PlayerEventBus;
    private static readonly EnemyEventBus EnemyEventBus = EventBusOwner.EnemyEventBus;

    public override void _Ready()
    {
        /*
         * normally we do this on every Run, to keep our
         * health, gold and deck between battles
         */
        var newStats = characterStats.CreateInstance();
        battleUI.CharacterStats = newStats;
        player.CharacterStats = newStats;

        enemyHandler.ChildOrderChanged += OnEnemiesChildOrderChanged;

        EnemyEventBus.EnemyTurnEnded += OnEnemyTurnEnded;

        PlayerEventBus.PlayerTurnEnded += playerHandler.EndTurn;
        PlayerEventBus.PlayerHandDiscarded += enemyHandler.StartTurn;
        PlayerEventBus.PlayerDied += OnPlayerDied;

        StartBattle(newStats);
    }

    private void StartBattle(CharacterStats stats)
    {
        SoundManager.MusicPlayer.Play(music, true);
        enemyHandler.ResetEnemyActions();
        playerHandler.StartBattle(stats);
    }

    private void OnEnemiesChildOrderChanged()
    {
        if (enemyHandler.GetChildCount() == 0)
        {
            GD.Print("Victory!");
        }
    }

    private void OnPlayerDied()
    {
        GD.Print("Game Over!");
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