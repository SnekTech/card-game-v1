using CardGameV1.Constants;
using CardGameV1.CustomResources;
using CardGameV1.EventBus;
using CardGameV1.MyExtensions;
using Godot;
using GodotUtilities;

namespace CardGameV1.UI.BattleReward;

[Scene]
public partial class BattleReward : Control
{
    [Node]
    private VBoxContainer rewards = null!;
    [Node]
    private Button backButton = null!;

    private static readonly PackedScene RewardButtonScene = GD.Load<PackedScene>(ScenePath.RewardButton);
    private static readonly Texture2D GoldIcon = GD.Load<Texture2D>("res://art/gold.png");
    private static readonly Texture2D CardIcon = GD.Load<Texture2D>("res://art/rarity.png");
    
    public RunStats RunStats { private get; set; } = new();

    public override void _Ready()
    {
        rewards.ClearChildren();
        
        // todo: remove these test code
        RunStats.GoldChanged += ()=> GD.Print($"gold: {RunStats.Gold}");
        AddGoldReward(77);
    }

    public override void _EnterTree()
    {
        backButton.Pressed += OnBackButtonPressed;
    }

    public override void _ExitTree()
    {
        backButton.Pressed -= OnBackButtonPressed;
    }

    private void AddGoldReward(int amount)
    {
        var goldRewardButton = RewardButtonScene.Instantiate<RewardButton>();
        goldRewardButton.RewardIcon = GoldIcon;
        goldRewardButton.RewardText = $"{amount} gold";
        goldRewardButton.Pressed += () => RunStats.Gold += amount;
        Callable.From(()=> rewards.AddChild(goldRewardButton)).CallDeferred();
    }

    private void OnBackButtonPressed() => EventBusOwner.Events.EmitBattleRewardExited();

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}