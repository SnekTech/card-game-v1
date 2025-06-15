using System;
using CardGameV1.Constants;
using CardGameV1.CustomResources;
using CardGameV1.EventBus;
using CardGameV1.Map;
using CardGameV1.MyExtensions;
using CardGameV1.UI.CardPileDisplay;
using Godot;
using GodotUtilities;

namespace CardGameV1.UI.Run;

[Scene]
public partial class Run : Node
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

    #region packed scenes

    private static readonly PackedScene BattleScene = SnekUtility.LoadScene(ScenePath.Battle);
    private static readonly PackedScene BattleRewardScene = SnekUtility.LoadScene(ScenePath.BattleReward);
    private static readonly PackedScene CampfireScene = SnekUtility.LoadScene(ScenePath.Campfire);
    private static readonly PackedScene ShopScene = SnekUtility.LoadScene(ScenePath.Shop);
    private static readonly PackedScene TreasureRoomScene = SnekUtility.LoadScene(ScenePath.TreasureRoom);

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

    private Node ChangeView(PackedScene scene)
    {
        if (currentView.HasAnyChild())
        {
            currentView.GetChild(0).QueueFree();
        }

        GetTree().Paused = false;
        var newView = scene.Instantiate();
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

    private void OnBattleWon()
    {
        var rewardScene = (BattleReward.BattleReward)ChangeView(BattleRewardScene);
        rewardScene.RunStats = _runStats;
        rewardScene.CharacterStats = Character;

        // todo: remove temporary, rewards will come from real battle encounter data
        rewardScene.AddGoldReward(77);
        rewardScene.AddCardReward();
    }

    private void OnBattleRewardExited() => ShowMap();
    private void OnCampfireExited() => ShowMap();
    private void OnShopExited() => ShowMap();
    private void OnTreasureRoomExited() => ShowMap();

    private void OnDeckButtonPressed() => deckView.ShowCurrentView("Deck");

    #region debug button handlers

    private void OnBattleButtonPressed() => ChangeView(BattleScene);
    private void OnCampfireButtonPressed() => ChangeView(CampfireScene);
    private void OnMapButtonPressed() => ShowMap();
    private void OnBattleRewardButtonPressed() => ChangeView(BattleRewardScene);
    private void OnShopButtonPressed() => ChangeView(ShopScene);
    private void OnTreasureRoomButtonPressed() => ChangeView(TreasureRoomScene);

    #endregion

    private void OnMapExited(Room room)
    {
        switch (room.Type)
        {
            case RoomType.Monster:
                ChangeView(BattleScene);
                break;
            case RoomType.Treasure:
                ChangeView(TreasureRoomScene);
                break;
            case RoomType.Campfire:
                ChangeView(CampfireScene);
                break;
            case RoomType.Shop:
                ChangeView(ShopScene);
                break;
            case RoomType.Boss:
                ChangeView(BattleScene);
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