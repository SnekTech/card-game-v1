using CardGameV1.CustomResources;
using CardGameV1.CustomResources.Cards;

namespace CardGameV1.Character;

public static class CharacterPool
{
    public static readonly CharacterStats Warrior = new(CardPool.WarriorStartingDeck)
    {
        MaxHealth = 35,
        CharacterName = "Warrior",
        Description = "Likes to slice things.",
        PortraitPath = "res://art/tile_0087.png",
        ArtPath = "res://art/tile_0087.png",
        DraftableCards = CardPool.WarriorDraftableCards,
    };

    public static readonly CharacterStats Wizard = new(CardPool.WarriorStartingDeck)
    {
        MaxHealth = 27,
        CharacterName = "Wizard",
        Description = "Years of wisdom and\nrecurring back pain.",
        PortraitPath = "res://art/tile_0084.png",
        ArtPath = "res://art/tile_0084.png",
        DraftableCards = CardPool.WarriorDraftableCards,
    };

    public static readonly CharacterStats Assassin = new(CardPool.WarriorStartingDeck)
    {
        MaxHealth = 25,
        CharacterName = "Assassin",
        Description = "This dagger is my favorite.",
        PortraitPath = "res://art/tile_0088.png",
        ArtPath = "res://art/tile_0088.png",
        DraftableCards = CardPool.WarriorDraftableCards,
    };
}