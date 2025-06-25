using System;
using Godot;

namespace CardGameV1.CustomResources;

public abstract class Stats
{
    public event Action? StatsChanged;

    public required string ArtPath { get; init; }
    public int MaxHealth { get; set; }

    public Texture2D Art => SnekUtility.LoadTexture(ArtPath);

    private const int MaxBlock = 999;

    private int _health;
    private int _block;

    public int Health
    {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, 0, MaxHealth);
            EmitStatsChanged();
        }
    }

    public int Block
    {
        get => _block;
        set
        {
            _block = Mathf.Clamp(value, 0, MaxBlock);
            EmitStatsChanged();
        }
    }

    public virtual void TakeDamage(int damage)
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

    protected void EmitStatsChanged() => StatsChanged?.Invoke();
}