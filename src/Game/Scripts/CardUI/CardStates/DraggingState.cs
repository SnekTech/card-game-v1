using Godot;

namespace CardGameV1.CardUI.CardStates;

public class DraggingState(CardDragStateMachine cardDragStateMachine) : CardState(cardDragStateMachine)
{
    public override void OnEnter()
    {
        var newParent = CardUI.GetTree().GetFirstNodeInGroup("ui_layer");
        if (newParent != null)
            CardUI.Reparent(newParent);
        
        CardUI.SetDebugInfo(Colors.NavyBlue, nameof(DraggingState));
    }

    public override void OnInput(InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseMotion mouseMotion)
        {
            CardUI.GlobalPosition = mouseMotion.GlobalPosition - CardUI.PivotOffset;
        }

        var cancel = inputEvent.IsActionPressed("right_mouse");
        var confirm = inputEvent.IsActionPressed("left_mouse") || inputEvent.IsActionReleased("left_mouse");
        if (cancel)
        {
            ChangeState<BaseState>();
        }
        else if (confirm)
        {
            CardUI.GetViewport().SetInputAsHandled();
            ChangeState<ReleasedState>();
        }
    }
}