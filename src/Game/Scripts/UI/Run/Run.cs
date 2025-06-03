using System;
using CardGameV1.CustomResources;
using CardGameV1.EventBus;
using Godot;
using GodotUtilities;

namespace CardGameV1.UI.Run;

[Scene]
public partial class Run : Node
{
    [Export]
    private RunStartup runStartup = null!;
    
    [Node]
    private Node currentView = null!;
    [Node]
    private Button mapButton = null!;
    [Node]
    private Button battleButton = null!;
    [Node]
    private Button shopButton = null!;
    [Node]
    private Button treasureRoomButton = null!;
    [Node]
    private Button battleRewardButton = null!;
    [Node]
    private Button campfireButton = null!;

    #region packed scenes

    private static readonly PackedScene BattleScene = GD.Load<PackedScene>("res://Scenes/Battle.tscn");
    private static readonly PackedScene BattleRewardScene =
        GD.Load<PackedScene>("res://Scenes/UI/battle_reward/BattleReward.tscn");
    private static readonly PackedScene CampfireScene = GD.Load<PackedScene>("res://Scenes/UI/campfire/Campfire.tscn");
    private static readonly PackedScene MapScene = GD.Load<PackedScene>("res://Scenes/UI/map/Map.tscn");
    private static readonly PackedScene ShopScene = GD.Load<PackedScene>("res://Scenes/UI/shop/Shop.tscn");
    private static readonly PackedScene TreasureRoomScene =
        GD.Load<PackedScene>("res://Scenes/UI/treasure_room/TreasureRoom.tscn");

    #endregion

    private CharacterStats Character { get; set; } = null!;

    public override void _Ready()
    {
        switch (runStartup.Type)
        {
            case RunStartup.RunType.NewRun:
                Character = runStartup.PickedCharacter.CreateInstance();
                StartRun();
                break;
            case RunStartup.RunType.ContinuedRun:
                // todo: load last run
                GD.Print("load previous run");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void StartRun()
    {
        SubscribeEvents();
        GD.Print("todo: procedurally generate map");
    }

    private void ChangeView(PackedScene scene)
    {
        if (currentView.HasAnyChild())
        {
            currentView.GetChild(0).QueueFree();
        }

        GetTree().Paused = false;
        var newView = scene.Instantiate();
        currentView.AddChild(newView);
    }

    private void SubscribeEvents()
    {
        var events = EventBusOwner.Events;
        events.BattleWon += OnBattleWon;
        events.BattleRewardExited += OnBattleRewardExited;
        events.CampfireExited += OnCampfireExited;
        events.MapExited += OnMapExited;
        events.ShopExited += OnShopExited;
        events.TreasureRoomExited += OnTreasureRoomExited;

        #region debug buttons

        battleButton.Pressed += OnBattleButtonPressed;
        campfireButton.Pressed += OnCampfireButtonPressed;
        mapButton.Pressed += OnMapButtonPressed;
        battleRewardButton.Pressed += OnBattleRewardButtonPressed;
        shopButton.Pressed += OnShopButtonPressed;
        treasureRoomButton.Pressed += OnTreasureRoomButtonPressed;

        #endregion
    }

    private void UnsubscribeEvents()
    {
        var events = EventBusOwner.Events;
        events.BattleWon -= OnBattleWon;
        events.BattleRewardExited -= OnBattleRewardExited;
        events.CampfireExited -= OnCampfireExited;
        events.MapExited -= OnMapExited;
        events.ShopExited -= OnShopExited;
        events.TreasureRoomExited -= OnTreasureRoomExited;

        #region debug buttons

        battleButton.Pressed -= OnBattleButtonPressed;
        campfireButton.Pressed -= OnCampfireButtonPressed;
        mapButton.Pressed -= OnMapButtonPressed;
        battleRewardButton.Pressed -= OnBattleRewardButtonPressed;
        shopButton.Pressed -= OnShopButtonPressed;
        treasureRoomButton.Pressed -= OnTreasureRoomButtonPressed;

        #endregion
    }

    private void OnBattleWon() => ChangeView(BattleRewardScene);
    private void OnBattleRewardExited() => ChangeView(MapScene);
    private void OnCampfireExited() => ChangeView(MapScene);
    private void OnShopExited() => ChangeView(MapScene);
    private void OnTreasureRoomExited() => ChangeView(MapScene);

    #region debug button handlers

    private void OnBattleButtonPressed() => ChangeView(BattleScene);
    private void OnCampfireButtonPressed() => ChangeView(CampfireScene);
    private void OnMapButtonPressed() => ChangeView(MapScene);
    private void OnBattleRewardButtonPressed() => ChangeView(BattleRewardScene);
    private void OnShopButtonPressed() => ChangeView(ShopScene);
    private void OnTreasureRoomButtonPressed() => ChangeView(TreasureRoomScene);

    #endregion

    private void OnMapExited()
    {
        // todo: change view on map exiting
        GD.Print("exit from map, change view based on room type");
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}