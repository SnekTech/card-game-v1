using Godot;

namespace CardGameV1.CardVisual.CardStates;

public class DraggingState(CardStateMachine cardStateMachine) : CardState(cardStateMachine)
{
    private const float DragThresholdMin = 0.05f;
    private bool _minDragThresholdHasElapsed;
    
    public override void OnEnter()
    {
        var newParent = CardUI.GetTree().GetFirstNodeInGroup("ui_layer");
        if (newParent != null)
            CardUI.Reparent(newParent);
        
        CardUI.SetDebugInfo(Colors.NavyBlue, nameof(DraggingState));

        _minDragThresholdHasElapsed = false;
        var timer = CardUI.GetTree().CreateTimer(DragThresholdMin, false);
        timer.Timeout += () => _minDragThresholdHasElapsed = true;
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
        else if (_minDragThresholdHasElapsed && confirm)
        {
            CardUI.GetViewport().SetInputAsHandled();
            ChangeState<ReleasedState>();
        }
    }
}