using System;

namespace CardGameV1.EventBus;

public class Events
{
    #region battle

    public event Action? BattleWon;
    public void EmitBattleWon() => BattleWon?.Invoke();

    #endregion

    #region map

    public event Action? MapExited;

    public void EmitMapExited() => MapExited?.Invoke();

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