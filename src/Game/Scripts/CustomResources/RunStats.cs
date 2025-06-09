using System;
using Godot;

namespace CardGameV1.CustomResources;

[GlobalClass]
public partial class RunStats : Resource
{
    public event Action? GoldChanged;

    private const int StartingGold = 70;

    private int _gold = StartingGold;

    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            GoldChanged?.Invoke();
        }
    }
}