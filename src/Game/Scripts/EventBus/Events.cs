using CardGameV1.Map;
using CardGameV1.StatusSystem;

namespace CardGameV1.EventBus;

public class Events
{
    // todo: use source generator to generate EmitXXX functions
    #region battle

    public event Action? BattleWon;
    public event Action? BattleLost;
    public event Action<IEnumerable<Status>>? StatusTooltipRequested;
    public void EmitBattleWon() => BattleWon?.Invoke();
    public void EmitBattleLost() => BattleLost?.Invoke();
    public void EmitStatusTooltipRequested(IEnumerable<Status> statuses) => StatusTooltipRequested?.Invoke(statuses);

    #endregion

    #region map

    public event Action<Room>? MapExited;

    public void EmitMapExited(Room room) => MapExited?.Invoke(room);

    #endregion

    #region shop

    public event Action? ShopExited;

    public void EmitShopExited() => ShopExited?.Invoke();

    #endregion

    #region campfire

    public event Action? CampfireExited;

    public void EmitCampfireExited() => CampfireExited?.Invoke();

    #endregion

    #region battle reward

    public event Action? BattleRewardExited;

    public void EmitBattleRewardExited() => BattleRewardExited?.Invoke();

    #endregion

    #region treasure room

    public event Action? TreasureRoomExited;

    public void EmitTreasureRoomExited() => TreasureRoomExited?.Invoke();

    #endregion
}