using CardGameV1.Constants;
using CardGameV1.EventBus;
using Godot;

namespace CardGameV1.CardVisual.CardStates;

public class AimingState(CardStateMachine cardDragStateMachine) : CardState(cardDragStateMachine)
{
    private const int MouseSnapbackThresholdY = 138;
    private readonly CardEventBus _eventBus = EventBusOwner.CardEventBus;

    public override void OnEnter()
    {
        CardUI.SetDebugInfo(Colors.WebPurple, nameof(AimingState));
        CardUI.Targets.Clear();
        var offset = new Vector2(CardUI.Parent.Size.X / 2, -CardUI.Size.Y / 2);
        offset.X -= CardUI.Size.X / 2;
        CardUI.AnimateToPosition(CardUI.Parent.GlobalPosition + offset, 0.2f);
        CardUI.MonitoringDrop = false;

        _eventBus.EmitCardAimStared(CardUI);
    }

    public override void OnExit()
    {
        _eventBus.EmitCardAimEnded(CardUI);
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