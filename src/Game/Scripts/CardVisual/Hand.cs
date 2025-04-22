using Godot;

namespace CardGameV1.CardVisual;

public partial class Hand : HBoxContainer
{
    public override void _Ready()
    {
        foreach (var child in GetChildren())
        {
            if (child is not CardUI cardUI) continue;

            cardUI.ReparentRequested += OnCardUIReparentRequested;
            cardUI.Parent = this;
        }
    }

    private void OnCardUIReparentRequested(CardUI cardUI) => cardUI.Reparent(this);
}