using CardGameV1.CustomResources;
using Godot;
using GodotUtilities;

namespace CardGameV1.UI;

[Scene]
public partial class Enemy : Area2D
{
    private const int ArrowOffset = 5;

    [Export]
    private Stats originalEnemyStats = null!;

    [Node]
    private Sprite2D sprite2D = null!;

    [Node]
    private Sprite2D arrow = null!;

    [Node]
    private StatsUI statsUI = null!;

    private bool _hasSubscribedStatsChanged;
    private Stats _stats = null!;

    public Stats Stats
    {
        get => _stats;
        set
        {
            _stats = value.CreateInstance();
            SubscribeStatsChanged();
            UpdateEnemy();
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
        Stats = originalEnemyStats;
    }

    private void SubscribeStatsChanged()
    {
        if (_hasSubscribedStatsChanged == false)
        {
            Stats.StatsChanged += UpdateStats;
        }

        _hasSubscribedStatsChanged = true;
    }

    private void UpdateEnemy()
    {
        if (IsInsideTree() == false)
            return;

        sprite2D.Texture = Stats.Art;
        var spriteWidth = sprite2D.GetRect().Size.X;
        arrow.Position = Vector2.Right * (spriteWidth / 2 + ArrowOffset);
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