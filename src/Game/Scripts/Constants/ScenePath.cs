using System;
using System.Collections.Generic;
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

    public const string Shop = "res://Scenes/UI/shop/Shop.tscn";
    public const string TreasureRoom = "res://Scenes/UI/treasure_room/TreasureRoom.tscn";
}

public static class SceneFactory
{
    private static readonly Dictionary<Type, string> Paths = [];

    public static void Register<T>(string path) => Paths[typeof(T)] = path;

    public static T Instantiate<T>() where T : Node =>
        ResourceLoader.Load<PackedScene>(Paths[typeof(T)]).Instantiate<T>();
}