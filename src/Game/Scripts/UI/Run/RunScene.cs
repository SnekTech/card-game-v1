using System;
using CardGameV1.CustomResources;
using CardGameV1.EventBus;
using CardGameV1.Map;
using CardGameV1.MyExtensions;
using CardGameV1.UI.BattleReward;
using CardGameV1.UI.Campfire;
using CardGameV1.UI.CardPileDisplay;
using CardGameV1.UI.Shop;
using CardGameV1.UI.TreasureRoom;
using Godot;
using GodotUtilities;

namespace CardGameV1.UI.Run;

[Scene]
public partial class RunScene : Node
{
    [Export]
    private RunStartup runStartup = null!;

    [Node]
    private MapScene map = null!;
    [Node]
    private Node currentView = null!;
    [Node]
    private GoldUI goldUI = null!;
    [Node]
    private CardPileOpener deckButton = null!;
    [Node]
    private CardPileView deckView = null!;

    #region debug buttons

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

    #endregion

    private RunStats _runStats = null!;
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

    public override void _ExitTree()
    {
        UnsubscribeEvents();
    }

    private void StartRun()
    {
        _runStats = new RunStats();

        SubscribeEvents();
        SetupTopBar();
        map.GenerateNewMap();
        map.UnlockFloor(0);
    }

    private void SetupTopBar()
    {
        goldUI.RunStats = _runStats;
        deckButton.CardPile = Character.Deck;
        deckView.CardPile = Character.Deck;
    }

    private T ChangeView<T>() where T : Node
    {
        if (currentView.HasAnyChild())
        {
            currentView.GetChild(0).QueueFree();
        }

        GetTree().Paused = false;
        var newView = SceneFactory.Instantiate<T>();
        currentView.AddChild(newView);
        map.HideMap();

        return newView;
    }

    private void ShowMap()
    {
        if (currentView.HasAnyChild())
        {
            currentView.GetChild(0).QueueFree();
        }

        map.ShowMap();
        map.UnlockNextRooms();
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

        deckButton.Pressed += OnDeckButtonPressed;

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

        deckButton.Pressed -= OnDeckButtonPressed;

        #region debug buttons

        battleButton.Pressed -= OnBattleButtonPressed;
        campfireButton.Pressed -= OnCampfireButtonPressed;
        mapButton.Pressed -= OnMapButtonPressed;
        battleRewardButton.Pressed -= OnBattleRewardButtonPressed;
        shopButton.Pressed -= OnShopButtonPressed;
        treasureRoomButton.Pressed -= OnTreasureRoomButtonPressed;

        #endregion
    }

    private void OnBattleRoomEntered(Room room)
    {
        var battleScene = ChangeView<Battle>();
        battleScene.CharacterStats = Character;
        battleScene.BattleStats = room.BattleStats!;
        battleScene.StartBattle();
    }

    private void OnCampfireEntered()
    {
        var campfire = ChangeView<CampfireScene>();
        campfire.CharacterStats = Character;
    }

    private void OnBattleWon()
    {
        var rewardScene = ChangeView<BattleRewardScene>();
        rewardScene.RunStats = _runStats;
        rewardScene.CharacterStats = Character;

        rewardScene.AddGoldReward(map.LastRoom!.BattleStats!.GoldRewardRoll);
        rewardScene.AddCardReward();
    }

    private void OnBattleRewardExited() => ShowMap();
    private void OnCampfireExited() => ShowMap();
    private void OnShopExited() => ShowMap();
    private void OnTreasureRoomExited() => ShowMap();

    private void OnDeckButtonPressed() => deckView.ShowCurrentView("Deck");

    #region debug button handlers

    private void OnBattleButtonPressed() => ChangeView<Battle>();
    private void OnCampfireButtonPressed() => ChangeView<CampfireScene>();
    private void OnMapButtonPressed() => ShowMap();
    private void OnBattleRewardButtonPressed() => ChangeView<BattleRewardScene>();
    private void OnShopButtonPressed() => ChangeView<ShopScene>();
    private void OnTreasureRoomButtonPressed() => ChangeView<TreasureRoomScene>();

    #endregion

    private void OnMapExited(Room room)
    {
        switch (room.Type)
        {
            case RoomType.Monster:
                OnBattleRoomEntered(room);
                break;
            case RoomType.Treasure:
                ChangeView<TreasureRoomScene>();
                break;
            case RoomType.Campfire:
                OnCampfireEntered();
                break;
            case RoomType.Shop:
                ChangeView<ShopScene>();
                break;
            case RoomType.Boss:
                OnBattleRoomEntered(room);
                break;
            case RoomType.NotAssigned:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(room), "room type out of range");
        }
    }

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}