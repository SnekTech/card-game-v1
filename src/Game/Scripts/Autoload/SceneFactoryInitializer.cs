using CardGameV1.Map;
using CardGameV1.StatusSystem.UI;
using CardGameV1.UI.BattleReward;
using CardGameV1.UI.Campfire;
using CardGameV1.UI.CardPileDisplay;
using CardGameV1.UI.Shop;
using CardGameV1.UI.TreasureRoom;
using Godot;

namespace CardGameV1.Autoload;

public partial class SceneFactoryInitializer : Node
{
    public override void _Ready()
    {
        SceneFactory.Register<CardMenuUI>("res://Scenes/UI/card_pile_view/CardMenuUI.tscn");
        SceneFactory.Register<Battle>("res://Scenes/Battle.tscn");
        SceneFactory.Register<BattleRewardScene>("res://Scenes/UI/battle_reward/BattleReward.tscn");
        SceneFactory.Register<CardRewards>("res://Scenes/UI/battle_reward/CardRewards.tscn");
        SceneFactory.Register<RewardButton>("res://Scenes/UI/battle_reward/RewardButton.tscn");
        SceneFactory.Register<CampfireScene>("res://Scenes/UI/campfire/Campfire.tscn");
        SceneFactory.Register<ShopScene>("res://Scenes/UI/shop/Shop.tscn");
        SceneFactory.Register<TreasureRoomScene>("res://Scenes/UI/treasure_room/TreasureRoom.tscn");
        SceneFactory.Register<MapRoomScene>("res://Scenes/map/MapRoom.tscn");
        SceneFactory.Register<MapLine>("res://Scenes/map/MapLine.tscn");
        SceneFactory.Register<StatusUI>("res://Scenes/statusHandler/StatusUI.tscn");
    }
}
