using CardGameV1.CardVisual;
using CardGameV1.Character;
using CardGameV1.Map;
using CardGameV1.StatusSystem.UI;
using CardGameV1.UI.BattleReward;
using CardGameV1.UI.Campfire;
using CardGameV1.UI.CardPileDisplay;
using CardGameV1.UI.Shop;
using CardGameV1.UI.TreasureRoom;

namespace CardGameV1;

public static class SceneFactory
{
    static SceneFactory()
    {
        Register<CardMenuUI>("res://Scenes/UI/card_pile_view/CardMenuUI.tscn");
        Register<CardUI>(CardUI.TscnFilePath);
        Register<Battle>("res://Scenes/Battle.tscn");
        Register<Enemy>(Enemy.TscnFilePath);
        Register<BattleRewardScene>(BattleRewardScene.TscnFilePath);
        Register<CardRewards>(CardRewards.TscnFilePath);
        Register<RewardButton>(RewardButton.TscnFilePath);
        Register<CampfireScene>("res://Scenes/UI/campfire/Campfire.tscn");
        Register<ShopScene>("res://Scenes/UI/shop/Shop.tscn");
        Register<TreasureRoomScene>("res://Scenes/UI/treasure_room/TreasureRoom.tscn");
        Register<MapRoomScene>("res://Scenes/map/MapRoom.tscn");
        Register<MapLine>("res://Scenes/map/MapLine.tscn");
        Register<StatusUI>(StatusUI.TscnFilePath);
        // todo: use source generator
        // add an attribute to the scene scripts to be registered here
        Register<StatusTooltip>(StatusTooltip.TscnFilePath);
    }
    
    private static readonly Dictionary<Type, string> Paths = [];

    private static void Register<T>(string path) => Paths[typeof(T)] = path;

    public static T Instantiate<T>() where T : Node =>
        ResourceLoader.Load<PackedScene>(Paths[typeof(T)]).Instantiate<T>();
}