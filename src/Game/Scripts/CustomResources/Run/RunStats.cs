namespace CardGameV1.CustomResources.Run;

public class RunStats
{
    public event Action? GoldChanged;

    private const int StartingGold = 70;
    private const int BaseCardRewards = 3;

    private int _gold = StartingGold;

    public CardRarityWeightStats CardRarityWeightStats { get; } = new();

    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            GoldChanged?.Invoke();
        }
    }

    public int CardRewards => BaseCardRewards;
}