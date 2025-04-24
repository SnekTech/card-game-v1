using System;
using Godot;

namespace CardGameV1.CustomResources;

[GlobalClass]
public partial class Stats : Resource
{
    public event Action? StatsChanged;

    [Export] private int maxHealth = 1;
    [Export] private Texture2D art = null!;

    private int _health;
    private int _block;

    public int Health
    {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, 0, maxHealth);
            EmitStatsChanged();
        }
    }

    public int Block
    {
        get => _block;
        set
        {
            _block = Mathf.Clamp(value, 0, 999);
            EmitStatsChanged();
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
            return;

        var initialDamage = damage;
        damage = Mathf.Clamp(damage - Block, 0, damage);
        Block = Mathf.Clamp(Block - initialDamage, 0, Block);
        Health -= damage;
    }

    public void Heal(int amount)
    {
        Health += amount;
    }

    public virtual Stats CreateInstance()
    {
        var instance = (Stats)Duplicate();
        instance.Health = maxHealth;
        instance.Block = 0;
        return instance;
    }

    protected void EmitStatsChanged() => StatsChanged?.Invoke();
}