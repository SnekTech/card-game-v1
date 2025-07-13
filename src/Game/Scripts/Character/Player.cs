using CardGameV1.CustomResources;
using CardGameV1.EffectSystem;
using CardGameV1.EventBus;
using CardGameV1.ModifierSystem;
using CardGameV1.StatusSystem;

namespace CardGameV1.Character;

[SceneTree]
public partial class Player : Node2D, ITarget
{
    private static readonly Material WhiteSprite = GD.Load<Material>("res://art/white_sprite_material.tres");
    private CharacterStats _stats = null!;
    private readonly CancellationTokenSource ctsOnQueueFree = new();

    public StatusHandler StatusHandler { get; private set; } = null!;
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

    public override void _Ready()
    {
        StatusHandler = new StatusHandler(this, _.StatusContainer);
    }

    public override void _ExitTree()
    {
        _stats.StatsChanged -= UpdateCharacterStats;
        StatusHandler.OnDispose();
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

        _.Sprite2D.Texture = CharacterStats.Art;
        UpdateCharacterStats();
    }

    private void UpdateCharacterStats() => _.StatsUI.UpdateStats(CharacterStats);

    public async Task TakeDamageAsync(int damage, ModifierType whichModifier)
    {
        _.Sprite2D.Material = WhiteSprite;
        var modifiedDamage = ModifierHandler.GetModifiedValue(damage, whichModifier);
        CharacterStats.TakeDamage(modifiedDamage);
        await this.ShakeAsync(16, 0.15f);
        _.Sprite2D.Material = null;

        if (Stats.Health <= 0)
        {
            QFree();
            EventBusOwner.PlayerEvents.EmitPlayerDied();
        }
    }
}