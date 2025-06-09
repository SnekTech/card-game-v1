using System;
using Godot;

namespace CardGameV1.CustomResources;

[GlobalClass]
public partial class RunStats : Resource
{
    public event Action? GoldChanged;

    private const int StartingGold = 70;
    private const int BaseCardRewards = 3;
    private const float BaseCommonWeight = 6f;
    private const float BaseUncommonWeight = 3.7f;
    private const float BaseRareWeight = 0.3f;

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

    public int CardRewards { get; set; } = BaseCardRewards;
    public float CommonWeight { get; set; } = BaseCommonWeight;
    public float UncommonWeight { get; set; } = BaseUncommonWeight;
    public float RareWeight { get; set; } = BaseRareWeight;

    public void ResetWeights()
    {
        CommonWeight = BaseCommonWeight;
        UncommonWeight = BaseUncommonWeight;
        RareWeight = BaseRareWeight;
    }
}