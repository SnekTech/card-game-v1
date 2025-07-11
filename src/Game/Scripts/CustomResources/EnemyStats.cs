using CardGameV1.Character;
using CardGameV1.EnemyAI;

namespace CardGameV1.CustomResources;

public class EnemyStats : Stats
{
    public required Func<Enemy, EnemyActionPicker> ActionPickerGetter { get; init; }

    public EnemyStats Duplicate()
    {
        var other = new EnemyStats
        {
            MaxHealth = MaxHealth,
            Health = MaxHealth,
            Block = Block,
            ArtPath = ArtPath,
            ActionPickerGetter = ActionPickerGetter,
        };
        return other;
    }
}