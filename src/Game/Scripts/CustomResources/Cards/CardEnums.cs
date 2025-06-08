namespace CardGameV1.CustomResources.Cards;

public enum CardType
{
    Attack,
    Skill,
    Power
}

public enum CardRarity
{
    Common,
    Uncommon,
    Rare
}

public enum CardTarget
{
    Self,
    SingleEnemy,
    AllEnemies,
    Everyone
}