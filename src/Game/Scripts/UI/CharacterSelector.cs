using CardGameV1.Character;
using CardGameV1.CustomResources;
using GodotUtilities;

namespace CardGameV1.UI;

[Scene]
public partial class CharacterSelector : Control
{
    [Export]
    private RunStartup runStartup = null!;

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

    private static readonly PackedScene RunScene = GD.Load<PackedScene>(Run.RunScene.TscnFilePath);

    private CharacterStats CurrentCharacter
    {
        get => _currentCharacter;
        set
        {
            _currentCharacter = value;
            UpdateVisuals(value);
        }
    }

    private CharacterStats _currentCharacter = CharacterPool.Warrior;

    public override void _Ready()
    {
        CurrentCharacter = CharacterPool.Warrior;
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
        runStartup.Type = RunStartup.RunType.NewRun;
        runStartup.PickedCharacter = CurrentCharacter;
        GetTree().ChangeSceneToPacked(RunScene);
    }

    private void OnWarriorButtonPressed() => CurrentCharacter = CharacterPool.Warrior;
    private void OnWizardButtonPressed() => CurrentCharacter = CharacterPool.Wizard;
    private void OnAssassinButtonPressed() => CurrentCharacter = CharacterPool.Assassin;

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            WireNodes();
        }
    }
}