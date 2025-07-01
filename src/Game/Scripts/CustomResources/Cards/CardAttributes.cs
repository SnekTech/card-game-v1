namespace CardGameV1.CustomResources.Cards;

public record CardAttributes
{
    public required string Id { get; init; }
    public required int Cost { get; init; }
    public required CardType Type { get; init; }
    public required CardRarity Rarity { get; init; }
    public required CardTarget Target { get; init; }
    public bool ShouldExhaust { get; set; }
    public string TooltipText { get; init; } = "default tooltip";
    public required string IconPath { get; init; }
    public required string SoundPath { get; init; }
}