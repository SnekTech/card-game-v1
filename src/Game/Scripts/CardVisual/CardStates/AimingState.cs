using CardGameV1.Constants;
using Godot;

namespace CardGameV1.CardVisual.CardStates;

public class AimingState(CardStateMachine cardDragStateMachine) : CardState(cardDragStateMachine)
{
    private const int MouseSnapbackThresholdY = 138;

    public override void OnEnter()
    {
        CardUI.Targets.Clear();
        var offset = new Vector2(CardUI.Parent.Size.X / 2, -CardUI.Size.Y / 2);
        offset.X -= CardUI.Size.X / 2;
        CardUI.AnimateToPosition(CardUI.Parent.GlobalPosition + offset, 0.2f);
        CardUI.MonitoringDrop = false;

        CardEvents.EmitCardAimStared(CardUI);
    }

    public override void OnExit()
    {
        CardEvents.EmitCardAimEnded(CardUI);
    }

    public override void OnInput(InputEvent inputEvent)
    {
        var mouseMoved = inputEvent is InputEventMouseMotion;
        var mouseTooLow = CardUI.GetGlobalMousePosition().Y > MouseSnapbackThresholdY;
        
        var shouldCancel = (mouseMoved && mouseTooLow) || inputEvent.IsActionPressed(InputActionNames.RightMouse);
        var playConfirmed = inputEvent.IsActionPressed(InputActionNames.LeftMouse) || inputEvent.IsActionReleased(InputActionNames.LeftMouse);

        if (shouldCancel)
        {
            ChangeState<BaseState>();
        }
        else if (playConfirmed)
        {
            ChangeState<ReleasedState>();
        }
    }
}