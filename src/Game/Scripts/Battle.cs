using CardGameV1.Character;
using CardGameV1.CustomResources;
using CardGameV1.EventBus;
using CardGameV1.TurnManagement;
using CardGameV1.UI.BattleUIComponents;
using GodotUtilities;

namespace CardGameV1;

[Scene]
public partial class Battle : Node2D
{
    [Export]
    private AudioStream music = null!;

    [Node]
    private BattleUI battleUI = null!;
    [Node]
    private Player player = null!;
    [Node]
    private PlayerHandler playerHandler = null!;
    [Node]
    private EnemyHandler enemyHandler = null!;

    private static readonly PlayerEvents PlayerEvents = EventBusOwner.PlayerEvents;
    private static readonly EnemyEvents EnemyEvents = EventBusOwner.EnemyEvents;
    private static readonly BattleEvents BattleEvents = EventBusOwner.BattleEvents;

    private readonly CancellationTokenSource _ctsBattleEnded = new();

    public BattleStats BattleStats { get; set; } = null!;

    public CharacterStats CharacterStats { get; set; } = null!;

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

    public void StartBattle()
    {
        GetTree().Paused = false;
        Autoload.SoundManager.MusicPlayer.Play(music, true);

        battleUI.CharacterStats = CharacterStats;
        player.CharacterStats = CharacterStats;
        enemyHandler.SetupEnemies(BattleStats);

        enemyHandler.ResetEnemyActions();
        playerHandler.StartBattle(CharacterStats);
        battleUI.InitCardPileUI();
    }

    private void OnEnemiesChildOrderChanged()
    {
        if (!enemyHandler.HasNoEnemyAlive) return;

        _ctsBattleEnded.Cancel();
        BattleEvents.EmitBattleOverScreenRequested("Victorious!", BattleOverPanel.PanelType.Win);
    }

    private void OnEnemyTurnEnded()
    {
        if (enemyHandler.HasNoEnemyAlive) return;

        playerHandler.StartTurnAsync(_ctsBattleEnded.Token).Fire();
        enemyHandler.ResetEnemyActions();
    }

    private void OnPlayerDied()
    {
        EventBusOwner.BattleEvents.EmitBattleOverScreenRequested("Game Over!", BattleOverPanel.PanelType.Lose);
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}