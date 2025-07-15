using CardGameV1.CustomResources.Cards.Warrior;

namespace CardGameV1.CustomResources.Cards;

public static class CardPool
{
    private static readonly Dictionary<Type, Card> Cards = [];

    public static T Get<T>() where T : Card
    {
        if (!Cards.ContainsKey(typeof(T)))
        {
            Cards[typeof(T)] = CardFactory.Create<T>();
        }

        return (T)Cards[typeof(T)];
    }

    public static readonly CardPile WarriorStartingDeck = new([
        Get<WarriorBlock>(),
        Get<WarriorBlock>(),
        Get<WarriorBlock>(),
        Get<WarriorBigSlam>(),
        Get<WarriorBigSlam>(),
        Get<WarriorSlash>(),
        Get<WarriorTrueStrength>(),
    ]);

    public static readonly CardPile WarriorDraftableCards = new([
        Get<WarriorBlock>(),
        Get<WarriorBlock>(),
        Get<WarriorSlash>(),
        Get<WarriorBigSlam>(),
        Get<WarriorBigSlam>(),
        Get<WarriorBigSlam>(),
        Get<WarriorTrueStrength>(),
        Get<WarriorTrueStrength>(),
        Get<WarriorTrueStrength>(),
    ]);
}