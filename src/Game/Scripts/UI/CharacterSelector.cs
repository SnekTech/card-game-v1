using CardGameV1.CustomResources;
using Godot;
using GodotUtilities;

namespace CardGameV1.UI;

[Scene]
public partial class CharacterSelector : Control
{
    [Node]
    private Label title = null!;
    [Node]
    private Label description = null!;
    [Node]
    private TextureRect characterPortrait = null!;
    [Node]
    private Button startButton = null!;
    [Node]
    private Button warriorButton = null!;
    [Node]
    private Button wizardButton = null!;
    [Node]
    private Button assassinButton = null!;

    private static readonly CharacterStats WarriorStats =
        GD.Load<CharacterStats>("res://characters/warrior/warrior.tres");
    private static readonly CharacterStats WizardStats =
        GD.Load<CharacterStats>("res://characters/wizard/wizard.tres");
    private static readonly CharacterStats AssassinStats =
        GD.Load<CharacterStats>("res://characters/assassin/assassin.tres");


    private CharacterStats CurrentCharacter
    {
        get => _currentCharacter;
        set
        {
            _currentCharacter = value;
            UpdateVisuals(value);
        }
    }

    private CharacterStats _currentCharacter = WarriorStats;

    public override void _Ready()
    {
        CurrentCharacter = WarriorStats;
    }

    public override void _EnterTree()
    {
        startButton.Pressed += OnStartButtonPressed;
        warriorButton.Pressed += OnWarriorButtonPressed;
        wizardButton.Pressed += OnWizardButtonPressed;
        assassinButton.Pressed += OnAssassinButtonPressed;
    }

    public override void _ExitTree()
    {
        startButton.Pressed -= OnStartButtonPressed;
        warriorButton.Pressed -= OnWarriorButtonPressed;
        wizardButton.Pressed -= OnWizardButtonPressed;
        assassinButton.Pressed -= OnAssassinButtonPressed;
    }

    private void UpdateVisuals(CharacterStats characterStats)
    {
        title.Text = characterStats.CharacterName;
        description.Text = characterStats.Description;
        characterPortrait.Texture = characterStats.Portrait;
    }

    private void OnStartButtonPressed()
    {
        GD.Print($"Start new Run with {CurrentCharacter.CharacterName}");
    }

    private void OnWarriorButtonPressed() => CurrentCharacter = WarriorStats;

    private void OnWizardButtonPressed() => CurrentCharacter = WizardStats;

    private void OnAssassinButtonPressed() => CurrentCharacter = AssassinStats;

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}