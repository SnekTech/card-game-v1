using CardGameV1.Constants;
using CardGameV1.EventBus;

namespace CardGameV1.CardVisual.CardStates;

public class DraggingState(CardStateMachine cardStateMachine) : CardState(cardStateMachine)
{
    private const float DragThresholdMin = 0.05f;
    private bool _minDragThresholdHasElapsed;

    public override void OnEnter()
    {
        var newParent = CardUI.GetTree().GetFirstNodeInGroup(GroupNames.UILayer);
        if (newParent != null)
            CardUI.Reparent(newParent);

        CardUI.SetPanelStyleBox(CardUI.DraggingStyleBox);
        EventBusOwner.CardEvents.EmitCardDragStared(CardUI);
        
        _minDragThresholdHasElapsed = false;
        var timer = CardUI.GetTree().CreateTimer(DragThresholdMin, false);
        timer.Timeout += () => _minDragThresholdHasElapsed = true;
    }

    public override void OnExit()
    {
        EventBusOwner.CardEvents.EmitCardDragEnded(CardUI);
    }

    public override void OnInput(InputEvent inputEvent)
    {
        var isCardSingleTargeted = CardUI.Card.IsSingleTargeted;
        var mouseMoved = inputEvent is InputEventMouseMotion;
        var shouldCancel = inputEvent.IsActionPressed(InputActions.RightMouse);
        var playConfirmed = inputEvent.IsActionPressed(InputActions.LeftMouse) ||
                            inputEvent.IsActionReleased(InputActions.LeftMouse);

        if (isCardSingleTargeted && mouseMoved && CardUI.IsOverlappingDropArea)
        {
            ChangeState<AimingState>();
            return;
        }

        if (mouseMoved)
        {
            CardUI.GlobalPosition = CardUI.GetGlobalMousePosition() - CardUI.PivotOffset;
        }

        if (shouldCancel)
        {
            ChangeState<BaseState>();
        }
        else if (_minDragThresholdHasElapsed && playConfirmed)
        {
            CardUI.GetViewport().SetInputAsHandled();
            ChangeState<ReleasedState>();
        }
    }
}