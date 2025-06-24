using Godot;

namespace CardGameV1.EnemyAI;

public record Intent(string Number, string IconPath)
{
    public Texture2D Icon => SnekUtility.LoadTexture(IconPath);
}