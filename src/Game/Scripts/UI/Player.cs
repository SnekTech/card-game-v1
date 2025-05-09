using CardGameV1.CustomResources;
using Godot;
using GodotUtilities;

namespace CardGameV1.UI;

[Scene]
public partial class Player : Node2D
{
    [Export]
    private CharacterStats originalPlayerStats = null!;

    [Node]
    private Sprite2D sprite2D = null!;

    [Node]
    private StatsUI statsUI = null!;

    private bool _hasSubscribedStatsChanged;
    private CharacterStats _stats = null!;

    public CharacterStats Stats
    {
        get => _stats;
        set
        {
            _stats = value.CreateInstance();
            SubscribeStatsChanged();
            UpdatePlayer();
        }
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }

    public override void _Ready()
    {
        Stats = originalPlayerStats;
    }

    private void SubscribeStatsChanged()
    {
        if (_hasSubscribedStatsChanged == false)
        {
            Stats.StatsChanged += UpdateStats;
        }

        _hasSubscribedStatsChanged = true;
    }

    private void UpdatePlayer()
    {
        if (IsInsideTree() == false)
            return;

        sprite2D.Texture = Stats.Art;
        UpdateStats();
    }

    private void UpdateStats() => statsUI.UpdateStats(Stats);

    public void TakeDamage(int damage)
    {
        if (Stats.Health <= 0)
            return;
        
        Stats.TakeDamage(damage);

        if (Stats.Health <= 0)
        {
            QueueFree();
        }
    }
}