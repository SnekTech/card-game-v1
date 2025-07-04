using System.Threading;
using System.Threading.Tasks;
using CardGameV1.CustomResources;
using CardGameV1.EffectSystem;
using CardGameV1.EventBus;
using CardGameV1.ModifierSystem;
using CardGameV1.StatusSystem;
using CardGameV1.UI.BattleUIComponents;
using GodotUtilities;

namespace CardGameV1.Character;

[Scene]
public partial class Player : Node2D, ITarget
{
    [Node]
    private Sprite2D sprite2D = null!;
    [Node]
    private StatsUI statsUI = null!;
    [Node]
    private StatusHandler statusHandler = null!;


    private static readonly Material WhiteSprite = GD.Load<Material>("res://art/white_sprite_material.tres");
    private CharacterStats _stats = null!;
    private readonly CancellationTokenSource ctsOnQueueFree = new();

    public StatusHandler StatusHandler => statusHandler;
    public ModifierHandler ModifierHandler { get; } = ModifierFactory.CreatePlayerModifierHandler();

    public CharacterStats CharacterStats
    {
        get => _stats;
        set
        {
            _stats = value;
            _stats.StatsChanged += UpdateCharacterStats;
            UpdatePlayer();
        }
    }

    public Stats Stats => _stats;

    public CancellationToken CancellationTokenOnQueueFree => ctsOnQueueFree.Token;

    public override void _ExitTree()
    {
        _stats.StatsChanged -= UpdateCharacterStats;
    }

    public void QFree()
    {
        ctsOnQueueFree.Cancel();
        QueueFree();
    }

    private void UpdatePlayer()
    {
        if (IsInsideTree() == false)
            return;

        sprite2D.Texture = CharacterStats.Art;
        UpdateCharacterStats();
    }

    private void UpdateCharacterStats() => statsUI.UpdateStats(CharacterStats);

    public async Task TakeDamageAsync(int damage, ModifierType whichModifier)
    {
        sprite2D.Material = WhiteSprite;
        var modifiedDamage = ModifierHandler.GetModifiedValue(damage, whichModifier);
        CharacterStats.TakeDamage(modifiedDamage);
        await this.ShakeAsync(16, 0.15f);
        sprite2D.Material = null;

        if (Stats.Health <= 0)
        {
            QFree();
            EventBusOwner.PlayerEvents.EmitPlayerDied();
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