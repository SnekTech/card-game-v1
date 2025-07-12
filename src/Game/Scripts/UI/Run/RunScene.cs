using CardGameV1.CustomResources;
using CardGameV1.CustomResources.Run;
using CardGameV1.EventBus;
using CardGameV1.Map;
using CardGameV1.MyExtensions;
using CardGameV1.UI.BattleReward;
using CardGameV1.UI.Campfire;
using CardGameV1.UI.Shop;
using CardGameV1.UI.TreasureRoom;

namespace CardGameV1.UI.Run;

[SceneTree]
public partial class RunScene : Node
{
    [Export]
    private RunStartup runStartup = null!;

    private RunStats _runStats = null!;
    private CharacterStats Character { get; set; } = null!;

    #region lifecycle

    public override void _Ready()
    {
        switch (runStartup.Type)
        {
            case RunStartup.RunType.NewRun:
                Character = runStartup.PickedCharacter.CreateInstance();
                Character.StatsChanged += OnCharacterStatsChanged;
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

    public override void _EnterTree()
    {
        SubscribeEvents();
    }

    public override void _ExitTree()
    {
        UnsubscribeEvents();
    }

    #endregion

    private void StartRun()
    {
        _runStats = new RunStats();

        SetupTopBar();
        Map.GenerateNewMap();
        Map.UnlockFloor(0);
    }

    private void SetupTopBar()
    {
        HealthUI.UpdateStats(Character);
        GoldUI.RunStats = _runStats;
        DeckButton.CardPile = Character.Deck;
        DeckView.CardPile = Character.Deck;
    }

    private T ChangeView<T>() where T : Node
    {
        if (CurrentView.HasAnyChild())
        {
            CurrentView.GetChild(0).QueueFree();
        }

        GetTree().Paused = false;
        var newView = SceneFactory.Instantiate<T>();
        CurrentView.AddChild(newView);
        Map.HideMap();

        return newView;
    }

    private void ShowMap()
    {
        if (CurrentView.HasAnyChild())
        {
            CurrentView.GetChild(0).QueueFree();
        }

        Map.ShowMap();
        Map.UnlockNextRooms();
    }

    private void SubscribeEvents()
    {
        var events = EventBusOwner.Events;
        events.BattleWon += OnBattleWon;
        events.BattleLost += OnBattleLost;
        events.BattleRewardExited += OnBattleRewardExited;
        events.CampfireExited += OnCampfireExited;
        events.MapExited += OnMapExited;
        events.ShopExited += OnShopExited;
        events.TreasureRoomExited += OnTreasureRoomExited;

        DeckButton.Pressed += OnDeckButtonPressed;
    }

    private void UnsubscribeEvents()
    {
        var events = EventBusOwner.Events;
        events.BattleWon -= OnBattleWon;
        events.BattleLost -= OnBattleLost;
        events.BattleRewardExited -= OnBattleRewardExited;
        events.CampfireExited -= OnCampfireExited;
        events.MapExited -= OnMapExited;
        events.ShopExited -= OnShopExited;
        events.TreasureRoomExited -= OnTreasureRoomExited;

        DeckButton.Pressed -= OnDeckButtonPressed;
        Character.StatsChanged -= OnCharacterStatsChanged;
    }

    private void OnCharacterStatsChanged()
    {
        HealthUI.UpdateStats(Character);
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

        rewardScene.AddGoldReward(Map.LastRoom!.BattleStats!.GoldRewardRoll);
        rewardScene.AddCardReward();
    }

    private void OnBattleLost()
    {
        // todo: handle losing this run
        GD.Print("should lose this run");
        GetTree().Paused = false;
        OnCampfireEntered();
    }

    private void OnBattleRewardExited() => ShowMap();
    private void OnCampfireExited() => ShowMap();
    private void OnShopExited() => ShowMap();
    private void OnTreasureRoomExited() => ShowMap();

    private void OnDeckButtonPressed() => DeckView.ShowCurrentView("Deck");

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
}