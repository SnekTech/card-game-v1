using System;
using System.Collections.Generic;
using CardGameV1.Map;
using Godot;

namespace CardGameV1.Constants;

public static class ScenePath
{
    public const string CardMenuUI = "res://Scenes/UI/card_pile_view/CardMenuUI.tscn";

    public const string Battle = "res://Scenes/Battle.tscn";
    public const string BattleReward = "res://Scenes/UI/battle_reward/BattleReward.tscn";
    public const string CardRewards = "res://Scenes/UI/battle_reward/CardRewards.tscn";
    public const string RewardButton = "res://Scenes/UI/battle_reward/RewardButton.tscn";
    public const string Campfire = "res://Scenes/UI/campfire/Campfire.tscn";

    public const string Map = "res://Scenes/map/Map.tscn";

    public const string Shop = "res://Scenes/UI/shop/Shop.tscn";
    public const string TreasureRoom = "res://Scenes/UI/treasure_room/TreasureRoom.tscn";
}

public static class SceneFactory
{
    private static readonly Dictionary<Type, string> Paths = new()
    {
        [typeof(MapRoomScene)] = "res://Scenes/map/MapRoom.tscn",
        [typeof(MapLine)] = "res://Scenes/map/MapLine.tscn",
    };

    public static T Instantiate<T>() where T : Node
    {
        return ResourceLoader.Load<PackedScene>(Paths[typeof(T)]).Instantiate<T>();
    }
}