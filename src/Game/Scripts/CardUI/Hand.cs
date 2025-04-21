using Godot;

namespace CardGameV1.CardUI;

public partial class Hand : HBoxContainer
{
    public override void _Ready()
    {
        foreach (var child in GetChildren())
        {
            if (child is CardUI cardUI)
            {
                cardUI.ReparentRequested += OnCardUIReparentRequested;
            }
        }
    }

    private void OnCardUIReparentRequested(CardUI cardUI) => cardUI.Reparent(this);
}