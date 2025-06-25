using CardGameV1.CustomResources;
using CardGameV1.EnemyAI;

namespace CardGameV1.Character;

public static class EnemyPool
{
    public static readonly EnemyStats Crab = new()
    {
        MaxHealth = 10,
        ActionPickerGetter = EnemyActionPickerFactory.CreateCrabBrain,
        ArtPath = "res://art/tile_0110.png",
    };
    
    public static readonly EnemyStats Bat = new()
    {
        MaxHealth = 8,
        ActionPickerGetter = EnemyActionPickerFactory.CreateBatBrain,
        ArtPath = "res://art/tile_0120.png",
    };
}