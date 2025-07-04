namespace CardGameV1.EnemyAI;

public record Intent(string BaseText, string IconPath)
{
    public string CurrentText { get; set; } = BaseText;
    public Texture2D Icon => SnekUtility.LoadTexture(IconPath);
}