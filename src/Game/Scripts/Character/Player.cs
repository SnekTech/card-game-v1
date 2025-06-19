using System.Threading.Tasks;
using CardGameV1.CustomResources;
using CardGameV1.EffectSystem;
using CardGameV1.StatusSystem;
using CardGameV1.UI.BattleUIComponents;
using Godot;
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

    public StatusHandler StatusHandler => statusHandler;

    private static readonly Material WhiteSprite = GD.Load<Material>("res://art/white_sprite_material.tres");
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

    public async Task TakeDamageAsync(int damage)
    {
        sprite2D.Material = WhiteSprite;
        CharacterStats.TakeDamage(damage);
        await this.ShakeAsync(16, 0.15f);
        sprite2D.Material = null;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}