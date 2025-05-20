using CardGameV1.CustomResources;
using CardGameV1.EffectSystem;
using CardGameV1.UI;
using Godot;
using GodotUtilities;

namespace CardGameV1.Character;

[Scene]
public partial class Player : Node2D, ITarget
{
    [Export]
    private CharacterStats originalPlayerStats = null!;

    [Node]
    private Sprite2D sprite2D = null!;

    [Node]
    private StatsUI statsUI = null!;

    private CharacterStats _stats = null!;

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

    public override void _Ready()
    {
        CharacterStats = originalPlayerStats;
    }

    public override void _ExitTree()
    {
        _stats.StatsChanged -= UpdateCharacterStats;
    }

    private void UpdatePlayer()
    {
        if (IsInsideTree() == false)
            return;

        sprite2D.Texture = CharacterStats.Art;
        UpdateCharacterStats();
    }

    private void UpdateCharacterStats() => statsUI.UpdateStats(CharacterStats);

    public void TakeDamage(int damage)
    {
        if (CharacterStats.Health <= 0)
            return;

        CharacterStats.TakeDamage(damage);

        if (CharacterStats.Health <= 0)
        {
            QueueFree();
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